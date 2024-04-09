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
    public partial class ElegirSucursal : Form
    {
        WebDataManager webDM;
        Action<int> setSucursal;
        Dictionary<string, int> indiceSucursales;
        public ElegirSucursal(WebDataManager wdm, Action<int> setSucursal)
        {
            InitializeComponent();
            this.webDM = wdm;
            this.setSucursal = setSucursal;
        }

        private void ElegirSucursal_Load(object sender, EventArgs e)
        {
            indiceSucursales = webDM.localDM.getIndicesSucursales();
            combo.Items.AddRange(indiceSucursales.Keys.ToArray());
            combo.SelectedIndex = 0;
        }

        private void cancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        cancelar.PerformClick();
                        break;
                    case Keys.Enter:
                        aceptar.PerformClick();
                        break;
                    default:
                        return base.ProcessDialogKey(keyData);
                }
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void aceptar_Click(object sender, EventArgs e)
        {
            if (combo.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de seleccionar una sucursal", "Advertencia");
            }
            else
            {
                setSucursal(indiceSucursales[combo.SelectedItem.ToString()]);
                this.DialogResult = DialogResult.Yes;
            }
        }
    }
}
