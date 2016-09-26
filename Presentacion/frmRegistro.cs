using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logica;
using System.Speech.Synthesis;

namespace Presentacion
{
    public partial class frmRegistro : Form
    {
        private ClsEscanner oEscanner;
        private ClsIdentificacion oIdentificacion;
        private List<Empleado> lstEmpleados;
        public frmRegistro(List<Empleado> lstEmpleados)
        {
            this.lstEmpleados = lstEmpleados;
            InitializeComponent();
            this.timer1.Interval = 1000;
            this.lblReloj.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
            timerLector.Start();
            this.oEscanner = new ClsEscanner();
            if (!this.oEscanner.iniciarScanner())
                lblMensaje.Text = "Escanner no encontrado";
                
            this.oIdentificacion = new ClsIdentificacion(ref this.lstEmpleados);
            this.oIdentificacion.crearDataBase();

        }

        private void darBienvenida()
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            //synth.SelectVoice
            synth.Speak("Acceso correcto ");
        }
        
        
        private void capturarHuella()
        {
            String fecha = DateTime.Now.ToString("yyyy_MM_dd_HH_MM_ss");
            ClsRetorno oRetorno = this.oEscanner.getImage(Sesion.getRuta() + "\\" + fecha, Huella.Dedo.Izq_Indice);
            if(oRetorno ==null)
                return;
            Empleado empleadoX = new Empleado();
            empleadoX.Fingerprints.Add(new Huella( 1, oRetorno.ruta, Huella.Dedo.Izq_Indice));
            Empleado empleadoIdentificado = this.oIdentificacion.identificarEmpleado(empleadoX);


            if (empleadoIdentificado != null)
            {
                float score = this.oIdentificacion.score;
                //MessageBox.Show("Bienvenido " + empleadoIdentificado.Nombres + "\r\nSimilitud: " + score);
                darBienvenida();
                lblMensaje.Text = empleadoIdentificado.Nombres + " " + empleadoIdentificado.ApellidoPat;
                new Registro() { 
                    idEmpleado =  empleadoIdentificado.EmpleadoID
                }.insertar();

            }
            else
                lblMensaje.Text = "Empleado no encontrado";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.lblReloj.Text = DateTime.Now.ToLongTimeString();
        }

        private void timerLector_Tick(object sender, EventArgs e)
        {
            //lblMensaje.Text = string.Empty;
            if (this.oEscanner.verificarDedo())
            {
                capturarHuella();
            }
        }

        private void frmRegistro_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.oEscanner.apagarScanner();
        }
    }
}
