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
using System.Media;

namespace Presentacion
{
    public partial class frmRegistro : Form
    {
        private ClsEscanner oEscanner;
        private ClsIdentificacion oIdentificacion;
        private List<Empleado> lstEmpleados;
        private Registro oRegistro;
        private List<String> lstMensajes;
        private int contador = 0;
        SoundPlayer Player;

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
            this.Player = new SoundPlayer();
            Player.SoundLocation = @"C:\Windows\Media\chimes.wav";
        }

        private void reproducirMensaje(String mensaje)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            //synth.SelectVoice
            synth.Speak(mensaje);
        }
        
        
        private void capturarHuella()
        {
            this.lstMensajes = new List<string>();
            String fecha = DateTime.Now.ToString("yyyy_MM_dd_HH_MM_ss");
            ClsRetorno oRetorno = this.oEscanner.getImage(Sesion.getRuta() + "\\" + fecha, Huella.Dedo.Izq_Indice);
            if(oRetorno ==null)
                return;
            Empleado empleadoX = new Empleado();
            empleadoX.Fingerprints.Add(new Huella( 1, oRetorno.ruta, Huella.Dedo.Izq_Indice));
            Empleado empleadoIdentificado = this.oIdentificacion.identificarEmpleado(empleadoX);

            if (empleadoIdentificado != null)
            {
                this.oRegistro = new Registro()
                {
                    empleado = new Empleado()
                    {
                        IdEmpleado = empleadoIdentificado.IdEmpleado,
                    }
                };
                this.oRegistro.insertar();
                this.lstMensajes.Add(empleadoIdentificado.Nombres + " " + empleadoIdentificado.ApellidoPat + "\r\n" + this.oRegistro.fechaRegistro.ToLongTimeString());
                this.lstMensajes.Add("Acceso Correcto");
            }
            else
            {
                this.lstMensajes.Add("Empleado no encontrado");
                this.lstMensajes.Add("Acceso Incorrecto");
                this.lstMensajes.Add("Error");
            }
            eliminarArchivo(oRetorno.ruta);
        }

        /// <summary>
        /// Elimina el archivo temporal creado para la identificacion
        /// </summary>
        /// <param name="ruta"></param>
        private void eliminarArchivo(String ruta)
        {
            if (System.IO.File.Exists(ruta))
            {
                System.IO.File.Delete(ruta);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.lblReloj.Text = DateTime.Now.ToLongTimeString();
            if (this.contador == 5)
            {
                this.contador = 0;
                lblMensaje.Text = "";
                pbImagen.Image = null;
            }
            this.contador++;
        }

        private void timerLector_Tick(object sender, EventArgs e)
        {
            if (this.oEscanner.verificarDedo())
            {
                worker.RunWorkerAsync();
            }
        }

        private void frmRegistro_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.oEscanner.apagarScanner();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            capturarHuella();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.contador = 0;
            lblMensaje.Text = this.lstMensajes[0];

            if (this.lstMensajes.Count > 2)
            {
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                pbImagen.Image = Presentacion.Properties.Resources.error;
                
                Player.Play();

            }
            else
            {
                lblMensaje.ForeColor = System.Drawing.Color.Black;
                SystemSounds.Beep.Play();
                pbImagen.Image = Presentacion.Properties.Resources.check;
            }
            //reproducirMensaje(this.lstMensajes[1]);
        }
    }
}
