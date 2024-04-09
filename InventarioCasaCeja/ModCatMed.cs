using System;
using System.Net;
using System.Windows.Forms;
using System.Collections.Generic;

namespace InventarioCasaCeja
{
    public partial class ModCatMed : Form
    {
        int type;
        string id;
        string nametype;
        WebDataManager webDM;
        public ModCatMed(int Type, string Id, string Nombre, WebDataManager webDataManager)
        {
            InitializeComponent();
            this.type = Type;
            this.id = Id;
            txtnombre.Text = Nombre;
            this.webDM = webDataManager;
            switch (type)
            {
                case 0:
                    nametype = "categoría";
                    
                    break;
                case 1:
                    nametype = "unidad de medida";
                    break;
            }
            this.Text = "Modificar "+nametype;
            groupBox1.Text = ("Modificar " + nametype).ToUpper();
        }

        private void upload_Click(object sender, EventArgs e)
        {
            if (txtnombre.Text.Equals(""))
            {
                MessageBox.Show("Debes ingresar un nombre");
            }
            else
            {
                Dictionary<string, string> content = new Dictionary<string, string>();
                content["_method"] = "PATCH";
                content["nombre"] = txtnombre.Text;
                send(content);
            }
        }
        private async void send(Dictionary<string, string> dato)
        {
            switch (type)
            {
                case 0:
                    if (await webDM.ModifyCategoriaAsync(id, dato))
                        this.Dispose();
                    else MessageBox.Show("No se pudo conectar con el servidor, favor de intentar más tarde", "Advertencia");
                    break;
                case 1:
                    if (await webDM.ModifyMedidaAsync(id, dato))
                        this.Dispose();
                    else MessageBox.Show("No se pudo conectar con el servidor, favor de intentar más tarde", "Advertencia");
                    break;
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("¿Esta seguro que desea eliminar esta " + nametype + "?", "Advertencia", MessageBoxButtons.OKCancel);
            if (dr.Equals(DialogResult.OK))
            {
                delete();
            }
        }
        private async void delete()
        {
            switch (type)
            {
                case 0:
                    if (await webDM.DisableCategoriaAsync(id))
                        this.Dispose();
                    else MessageBox.Show("No se pudo conectar con el servidor, favor de intentar más tarde", "Advertencia");
                    break;
                case 1:
                    if (await webDM.DisableMedidaAsync(id))
                        this.Dispose();
                    else MessageBox.Show("No se pudo conectar con el servidor, favor de intentar más tarde", "Advertencia");
                    break;
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
                        if (upload.Focused || button1.Focused || cancel.Focused)
                            return base.ProcessDialogKey(keyData);
                        SendKeys.Send("{TAB}");
                        break;
                    case Keys.F1:
                        txtnombre.Focus();
                        break;
                    case Keys.F5:
                        upload.PerformClick();
                        break;
                    case Keys.F6:
                        button1.PerformClick();
                        break;
                    default:
                        return base.ProcessDialogKey(keyData);
                }
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
        private void buttonEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                (sender as Button).PerformClick();
            }
        }
    }
}
