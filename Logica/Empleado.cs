using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceAFIS.Simple;
using DAO;
using MySql.Data.MySqlClient;
namespace Logica
{
    [Serializable]
    public    class Empleado :Person
    {
        public int EmpleadoID { get; set; }
        public String Nombres { get; set; }
        public String ApellidoPat { get; set; }
        public String ApellidoMat { get; set; }
        public String CURP { get; set; }

        private ClsMySQL oMySql;

        public Empleado()
        {
            this.oMySql = new ClsMySQL();
        }

        public void insertar()
        {
            MySqlTransaction trans = null;
            try
            {
                this.oMySql.abrirConexion();
                trans = this.oMySql.connection.BeginTransaction(); 
                MySqlCommand cmd = new MySqlCommand("INSERT INTO empleado(nombre, apellidoPat, apellidoMat,CURP) VALUES(@nombre,@apellidoPat,@apellidoMat, @CURP)", this.oMySql.connection,trans);
                cmd.Parameters.AddWithValue("@nombre", this.Nombres);
                cmd.Parameters.AddWithValue("@apellidoPat", this.ApellidoPat);
                cmd.Parameters.AddWithValue("@apellidoMay", this.ApellidoMat);
                cmd.Parameters.AddWithValue("@CURP", this.CURP);
                cmd.Transaction = trans;
                this.EmpleadoID = (int)cmd.ExecuteScalar();
                foreach (Huella huella in this.Fingerprints)
                {
                    insertarHuella(huella,this.EmpleadoID, trans);
                }
                trans.Commit();
                
            }
            catch (Exception e)
            {
                trans.Rollback();
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

        public void actualizar()
        { 
        
        }
        public void guardar()
        {
            if (this.EmpleadoID == 0)
                insertar();
            else
                actualizar();
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
                        EmpleadoID = reader.GetInt32("EmpleadoID"),
                        Nombres = reader.GetString("Nombres"),
                        ApellidoPat = reader.GetString("ApellidoPat"),
                        ApellidoMat = reader.GetString("ApellidoMat"),
                        CURP =  reader.GetString("CURP")
                    };
                    getHuellasByIdEmpleado(ref empleado);
                    lstDatos.Add(empleado);
                }

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
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM empleadobiometrico WHERE idEmpleado = " + empleado.EmpleadoID.ToString(), this.oMySql.connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                Huella huella;
                while (reader.Read())
                {
                    huella = new Huella()
                    {
                        idHuella = reader.GetInt32("idempleadoBiometrico"),
                        ruta = reader.GetString("rutaArchivo"),
                        dedo = (Huella.Dedo)reader.GetInt32("idBiometrico")
                    };
                    empleado.Fingerprints.Add(huella);
                }
            }
            catch (Exception e)
            { 
                
            }
            
        }
    }
}
