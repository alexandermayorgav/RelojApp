using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceAFIS.Simple;
using DAO;
using MySql.Data.MySqlClient;
using System.ComponentModel;
namespace Logica
{
    [Serializable]
    public    class Empleado :Person
    {
        [Browsable(false)]
        public int IdEmpleado { get; set; }
        [DisplayName("Nombre")]
        public String Nombres { get; set; }
        [DisplayName("Apellido Paterno")]
        public String ApellidoPat { get; set; }
        [DisplayName("Apellido Materno")]
        public String ApellidoMat { get; set; }
        [DisplayName("CURP")]
        public String CURP { get; set; }
        [NonSerialized]
        private ClsMySQL oMySql;

        public String getNombreCompleto()
        {
            return this.Nombres.ToUpper() + " " + this.ApellidoPat.ToUpper() + " " + this.ApellidoMat.ToUpper(); 
        }
        public Empleado()
        {
            this.oMySql = new ClsMySQL();
        }

        private void insertar()
        {
            MySqlTransaction trans = null;
            try
            {
                this.oMySql.abrirConexion();
                trans = this.oMySql.connection.BeginTransaction(); 
                MySqlCommand cmd = new MySqlCommand("INSERT INTO empleado(nombres, apellidoPat, apellidoMat,CURP) VALUES(@nombre,@apellidoPat,@apellidoMat, @CURP)", this.oMySql.connection);
                cmd.Parameters.AddWithValue("@nombre", this.Nombres);
                cmd.Parameters.AddWithValue("@apellidoPat", this.ApellidoPat);
                cmd.Parameters.AddWithValue("@apellidoMat", this.ApellidoMat);
                cmd.Parameters.AddWithValue("@CURP", this.CURP);
               // cmd.Transaction = trans;
            //    cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                this.IdEmpleado =Convert.ToInt32( cmd.LastInsertedId);
                foreach (Huella huella in this.Fingerprints)
                {
                    insertarHuella(huella,this.IdEmpleado, trans);
                }
                trans.Commit();
                
            }
            catch (MySqlException e)
            {
                trans.Rollback();
                this.oMySql.setError("Error al insertar en la tabla empleado", e.Message + "\r\n" + e.StackTrace);
            }
            finally { this.oMySql.cerrarConexion(); }
        
        }

        private void insertarHuella(Huella huella, int idEmpleado, MySqlTransaction trans)
        { 
            
            MySqlCommand cmd = new MySqlCommand("INSERT INTO empleadobiometrico ( idEmpleado, idBiometrico, ruta, fechaRegistro, estatus) VALUES (@idEmpleado, @idBiometrico, @ruta, @fechaRegistro, @estatus)", this.oMySql.connection,trans);
            cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
            cmd.Parameters.AddWithValue("@idBiometrico", (int)huella.dedo);
            cmd.Parameters.AddWithValue("@ruta", huella.ruta);
            cmd.Parameters.AddWithValue("@estatus", Huella.Estatus.activa);
            cmd.Parameters.AddWithValue("@fechaRegistro", DateTime.Now);
            cmd.ExecuteNonQuery();
        }

        private void eliminarHuella(Huella huella, MySqlTransaction trans)
        {
            MySqlCommand cmd = new MySqlCommand("DELETE empleadobiometrico WHERE idempleadobiometrico = @idempleadobiometrico", this.oMySql.connection, trans);
            cmd.Parameters.AddWithValue("@idempleadobiometrico", huella.idHuella);
            cmd.ExecuteNonQuery();
        }

        
        private void actualizar()
        {
            MySqlTransaction trans = null;
            try
            {
                this.oMySql.abrirConexion();
                trans = this.oMySql.connection.BeginTransaction();
                MySqlCommand cmd = new MySqlCommand("UPDATE empleado SET nombres = @nombre, apellidoPat = @apellidoPat, apellidoMat = @apellidoMat,CURP = @CURP WHERE idEmpleado = @idEmpleado", this.oMySql.connection);
                cmd.Parameters.AddWithValue("@nombre", this.Nombres);
                cmd.Parameters.AddWithValue("@apellidoPat", this.ApellidoPat);
                cmd.Parameters.AddWithValue("@apellidoMat", this.ApellidoMat);
                cmd.Parameters.AddWithValue("@CURP", this.CURP);
                cmd.Parameters.AddWithValue("@idEmpleado", this.IdEmpleado);
                // cmd.Transaction = trans;
                //    cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                this.IdEmpleado = Convert.ToInt32(cmd.LastInsertedId);
                foreach (Huella huella in this.Fingerprints.Where(item => ((Huella)item).estatus == Huella.Estatus.nueva))
                {
                    insertarHuella(huella, this.IdEmpleado, trans);
                }
                List<String> lstArchivos = new List<string>();
                foreach (Huella huella in this.Fingerprints.Where(item => ((Huella)item).estatus == Huella.Estatus.baja & ((Huella)item).idHuella !=0 ))
                {
                    eliminarHuella(huella, trans);
                    lstArchivos.Add(huella.ruta);
                }
                trans.Commit();

            }
            catch (MySqlException e)
            {
                trans.Rollback();
                this.oMySql.setError("Error al insertar en la tabla empleado", e.Message + "\r\n" + e.StackTrace);
            }
            finally { this.oMySql.cerrarConexion(); }
        }
        public bool guardar()
        {
            if (this.IdEmpleado == 0)
                insertar();
            else
                actualizar();
            return  this.oMySql.getError();
        }
        /// <summary>
        /// Obtiene los empleados registrados
        /// </summary>
        /// <returns></returns>
        public List<Empleado> getEmpleados()
        {
            List<Empleado> lstDatos = null;
            try
            {
                lstDatos = new List<Empleado>();
                this.oMySql.abrirConexion();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM empleado", this.oMySql.connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                Empleado empleado;
                while (reader.Read())
                {
                    empleado = new Empleado()
                    {
                        IdEmpleado = reader.GetInt32("idEmpleado"),
                        Nombres = reader.GetString("Nombres"),
                        ApellidoPat = reader.GetString("ApellidoPat"),
                        ApellidoMat = reader.GetString("ApellidoMat"),
                        CURP =  reader.GetString("CURP")
                    };
                    //getHuellasByIdEmpleado(ref empleado);
                    lstDatos.Add(empleado);
                }
                reader.Close();
                lstDatos.ForEach(item => getHuellasByIdEmpleado(ref item));

            }
            catch (Exception e)
            {

            }
            finally { this.oMySql.cerrarConexion(); }
            return lstDatos;
            
        }
        /// <summary>
        /// Obtiene las huellas relacionadas al empleado
        /// </summary>
        /// <param name="empleado"></param>
        private void getHuellasByIdEmpleado(ref Empleado empleado)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM empleadobiometrico WHERE idEmpleado = " + empleado.IdEmpleado.ToString(), this.oMySql.connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                Huella huella;
                while (reader.Read())
                {
                    huella = new Huella()
                    {
                        idHuella = reader.GetInt32("idempleadoBiometrico"),
                        ruta = reader.GetString("ruta"),
                        dedo = (Huella.Dedo)reader.GetInt32("idBiometrico"),
                        estatus = Huella.Estatus.activa
                    };
                    empleado.Fingerprints.Add(huella);
                }
                reader.Close();
            }
            catch (Exception e)
            { 
                
            }
            
        }
    }
}
