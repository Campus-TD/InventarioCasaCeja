using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace InventarioCasaCeja
{
    public partial class HistEntradasSalidas : Form
    {
        int type;
        List<string> tipo = new List<string>();
        string[] range = { "Entradas", "Salidas" };
        LocaldataManager localDM = new LocaldataManager();

        public HistEntradasSalidas()
        {
            InitializeComponent();
            this.type = 0;
            tipo.AddRange(range);
            BoxTipo.DataSource = tipo;
            BoxTipo.SelectedIndex = 0;

            // Cargar las entradas en el DataGridView
            CargarEntradasEnDataGrid();
        }

        private void CargarEntradasEnDataGrid()
        {
            // Obtener los datos de las entradas
            DataTable tablaEntradas = localDM.getEntradas();

            // Enlazar el DataTable con el DataGridView
            tablaEntradasySalidas.DataSource = tablaEntradas;

            // Opcional: Ajustar el tamaño de las columnas para que se adapten al contenido
            tablaEntradasySalidas.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None)
            {
                switch (keyData)
                {
                    case Keys.Enter:
                        BSelRegistro.PerformClick();
                        break;
                    case Keys.Escape:
                        this.Close();
                        break;
                    case Keys.F1:
                        BoxTipo.DroppedDown = true;
                        BoxTipo.Focus();
                        break;
                    case Keys.F5:
                        BelimHistorial.PerformClick();
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

        private void BelimHistorial_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("¿Desea eliminar el historial?", "Eliminar Registro", MessageBoxButtons.YesNo))
            {
                MessageBox.Show("Registro Eliminado");
            }
            return;
        }

        private void BSelRegistro_Click(object sender, EventArgs e)
        {
            VerEntradaSalida verEntradaSalida = new VerEntradaSalida(type);
            verEntradaSalida.ShowDialog();
        }

        private void tablaEntradasySalidas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BSelRegistro.PerformClick();
                return;
            }
            if (e.KeyCode == Keys.F5)
            {
                BelimHistorial.PerformClick();
                return;
            }
            if (e.KeyCode == Keys.F1)
            {
                BoxTipo.DroppedDown = true;
                BoxTipo.Focus();
                return;
            }
        }

        private void BoxTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BoxTipo.SelectedIndex == 0)
            {
                type = 0;
                // Aquí podrías cargar las entradas nuevamente si el tipo cambia
                CargarEntradasEnDataGrid();
            }
            else
            {
                type = 1;
                // Aquí podrías cargar otro tipo de datos si es necesario
            }
        }
    }
}
