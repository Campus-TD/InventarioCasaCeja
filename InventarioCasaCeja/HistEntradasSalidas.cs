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
        int idSucursal;
        DataTable tablaEntradas = new DataTable();
        DataTable tablaSalidas = new DataTable();

        public HistEntradasSalidas(int idSucursal)
        {
            InitializeComponent();
            this.type = 0;
            tipo.AddRange(range);
            BoxTipo.DataSource = tipo;
            BoxTipo.SelectedIndex = 0;
            this.idSucursal = idSucursal;
            CargarEntradasEnDataGrid();
        }

        private void CargarEntradasEnDataGrid()
        {
            tablaEntradas = localDM.getEntradasPorSucursal(idSucursal);

            tablaEntradasySalidas.DataSource = tablaEntradas;

            tablaEntradasySalidas.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
        private void CargarSalidasEnDataGrid()
        {
            tablaSalidas = localDM.getSalidasPorSucursal(idSucursal);

            tablaEntradasySalidas.DataSource = tablaSalidas;

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
            if (tablaEntradasySalidas.SelectedRows.Count > 0)
            {
                // Acceder al valor de la primera columna de la fila seleccionada
                int id = Convert.ToInt32(tablaEntradasySalidas.SelectedRows[0].Cells[0].Value);
                Console.WriteLine(id);
                // Crear la instancia de la ventana de detalle
                VerEntradaSalida verEntradaSalida = new VerEntradaSalida(type, id);
                verEntradaSalida.ShowDialog();
            }
            else
            {
                // Mostrar un mensaje si no hay ninguna fila seleccionada
                MessageBox.Show("No hay ninguna fila seleccionada.");
            }
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
                CargarEntradasEnDataGrid();
            }
            else
            {
                type = 1;
                CargarSalidasEnDataGrid();
                
            }
        }
    }
}
