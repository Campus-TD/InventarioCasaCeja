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
    public partial class VerEntradaSalida : Form
    {
        int type;

        public VerEntradaSalida(int type)
        {
            InitializeComponent();
            this.type = type;
            switch (type)
            {
                case 0:
                    this.Text = "Ver Entrada";
                    groupBox1.Text = "VER ENTRADA";
                    break;
                case 1:
                    this.Text = "Ver Salida";
                    groupBox1.Text = "VER SALIDA";
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
                    default:
                        return base.ProcessDialogKey(keyData);
                }
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
