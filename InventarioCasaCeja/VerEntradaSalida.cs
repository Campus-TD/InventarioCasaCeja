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
        int id;
        LocaldataManager localDM = new LocaldataManager();
        public VerEntradaSalida(int type, int id)
        {
            InitializeComponent();
            this.type = type;
            DataTable dataTable;
            switch (type)
            {
                case 0:
                    this.Text = "Ver Entrada";
                    groupBox1.Text = "VER ENTRADA";
                    dataTable = localDM.getProductoEntradaInfo(id);
                    tablaInfo.DataSource = dataTable;
                    break;
                case 1:
                    this.Text = "Ver Salida";
                    groupBox1.Text = "VER SALIDA";
                    dataTable = localDM.getProductosFromSalida(id);
                    tablaInfo.DataSource = dataTable;
                    break;
                default:
                    dataTable = null;
                    break;
            }
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("resultado");
            // Imprimir en consola el resultado de la consulta
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        Console.Write($"{column.ColumnName}: {row[column]} ");
                    }
                    Console.WriteLine();
                }
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
