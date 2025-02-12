using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventarioCasaCeja.Properties;
using System.Drawing.Text;
using Windows.UI.Xaml;

namespace InventarioCasaCeja
{
    public partial class ConfigWindow : Form
    {
        LocaldataManager localDM;
        Dictionary<string, int> mapasucursales;
        List<string> sucursales;
        List<string> listfont;
        List<int> listSizes;

        public ConfigWindow(LocaldataManager localdata)
        {
            InitializeComponent();
            this.localDM = localdata;
            mapasucursales = localDM.getIndicesSucursales();
            sucursales = new List<string> (mapasucursales.Keys);
            boxsucursal.DataSource = sucursales;
            listfont = new List<string>();
            listSizes = new List<int> {5,6,7,8,9,10,11,12,13,14,15};
        }

        private void selectprinter_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            DialogResult result = pd.ShowDialog();
            if(result == DialogResult.OK)
            {
                txtprintername.Text = pd.PrinterSettings.PrinterName;
            }
        }

        private void aceptar_Click(object sender, EventArgs e)
        {
            if (tamaños.SelectedIndex==-1)
            {
                MessageBox.Show("No se ha establecido el tamaño de texto", "Advertencia");
                return;
            }
            if (fuentes.SelectedIndex == -1)
            {
                MessageBox.Show("No se ha establecido la fuente de texto", "Advertencia");
                return;
            }
            if (txtprintername.Text == "")
            {
                MessageBox.Show("No se ha establecido la impresora", "Advertencia");
                return;
            }
            //this.boxsucursal.SelectedItem = 1;
            //Console.WriteLine(boxsucursal.SelectedItem.ToString());
            string selectsedsucursal = boxsucursal.SelectedItem.ToString();
            if (mapasucursales.ContainsKey(selectsedsucursal))
            {
                Settings.Default["sucursalid"] = mapasucursales[selectsedsucursal];
            }
            else
            {
                Settings.Default["sucursalid"] = 1;
            }
            Settings.Default["fontName"] = fuentes.SelectedItem.ToString();
            Settings.Default["fontSize"] = int.Parse(tamaños.SelectedItem.ToString());
            Settings.Default["printername"] = txtprintername.Text;
            Settings.Default["printertype"] = tipo.SelectedIndex;
            Settings.Default.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConfigWindow_Load(object sender, EventArgs e)
        {
            tamaños.DataSource = listSizes;
            string key = mapasucursales.FirstOrDefault(x => x.Value == int.Parse(Settings.Default["sucursalid"].ToString())).Key;
            boxsucursal.SelectedIndex = sucursales.IndexOf(key)==-1?0: sucursales.IndexOf(key);
            txtprintername.Text = Settings.Default["printername"].ToString();
            tamaños.SelectedIndex = listSizes.IndexOf(int.Parse(Settings.Default["fontSize"].ToString()));
            tipo.SelectedIndex = int.Parse(Settings.Default["printertype"].ToString());
            using (InstalledFontCollection col = new InstalledFontCollection())
            {
                foreach (FontFamily fa in col.Families)
                {
                    fuentes.Items.Add(fa.Name);
                    listfont.Add(fa.Name);
                }
            }
            fuentes.SelectedIndex = listfont.IndexOf(Settings.Default["fontName"].ToString());
        }
        private void integerInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
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
                    case Keys.F5:
                        aceptar.PerformClick();
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
