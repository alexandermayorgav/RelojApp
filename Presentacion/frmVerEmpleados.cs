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
    public partial class frmVerEmpleados : Form
    {
        private List<Logica.Empleado> lstEmpleados;
        public frmVerEmpleados(List<Logica.Empleado> lstEmpleados)
        {
            this.lstEmpleados =  lstEmpleados;
            InitializeComponent();
            this.gridDatos.DataSource = this.lstEmpleados;
            this.gridDatos.Columns[4].Visible = false;
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string texto = txtBuscar.Text.ToLower();
            this.gridDatos.DataSource = this.lstEmpleados.Where(item => item.Nombres.ToLower().Contains(texto) | item.ApellidoPat.ToLower().Contains(texto) | item.ApellidoMat.ToLower().Contains(texto)).ToList();
        }

        private void gridDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Empleado oEmpleado = this.gridDatos.CurrentRow.DataBoundItem as Empleado;
            if (oEmpleado == null)
                return;
            frmRegistroEmpleado form = new frmRegistroEmpleado(oEmpleado);
            form.ShowDialog();
        }

       
    }
}
