using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceAFIS.Simple;
namespace Logica
{
    [Serializable]
    public class Huella : Fingerprint
    {
        public Huella(int idHuella, String ruta, Dedo dedo)
        {
            this.idHuella = idHuella;
            this.ruta = ruta;
            this.dedo = dedo;
        }
        public Huella()
        { 
        
        }
        public int idHuella { get; set; }
        public String ruta { get; set; }
        /// <summary>
        /// es el IdBiometrico
        /// </summary>
        public Dedo dedo { get; set; }
        public Estatus estatus { get; set; }

        public enum Dedo
        {
            Izq_Menique = 1,
            Izq_Anular = 2,
            Izq_Medio = 3,
            Izq_Indice = 4,
            Izq_Pulgar = 5,
            Der_Menique = 6,
            Der_Anular = 7,
            Der_Medio = 8,
            Der_Indice = 9,
            Der_Pulgar = 10

        }
        public enum Estatus { activa,baja}
    }
}
