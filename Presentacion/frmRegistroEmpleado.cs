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

namespace Presentacion
{
    public partial class frmRegistroEmpleado : Form
    {
        private Logica.Empleado oEmpleado;
        private ClsEscanner oEscanner;
        private List<Logica.Empleado> lstEmpleados;
        /// <summary>
        /// Captura una imagen de una huella 
        /// </summary>
        private void getImage()
        {
            Huella.Dedo tipoDedo;
            Enum.TryParse<Huella.Dedo>(this.cmbDedos.SelectedValue.ToString(), out tipoDedo);

            if (this.oEmpleado.Fingerprints.Count(item => ((Huella)item).dedo == tipoDedo) != 0)
            {
                if (MessageBox.Show("El usuario ya cuenta con un registro para el dedo seleccionado,¿Desea capturarlo nuevamente?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    return;
                else
                    this.oEmpleado.Fingerprints.Remove(this.oEmpleado.Fingerprints.Where(huella => ((Huella)huella).dedo == tipoDedo).FirstOrDefault());
            }

            ClsRetorno oRetorno = this.oEscanner.getImage(Sesion.getRuta() + "\\" + txtCURP.Text.Trim(), tipoDedo);
            if (oRetorno == null)
                return;
   
            if (oRetorno.calidad < 50)
            {
                MessageBox.Show("La calidad de la captura es inferior a la aceptada, repita la captura");
                return;
            }
            this.oEmpleado.Fingerprints.Add(new Huella(this.oEmpleado.Fingerprints.Count + 1, oRetorno.ruta, tipoDedo));
            fillGridView();
        }
        /// <summary>
        /// Muestra las huellas registradas de un empleado
        /// </summary>
        private void fillGridView()
        {
            this.gridDatos.Rows.Clear();
            int row = 0;
            foreach (Huella huella in this.oEmpleado.Fingerprints)
            {
                this.gridDatos.Rows.Add();
                this.gridDatos.Rows[row].Cells[0].Value = huella.dedo;
                this.gridDatos.Rows[row].Cells[1].Value = huella.ruta;
                row++;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public frmRegistroEmpleado()
        {
            InitializeComponent();
            this.oEscanner = new ClsEscanner();
            this.oEscanner.iniciarScanner();
            this.cmbDedos.DataSource = Enum.GetValues(typeof(Huella.Dedo));
            this.cmbDedos.SelectedIndex = 0;
            this.oEmpleado =  new Logica.Empleado();
            this.lstEmpleados = new List<Logica.Empleado>();
            setBinding();
        }
        /// <summary>
        /// Agrega una huella al empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgregarHuella_Click(object sender, EventArgs e)
        {
            if (txtCURP.Text.Length != 0)
                getImage();
        }
        /// <summary>
        /// Guarda un empleado en la BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!validar())
                return;
            
            this.oEmpleado.guardar();
            MessageBox.Show("Guardado correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //this.Close();
        }
        /// <summary>
        /// Valida el formulario
        /// </summary>
        /// <returns></returns>
        private bool validar()
        {
            String mensaje = string.Empty;
            if (txtNombre.Text.Length==0)
            {
                mensaje += "Nombre\r\n";
            }
            if (txtApellidoPat.Text.Length == 0)
            {
                mensaje += "Apellido Paterno\r\n";
            }
            if (txtCURP.Text.Length == 0)
            {
                mensaje += "CURP\r\n";
            }
            if (this.oEmpleado.Fingerprints.Count != 0)
            {
                int numHuellas = 0;

                Sesion.getDedosRequeridos().ForEach(dedoRequerido => numHuellas += this.oEmpleado.Fingerprints.Where(huella => ((Huella)huella).dedo == dedoRequerido).Count());
                if (numHuellas!=Sesion.getDedosRequeridos().Count)
                {
                    mensaje+="Huellas digitales requeridas:\r\n";
                    Sesion.getDedosRequeridos().ForEach(dedo => mensaje += dedo.ToString() + "\r\n"); 
                }
            }
            else {
                mensaje += "Huellas Digitales\r\n";
            }
            if (mensaje.Length!=0)
            {
                MessageBox.Show("Los siguientes campos son requeridos:\r\n" + mensaje, Sesion.getAppNombre(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return mensaje.Length == 0;
        }

        private void setBinding()
        {
            this.txtNombre.DataBindings.Add("Text", this.oEmpleado,"Nombres");
            this.txtApellidoPat.DataBindings.Add("Text", this.oEmpleado, "ApellidoPat");
            this.txtApellidoMat.DataBindings.Add("Text",this.oEmpleado,"ApellidoMat");
            this.txtCURP.DataBindings.Add("Text", this.oEmpleado, "CURP");
        }

        private void frmRegistroEmpleado_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.oEscanner.apagarScanner();
        }

    }
}
