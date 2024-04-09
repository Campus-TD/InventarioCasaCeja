using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventarioCasaCeja
{
    public partial class ModificarProveedor : Form
    {
        string id;
        WebDataManager webDM;
        public ModificarProveedor(WebDataManager webDataManager)
        {
            InitializeComponent();
            this.webDM = webDataManager;
            id = "";
        }
        public void setData(DataGridViewRow row)
        {
            id = row.Cells[0].Value.ToString();
            txtnombre.Text = row.Cells[1].Value.ToString();
            txtdireccion.Text = row.Cells[2].Value.ToString();
            txtcorreo.Text = row.Cells[3].Value.ToString();
            txttelefono.Text = row.Cells[4].Value.ToString();            
            txtdescripcion.Text = row.Cells[5].Value.ToString();
        }
        private void delete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Desea dar de baja este proveedor?", "Advertencia", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                disable();
            }
        }
        private async void disable()
        {
            if (await webDM.DisableProveedorAsync(id))
                this.Dispose();
            else MessageBox.Show("No se pudo conectar con el servidor, favor de intentar más tarde", "Advertencia");
        }
        private async void modify(Dictionary<string, string> data)
        {
            if (await webDM.ModifyProveedorAsync(id, data))
                this.Dispose();
            else MessageBox.Show("No se pudo conectar con el servidor, favor de intentar más tarde", "Advertencia");
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void accept_Click(object sender, EventArgs e)
        {
            if (txtnombre.Text.Equals("") || txtcorreo.Text.Equals("") || txttelefono.Text.Equals("") || txtdescripcion.Text.Equals(""))
            {
                MessageBox.Show("Favor de completar todos los campos", "Advertencia");
            }
            else
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data["_method"] = "PATCH";
                data["nombre"] = txtnombre.Text;
                data["direccion"] = txtdireccion.Text;
                data["correo"] = txtcorreo.Text;
                data["telefono"] = txttelefono.Text;
                data["descripcion"] = txtdescripcion.Text;
                modify(data);
            }
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        this.Close();
                        break;
                    case Keys.Enter:
                        if (cancel.Focused || accept.Focused || delete.Focused)
                            return base.ProcessDialogKey(keyData);
                        SendKeys.Send("{TAB}");
                        break;
                    case Keys.F1:
                        txtnombre.Focus();
                        break;
                    case Keys.F5:
                        accept.PerformClick();
                        break;
                    case Keys.F6:
                        delete.PerformClick();
                        break;
                    default:
                        return base.ProcessDialogKey(keyData);
                }
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
