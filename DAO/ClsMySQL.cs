using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DAO
{
    public class ClsMySQL
    {
        public MySqlConnection connection { get; set; } 
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public ClsMySQL()
        {
            Initialize();
        }

        //Initialize values
        protected void Initialize()
        {
            server = "localhost";
            database = "reloj";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + 
		    database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }
        public void iniciarTransaccion()
        {
            this.connection.BeginTransaction();
        }

        public void confirmarTransaccion()
        {
            
        }

        public void abrirConexion()
        {
            this.connection.Open();
        }
        public void cerrarConexion()
        {
            if (this.connection!=null)
                this.connection.Close();
        }

        

    }
}
