using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logica;
namespace Presentacion
{
    public static class Sesion
    {
        private static String ruta = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        private static String appNombre = "Reloj App";
        private static List<Huella.Dedo> lstHuellas;

        public static String getRuta()
        {
            return ruta.Substring(6);
        }

        public static String getAppNombre()
        {
            return appNombre;
        }

        public static List<Huella.Dedo> getDedosRequeridos()
        {
            lstHuellas = new List<Huella.Dedo>();
            lstHuellas.Add(Huella.Dedo.Der_Indice);
            lstHuellas.Add(Huella.Dedo.Izq_Indice);
            return lstHuellas;
        }
    }
}
