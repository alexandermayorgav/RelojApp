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
    public partial class frmVerRegistros : Form
    {
        private List<Registro> lstRegistro;
        private Registro oRegistro;
        public frmVerRegistros()
        {
            InitializeComponent();
            this.oRegistro = new Registro();
            this.dtFinal.Value = DateTime.Now;
            this.dtInicial.Value = DateTime.Now.AddDays(-1);
        }

        private bool validar()
        {
            if (dtFinal.Value<dtInicial.Value)
            {
                MessageBox.Show("La fecha final no puede ser anterior a la inicial", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }
        private void btnbuscar_Click(object sender, EventArgs e)
        {
            if (!validar())
                return;
            lstRegistro = this.oRegistro.getRegistrosByFecha(dtInicial.Value, dtFinal.Value);
            setDatos();
        }
        private void setDatos()
        {
            this.gridDatos.Rows.Clear();
            int row = 0;
            foreach (Registro item in this.lstRegistro)
            {
                this.gridDatos.Rows.Add();
                this.gridDatos.Rows[row].Cells[0].Value = item.fechaRegistro;
                this.gridDatos.Rows[row].Cells[1].Value = item.empleado.getNombreCompleto();
                row++;
            }
        }
    }
}
