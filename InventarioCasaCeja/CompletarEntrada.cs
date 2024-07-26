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
    public partial class CompletarEntrada : Form
    {
        WebDataManager webDM;
        List<ProductoEntrada> productos;
        bool hasTemporal;
        Dictionary<string, int> proveedores;
        int sucursal = 0;
        public CompletarEntrada(WebDataManager webDataManager, List<ProductoEntrada>productos, bool hasT, int idsucursal)
        {
            InitializeComponent();
            this.webDM = webDataManager;
            this.productos = productos;
            this.hasTemporal = hasT;
            this.sucursal = idsucursal;
        }
        public void setSucursal(int s)
        {
            sucursal = s;
        }
        private void v_Click(object sender, EventArgs e)
        {
            if (sucursal == 0)
            {
                solicitarSucursal();
            }
            else
            {
                if (txtfolio.Text.Equals("") || txttotal.Text.Equals("") || txttotal.Text.Equals(".") || comboproveedores.SelectedIndex == -1)
                {
                    MessageBox.Show("Faltan datos de factura", "Advertencia");
                }
                else
                {
                    Dictionary<string, object> entrada = new Dictionary<string, object>();
                    entrada["folio_factura"] = txtfolio.Text;
                    entrada["total_factura"] = double.Parse(txttotal.Text).ToString("0.00");
                    entrada["fecha_factura"] = fechafactura.Value.Year + "-" + fechafactura.Value.Month + "-" + fechafactura.Value.Day;
                    entrada["usuario_id"] = webDM.activeUser.id.ToString();
                    entrada["sucursal_id"] = sucursal;
                    entrada["proveedor_id"] = proveedores[comboproveedores.SelectedItem.ToString()];
                    int id = webDM.localDM.registrarEntrada(entrada, productos);
                    enviarEntrada(id, entrada);
                }
            }
            
        }
        public async void enviarEntrada(int id, Dictionary<string, object> entrada)
        {
            if (await webDM.SendEntrada(id, hasTemporal, entrada, productos))
            {
                webDM.localDM.changeEstadoEntrada(id, 2, "Enviado");
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else MessageBox.Show("No se pudo conectar con el servidor, favor de intentar más tarde", "Advertencia");
        }
        private void numericInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        void solicitarSucursal()
        {
            ElegirSucursal es = new ElegirSucursal(webDM, setSucursal);
            DialogResult response = es.ShowDialog();
            if (response != DialogResult.Yes)
            {
                MessageBox.Show("Favor de seleccionar una sucursal antes de completar la entrada", "Advertencia");
            }
        }

        private void CompletarEntrada_Load(object sender, EventArgs e)
        {
            cargarProveedores();
        }
        private void cargarProveedores()
        {
            proveedores = webDM.localDM.getIndicesProveedores();
            comboproveedores.Items.Clear();
            comboproveedores.Items.AddRange(proveedores.Keys.ToArray());
        }

        private void crarprov_Click(object sender, EventArgs e)
        {
            CrearProveedor create = new CrearProveedor(webDM);
            DialogResult response = create.ShowDialog();
            if(response == DialogResult.Yes)
            {
                cargarProveedores();
                comboproveedores.SelectedIndex = comboproveedores.Items.Count - 1;
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
                        if (complete.Focused || exit.Focused)
                            return base.ProcessDialogKey(keyData);
                        SendKeys.Send("{TAB}");
                        break;
                    case Keys.F1:
                        txtfolio.Focus();
                        break;
                    case Keys.F5:
                        complete.PerformClick();
                        break;
                    default:
                        return base.ProcessDialogKey(keyData);
                }
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
