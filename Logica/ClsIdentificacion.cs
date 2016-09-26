using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceAFIS.Simple;
using System.Windows.Media.Imaging;

namespace Logica
{
    public class ClsIdentificacion
    {
        private AfisEngine afis;
        private List<Empleado> lstEmpleados { get; set; }
        public float score { get; set; }
        public ClsIdentificacion(ref List<Empleado> lstEmpleados)
        {
            this.afis = new AfisEngine();
            this.afis.Threshold = 30;//default 25
            this.lstEmpleados = lstEmpleados;
        }
        public void crearDataBase()
        {
            lstEmpleados.ForEach(empleado => empleado.Fingerprints.ForEach(huella => huella.AsBitmapSource = getBitmapSource(((Huella)huella).ruta)));
            lstEmpleados.ForEach(empleado => this.afis.Extract(empleado));
        }

        private BitmapImage getBitmapSource(String ruta)
        { 
            return new BitmapImage(new Uri(ruta, UriKind.RelativeOrAbsolute));
        }

        public Empleado identificarEmpleado(Empleado empleadoX)
        {
            empleadoX.Fingerprints.ForEach(huella => huella.AsBitmapSource = getBitmapSource(((Huella)huella).ruta));
            this.afis.Extract(empleadoX);
            Empleado empleadoIdentificado = this.afis.Identify(empleadoX, this.lstEmpleados).FirstOrDefault() as Empleado;
            if (empleadoIdentificado != null)
                this.score = this.afis.Verify(empleadoX, empleadoIdentificado);
            return empleadoIdentificado;
        }
    }
}
