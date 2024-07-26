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
    public partial class CrearProveedor : Form
    {
        WebDataManager webDM;
        public CrearProveedor(WebDataManager webDataManager)
        {
            InitializeComponent();
            this.webDM = webDataManager;
        }

        private void accept_Click(object sender, EventArgs e)
        {
             if(txtnombre.Text.Equals("") || txtcorreo.Text.Equals("") || txttelefono.Text.Equals(""))
            {
                MessageBox.Show("Favor de completar todos los campos obligatorios", "Advertencia");
            }
            else
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data["nombre"] = txtnombre.Text;
                data["direccion"] = txtdireccion.Text;
                data["correo"] = txtcorreo.Text;
                data["telefono"] = txttelefono.Text;
                data["descripcion"] = txtdescripcion.Text;
                send(data);
            }

        }
        private async void send(Dictionary<string, string> data)
        {
            if (await webDM.SendProveedor(data))
            {
                txtnombre.Text = "";
                txtcorreo.Text = "";
                txttelefono.Text = "";
                txtdescripcion.Text = "";
                txtdireccion.Text = "";
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else MessageBox.Show("No se pudo conectar con el servidor, favor de intentar más tarde", "Advertencia");
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
                        if (cancel.Focused || accept.Focused)
                            return base.ProcessDialogKey(keyData);
                        SendKeys.Send("{TAB}");
                        break;
                    case Keys.F1:
                        txtnombre.Focus();
                        break;
                    case Keys.F5:
                        accept.PerformClick();
                        break;
                    default:
                        return base.ProcessDialogKey(keyData);
                }
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void integerInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
