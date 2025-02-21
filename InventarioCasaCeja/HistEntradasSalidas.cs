using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Markup;

namespace InventarioCasaCeja
{
    public partial class HistEntradasSalidas : Form
    {
        int rowCount, maxPages, currentPage = 1, offset, rowsPerPage = 20;
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
            rowCount = localDM.getEntradasCountPorSucursal(idSucursal);
            calculateMaxPages(rowCount);
            tablaEntradas = localDM.getEntradasPorSucursal(idSucursal, offset, rowsPerPage);
            tablaEntradasySalidas.DataSource = tablaEntradas;
            tablaEntradasySalidas.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            // Cambiar el tamaño de la fuente de los encabezados de las columnas
            tablaEntradasySalidas.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 18, FontStyle.Bold);
        }
        private void CargarSalidasEnDataGrid()
        {
            rowCount = localDM.getSalidasCountPorSucursal(idSucursal);
            calculateMaxPages(rowCount);
            tablaSalidas = localDM.getSalidasPorSucursal(idSucursal, offset, rowsPerPage);
            tablaEntradasySalidas.DataSource = tablaSalidas;
            tablaEntradasySalidas.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            // Cambiar el tamaño de la fuente de los encabezados de las columnas
            tablaEntradasySalidas.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 18, FontStyle.Bold);
        }
        private void BcrearExcel_Click(object sender, EventArgs e)
        {
            int opc = BoxTipo.SelectedIndex;
            GenerarExcel(opc);
        }
        private void GenerarExcel(int opc)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;            
            DataTable EntradasTable = localDM.getEntradasPorSucursal(idSucursal);
            DataTable SalidasTable = localDM.getSalidasPorSucursal(idSucursal);
            DateTime localDate = DateTime.Now;
            string fecha = localDate.ToString("dd-MM-yyyy");

            if (EntradasTable.Rows.Count == 0 && opc == 0)
            {
                MessageBox.Show("No hay Entradas de Productos disponibles para la sucursal actual.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (SalidasTable.Rows.Count == 0 && opc == 1)
            {
                MessageBox.Show("No hay Salidas de Productos disponibles para la sucursal actual.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (EntradasTable.Rows.Count == 0 && SalidasTable.Rows.Count == 0 && opc == 2)
            {
                MessageBox.Show("No hay informacion disponibles para la sucursal actual.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Especificar la carpeta y el nombre del archivo
            string carpeta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CasaCejaDocs");
            string nombre = "";
            if (opc == 0)
            {
                nombre = "ListaEntradas ";
            }
            else if (opc == 1)
            {
                nombre = "ListaSalidas ";
            }
            string nombreArchivo = nombre + fecha + ".xlsx";
            string rutaArchivo = Path.Combine(carpeta, nombreArchivo);

            // Verificar si la carpeta existe, si no, crearla
            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            // Verificar si el archivo ya existe
            if (File.Exists(rutaArchivo))
            {
                DialogResult dialogResult = MessageBox.Show("El archivo ya existe. ¿Deseas sobrescribirlo?", "Archivo Existente", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            // Generar el archivo Excel
            try
            {
                using (ExcelPackage paquete = new ExcelPackage())
                {
                    if (opc == 0 || opc == 2)
                    {
                        // Crear la hoja para las entradas
                        ExcelWorksheet hojaEntradas = paquete.Workbook.Worksheets.Add("ListadoEntradas " + fecha);

                        // Agregar los encabezados de las columnas
                        for (int i = 0; i < EntradasTable.Columns.Count; i++)
                        {
                            hojaEntradas.Cells[1, i + 1].Value = EntradasTable.Columns[i].ColumnName;
                            hojaEntradas.Cells[1, i + 1].Style.Font.Bold = true;
                            hojaEntradas.Cells[1, i + 1].Style.Font.Size = 14;
                        }

                        // Agregar los datos de las filas
                        for (int fila = 0; fila < EntradasTable.Rows.Count; fila++)
                        {
                            for (int col = 0; col < EntradasTable.Columns.Count; col++)
                            {
                                hojaEntradas.Cells[fila + 2, col + 1].Value = EntradasTable.Rows[fila][col].ToString();
                            }
                        }
                    }

                    if (opc == 1 || opc == 2)
                    {
                        // Crear la hoja para las salidas
                        ExcelWorksheet hojaSalidas = paquete.Workbook.Worksheets.Add("ListadoSalidas " + fecha);

                        // Agregar los encabezados de las columnas
                        for (int i = 0; i < SalidasTable.Columns.Count; i++)
                        {
                            hojaSalidas.Cells[1, i + 1].Value = SalidasTable.Columns[i].ColumnName;
                            hojaSalidas.Cells[1, i + 1].Style.Font.Bold = true;
                            hojaSalidas.Cells[1, i + 1].Style.Font.Size = 14;
                        }

                        // Agregar los datos de las filas
                        for (int fila = 0; fila < SalidasTable.Rows.Count; fila++)
                        {
                            for (int col = 0; col < SalidasTable.Columns.Count; col++)
                            {
                                hojaSalidas.Cells[fila + 2, col + 1].Value = SalidasTable.Rows[fila][col].ToString();
                            }
                        }
                    }

                    // Guardar el archivo en la ruta especificada
                    FileInfo archivo = new FileInfo(rutaArchivo);
                    paquete.SaveAs(archivo);

                    // Mostrar mensaje de éxito si se ha creado correctamente
                    MessageBox.Show(nombre + fecha + ".xlsx" + " se generó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al generar el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    case Keys.F3:
                        BcrearExcel.PerformClick(); 
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
                int id = Convert.ToInt32(tablaEntradasySalidas.SelectedRows[0].Cells[0].Value);
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
            if (e.KeyCode == Keys.F3)
            {
                BcrearExcel.PerformClick();
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

        private void calculateMaxPages(int rowCount)
        {
            maxPages = (rowCount + rowsPerPage - 1) / rowsPerPage; // Divisón entera redondeando hacia arriba
            if (maxPages == 0)
                maxPages = 1;
            if (maxPages < currentPage)
            {
                currentPage = maxPages;
                offset = (currentPage - 1) * rowsPerPage;
            }
            pageLabel.Text = $"Página {currentPage}/{maxPages}";
        }

        private void prev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                offset -= rowsPerPage;
                currentPage--;
                if (BoxTipo.SelectedIndex == 0)
                {
                    CargarEntradasEnDataGrid();
                }
                else
                    CargarSalidasEnDataGrid();
            }
        }

        private void next_Click(object sender, EventArgs e)
        {
            if (currentPage < maxPages)
            {
                offset += rowsPerPage;
                currentPage++;
                if (BoxTipo.SelectedIndex == 0)
                {
                    CargarEntradasEnDataGrid();
                }
                else
                    CargarSalidasEnDataGrid();
            }
        }
    }
}
