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
        private Empleado oEmpleado;
        private ClsEscanner oEscanner;
        private List<Empleado> lstEmpleados;

        private void getImage()
        {
            Huella.Dedo tipoDedo;
            Enum.TryParse<Huella.Dedo>(this.cmbDedos.SelectedValue.ToString(), out tipoDedo);

            if (this.oEmpleado.Fingerprints.Count(item => ((Huella)item).dedo == tipoDedo) != 0)
            {
                if (MessageBox.Show("El usuario ya cuenta con un registro para el dedo seleccionado,¿Desea capturarlo nuevamente?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    return;
            }

            ClsRetorno oRetorno = this.oEscanner.getImage(Sesion.getRuta() + "\\" + txtCURP.Text.Trim(), tipoDedo);

            
            if (oRetorno.calidad < 50)
            {
                MessageBox.Show("La calidad de la captura es inferior a la aceptada, repita la captura");
                return;
            }
            this.oEmpleado.Fingerprints.Add(new Huella(this.oEmpleado.Fingerprints.Count + 1, oRetorno.ruta, tipoDedo));
            fillGridView();
        }

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
        public frmRegistroEmpleado()
        {
            InitializeComponent();
            this.oEscanner = new ClsEscanner();
            this.oEscanner.iniciarScanner();
            this.cmbDedos.DataSource = Enum.GetValues(typeof(Huella.Dedo));
            this.cmbDedos.SelectedIndex = 0;
            this.oEmpleado =  new Empleado();
            this.lstEmpleados = new List<Empleado>();
        }

        private void btnAgregarHuella_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

        }
    }
}
