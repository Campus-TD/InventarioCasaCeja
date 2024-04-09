using System;
using System.Net;
using System.Windows.Forms;

namespace InventarioCasaCeja
{
    public partial class CatMedCreator : Form
    {
        int type;
        WebDataManager webDM;
        public CatMedCreator(int Type, WebDataManager webDataManager)
        {
            InitializeComponent();
            this.type = Type;
            this.webDM = webDataManager;
            switch (type)
            {
                case 0:
                    this.Text = "Nueva categoría";
                    groupBox1.Text = "ALTA DE CATEGORÍA";
                    break;
                case 1:
                    this.Text = "Nueva unidad de medida";
                    groupBox1.Text = "ALTA DE UNIDAD DE MEDIDA";
                    break;
            }
        }

        private void upload_Click(object sender, EventArgs e)
        {
            if (txtnombre.Text.Equals(""))
            {
                MessageBox.Show("Debes ingresar un nombre", "Advertencia");
            }
            else
            {
                string nombre=txtnombre.Text;
                send(nombre);
                
            }
            
        }
        private async void send(string dato)
        {
            switch (type)
            {
                case 0:
                    if (await webDM.SendCategoriaAsync(dato))
                        txtnombre.Text = "";
                    else MessageBox.Show("No se pudo conectar con el servidor, favor de intentar más tarde", "Advertencia");
                    break;
                case 1:
                    if(await webDM.SendMedidaAsync(dato))
                        txtnombre.Text = "";
                    else MessageBox.Show("No se pudo conectar con el servidor, favor de intentar más tarde", "Advertencia");
                    break;
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
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
                        if (upload.Focused || cancel.Focused)
                            return base.ProcessDialogKey(keyData);
                        SendKeys.Send("{TAB}");
                        break;
                    case Keys.F1:
                        txtnombre.Focus();
                        break;
                    case Keys.F5:
                        upload.PerformClick();
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
            if (e.KeyData == Keys.Enter)
            {
                (sender as Button).PerformClick();
            }
        }
    }
}
