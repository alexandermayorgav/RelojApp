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
        public int idEmpleado { get; set; }
        public DateTime fechaRegistro { get; set; }

        public void insertar()
        {
            try
            {
                this.abrirConexion();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO registro (idEmpleado,fechaRegistro) VALUES (@idEmpleado, @fechaRegistro)", this.connection);
                cmd.Parameters.AddWithValue("@idEmpleado", this.idEmpleado);
                
                cmd.Parameters.AddWithValue("@fechaRegistro", DateTime.Now);
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

        
    }
}
