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
    this.id = id;
    DataTable dataTable;
    
    Console.WriteLine($"★ VerEntradaSalida - Tipo: {type}, ID: {id}");
    
    switch (type)
    {
        case 0: // Entradas
            this.Text = "Ver Entrada";
            groupBox1.Text = "VER ENTRADA";
            Console.WriteLine($"★ Obteniendo detalles de entrada {id}...");
            dataTable = localDM.getProductoEntradaInfo(id);
            Console.WriteLine($"★ Registros encontrados en producto_entrada: {dataTable.Rows.Count}");
            tablaInfo.DataSource = dataTable;
            break;
            
        case 1: // Salidas
            this.Text = "Ver Salida";
            groupBox1.Text = "VER SALIDA";
            Console.WriteLine($"★ Obteniendo detalles de salida {id}...");
            dataTable = localDM.getProductosFromSalida(id);
            Console.WriteLine($"★ Productos encontrados en salida: {dataTable.Rows.Count}");
            tablaInfo.DataSource = dataTable;
            break;
            
        default:
            dataTable = null;
            break;
    }

    // Diagnóstico detallado
    if (dataTable != null && dataTable.Rows.Count > 0)
    {
        Console.WriteLine($"★ === DETALLES DE {(type == 0 ? "ENTRADA" : "SALIDA")} {id} ===");
        foreach (DataRow row in dataTable.Rows)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                Console.Write($"{column.ColumnName}: {row[column]} ");
            }
            Console.WriteLine();
        }
        Console.WriteLine($"★ === FIN DETALLES ===");
    }
    else
    {
        Console.WriteLine($"★ ❌ NO SE ENCONTRARON DETALLES para {(type == 0 ? "entrada" : "salida")} {id}");
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
