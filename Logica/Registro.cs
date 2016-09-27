using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;
using MySql.Data.MySqlClient;
namespace Logica
{
    public class Registro:ClsMySQL
    {
        public int idRegistro { get; set; }
        public Empleado empleado { get; set; }
        public DateTime fechaRegistro { get; set; }

        public void insertar()
        {
            try
            {
                this.abrirConexion();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO registro (idEmpleado,fechaRegistro) VALUES (@idEmpleado, @fechaRegistro)", this.connection);
                cmd.Parameters.AddWithValue("@idEmpleado", this.empleado.IdEmpleado);
                this.fechaRegistro = DateTime.Now;
                cmd.Parameters.AddWithValue("@fechaRegistro", fechaRegistro);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                
                throw;
            }
                finally{
                this.cerrarConexion();            
            }

        }

        public List<Registro> getRegistrosByFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            List<Registro> lstDatos = new List<Registro>();
            try
            {
                this.abrirConexion();
                MySqlCommand cmd = new MySqlCommand("SELECT E.*, R.fechaRegistro FROM registro R inner join empleado E on R.idEmpleado = E.idEmpleado WHERE R.fechaRegistro BETWEEN @fechaInicio AND @fechaFin ORDER BY fechaRegistro ASC", this.connection);
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lstDatos.Add(
                        new Registro()
                        {
                            empleado = new Empleado() { 
                                IdEmpleado  = reader.GetInt32("idEmpleado"),
                                Nombres = reader.GetString("nombres"),
                                ApellidoPat = reader.GetString("apellidoPat"),
                                ApellidoMat = reader.GetString("apellidoMat")
                            },
                            fechaRegistro =  reader.GetDateTime("fechaRegistro")
                        });
                }

            }
            catch (Exception e)
            {

                throw;
            }
            finally { this.cerrarConexion(); }
            return lstDatos;
        }

        
    }
}
