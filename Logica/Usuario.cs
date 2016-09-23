using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;
using SourceAFIS.Simple;

namespace Logica
{
    public class Usuario : Person
    {
        public int idUsuario{ get; set; }
        public String nombre { get; set; }
        private ClsMySQL oMySQL;

        public Usuario()
        {
            this.oMySQL = new ClsMySQL();
        }

        

      
    }
}
