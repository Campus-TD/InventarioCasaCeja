using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
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
        // ⭐ NUEVAS VARIABLES PARA FILTRO DE FECHAS
        private DateTime? fechaInicio = null;
        private DateTime? fechaFin = null;
        private bool usarFiltroFecha = false;

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

        // ⭐ MÉTODO ACTUALIZADO PARA GENERAR EXCEL CON FILTRO DE FECHAS
        private void BcrearExcel_Click(object sender, EventArgs e)
        {
            // Mostrar formulario de selección de fechas
            using (SelectorFechasHist formFechas = new SelectorFechasHist())
            {
                if (formFechas.ShowDialog() == DialogResult.OK && formFechas.FechasSeleccionadas)
                {
                    int opc = BoxTipo.SelectedIndex;
                    DateTime fechaInicio = formFechas.FechaInicio;
                    DateTime fechaFin = formFechas.FechaFin;

                    GenerarExcelConFecha(opc, fechaInicio, fechaFin);
                }
            }
        }

        // ⭐ NUEVO MÉTODO PARA GENERAR EXCEL CON RANGO DE FECHAS
        public void GenerarExcelConFecha(int opc, DateTime fechaInicio, DateTime fechaFin)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // Obtener datos filtrados por fecha
            DataTable EntradasTable = localDM.getEntradasPorSucursalPorFecha(idSucursal, fechaInicio, fechaFin);
            DataTable SalidasTable = localDM.getSalidasPorSucursalPorFecha(idSucursal, fechaInicio, fechaFin);

            string fechaInicioStr = fechaInicio.ToString("dd-MM-yyyy");
            string fechaFinStr = fechaFin.ToString("dd-MM-yyyy");
            string rangoFechas = $"{fechaInicioStr}_al_{fechaFinStr}";

            // Validaciones para el documento
            if (EntradasTable.Rows.Count == 0 && opc == 0)
            {
                MessageBox.Show($"No hay Entradas de Productos disponibles para la sucursal actual en el período del {fechaInicioStr} al {fechaFinStr}.",
                                  "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (SalidasTable.Rows.Count == 0 && opc == 1)
            {
                MessageBox.Show($"No hay Salidas de Productos disponibles para la sucursal actual en el período del {fechaInicioStr} al {fechaFinStr}.",
                                  "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (EntradasTable.Rows.Count == 0 && SalidasTable.Rows.Count == 0 && opc == 2)
            {
                MessageBox.Show($"No hay información disponible para la sucursal actual en el período del {fechaInicioStr} al {fechaFinStr}.",
                                  "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Configuración de archivo
            string carpetaPrincipal = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CasaCejaDocs");
            string subcarpeta = Path.Combine(carpetaPrincipal, "Inventario");
            string nombre;

            switch (opc)
            {
                case 0:
                    nombre = "ListaEntradas_";
                    break;
                case 1:
                    nombre = "ListaSalidas_";
                    break;
                default:
                    nombre = "MovimientosCompletos_";
                    break;
            }

            string nombreArchivo = nombre + rangoFechas + ".xlsx";
            string rutaArchivo = Path.Combine(subcarpeta, nombreArchivo);

            // Se crean las carpetas si no existen
            if (!Directory.Exists(carpetaPrincipal))
                Directory.CreateDirectory(carpetaPrincipal);

            if (!Directory.Exists(subcarpeta))
                Directory.CreateDirectory(subcarpeta);

            if (File.Exists(rutaArchivo))
            {
                var respuesta = MessageBox.Show("El archivo ya existe. ¿Deseas sobrescribirlo?",
                                          "Archivo Existente", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.No)
                    return;
            }

            try
            {
                using (ExcelPackage paquete = new ExcelPackage())
                {
                    // Hoja general de Entradas y hoja de detalle de Entradas
                    if (opc == 0 || opc == 2)
                    {
                        ExcelWorksheet hojaEntradas = paquete.Workbook.Worksheets.Add("Entradas");
                        CargarDatosEnHojaConFecha(hojaEntradas, EntradasTable, fechaInicio, fechaFin, "ENTRADAS");

                        ExcelWorksheet hojaDetalleEntradas = paquete.Workbook.Worksheets.Add("DetalleEntradas");
                        ConfigurarHojaDetalles(hojaDetalleEntradas);
                        LlenarDetallesEntradasConFecha(hojaDetalleEntradas, EntradasTable);
                    }

                    // Hojas de Salidas y Detalles
                    if (opc == 1 || opc == 2)
                    {
                        ExcelWorksheet hojaSalidas = paquete.Workbook.Worksheets.Add("Salidas");
                        CargarDatosEnHojaConFecha(hojaSalidas, SalidasTable, fechaInicio, fechaFin, "SALIDAS");

                        ExcelWorksheet hojaDetalles = paquete.Workbook.Worksheets.Add("DetalleSalidas");
                        ConfigurarHojaDetalles(hojaDetalles);
                        LlenarDetallesSalidasConFecha(hojaDetalles, SalidasTable);
                    }

                    paquete.SaveAs(new FileInfo(rutaArchivo));

                    string mensaje = $"{nombreArchivo} generado correctamente\n\n" +
                                   $"Período: {fechaInicioStr} al {fechaFinStr}\n" +
                                   $"Ubicación: {subcarpeta}";

                    MessageBox.Show(mensaje, "✅ Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el archivo: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ⭐ MÉTODO ACTUALIZADO PARA CARGAR DATOS CON INFORMACIÓN DE FECHA
        private void CargarDatosEnHojaConFecha(ExcelWorksheet hoja, DataTable tabla, DateTime fechaInicio, DateTime fechaFin, string tipoReporte)
        {
            // Agregar título con rango de fechas
            hoja.Cells[1, 1].Value = $"REPORTE DE {tipoReporte} - CASA CEJA";
            hoja.Cells[1, 1, 1, tabla.Columns.Count].Merge = true;
            hoja.Cells[1, 1].Style.Font.Bold = true;
            hoja.Cells[1, 1].Style.Font.Size = 16;
            hoja.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

            // Agregar información del período
            string fechaInicioStr = fechaInicio.ToString("dd/MM/yyyy");
            string fechaFinStr = fechaFin.ToString("dd/MM/yyyy");
            hoja.Cells[2, 1].Value = $"Período: {fechaInicioStr} al {fechaFinStr}";
            hoja.Cells[2, 1, 2, tabla.Columns.Count].Merge = true;
            hoja.Cells[2, 1].Style.Font.Bold = true;
            hoja.Cells[2, 1].Style.Font.Size = 12;
            hoja.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            // Agregar fecha de generación
            hoja.Cells[3, 1].Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
            hoja.Cells[3, 1, 3, tabla.Columns.Count].Merge = true;
            hoja.Cells[3, 1].Style.Font.Size = 10;
            hoja.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[3, 1].Style.Font.Color.SetColor(Color.Gray);

            // Encabezados de tabla (fila 5)
            for (int i = 0; i < tabla.Columns.Count; i++)
            {
                var celda = hoja.Cells[5, i + 1];
                celda.Value = tabla.Columns[i].ColumnName;
                celda.Style.Font.Bold = true;
                celda.Style.Font.Size = 12;
                celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                celda.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                celda.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Datos de la tabla (desde fila 6)
            for (int fila = 0; fila < tabla.Rows.Count; fila++)
            {
                for (int col = 0; col < tabla.Columns.Count; col++)
                {
                    var celda = hoja.Cells[fila + 6, col + 1];
                    celda.Value = tabla.Rows[fila][col];
                    celda.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    // Formatear fechas
                    if (tabla.Columns[col].ColumnName.ToUpper().Contains("FECHA"))
                    {
                        celda.Style.Numberformat.Format = "dd/mm/yyyy";
                    }
                    // Formatear montos
                    else if (tabla.Columns[col].ColumnName.ToUpper().Contains("TOTAL"))
                    {
                        celda.Style.Numberformat.Format = "$#,##0.00";
                    }
                }
            }

            // Agregar totales si es necesario
            if (tabla.Columns.Contains("TOTAL"))
            {
                int totalColumn = tabla.Columns["TOTAL"].Ordinal + 1;
                int lastRow = tabla.Rows.Count + 6;

                hoja.Cells[lastRow, totalColumn - 1].Value = "TOTAL:";
                hoja.Cells[lastRow, totalColumn - 1].Style.Font.Bold = true;
                hoja.Cells[lastRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                hoja.Cells[lastRow, totalColumn].Formula = $"=SUM({GetExcelColumnName(totalColumn)}6:{GetExcelColumnName(totalColumn)}{lastRow - 1})";
                hoja.Cells[lastRow, totalColumn].Style.Font.Bold = true;
                hoja.Cells[lastRow, totalColumn].Style.Numberformat.Format = "$#,##0.00";
                hoja.Cells[lastRow, totalColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells[lastRow, totalColumn].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
            }

            hoja.Cells[hoja.Dimension.Address].AutoFitColumns();
            hoja.View.FreezePanes(6, 1); // Congelar hasta la fila de encabezados
        }

        // ⭐ MÉTODO AUXILIAR PARA OBTENER NOMBRE DE COLUMNA EN EXCEL
        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        // ⭐ MÉTODO PARA LLENAR DETALLES DE ENTRADAS CON FECHA
        private void LlenarDetallesEntradasConFecha(ExcelWorksheet hoja, DataTable entradas)
        {
            int filaActual = 2;
            int ultimoIdEntrada = -1;

            string[] encabezados = { "ENTRADA ID", "ID", "CÓDIGO", "NOMBRE", "CANTIDAD", "COSTO" };
            for (int i = 0; i < encabezados.Length; i++)
            {
                var celda = hoja.Cells[1, i + 1];
                celda.Value = encabezados[i];
                celda.Style.Font.Bold = true;
                celda.Style.Font.Size = 11;
                celda.Style.Font.Color.SetColor(Color.Black);
                celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                celda.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                celda.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            foreach (DataRow entrada in entradas.Rows)
            {
                int entradaId = Convert.ToInt32(entrada["ID"]);
                DataTable productos = localDM.getProductoEntradaInfo(entradaId);

                if (productos.Rows.Count == 0) continue;

                if (ultimoIdEntrada != -1 && ultimoIdEntrada != entradaId)
                {
                    filaActual++;
                }

                foreach (DataRow producto in productos.Rows)
                {
                    var colorFondo = Color.White;

                    hoja.Cells[filaActual, 1].Value = entradaId;
                    hoja.Cells[filaActual, 2].Value = producto["ID"];
                    hoja.Cells[filaActual, 3].Value = producto["CODIGO"];
                    hoja.Cells[filaActual, 4].Value = producto["NOMBRE"];
                    hoja.Cells[filaActual, 5].Value = producto["CANTIDAD"];
                    hoja.Cells[filaActual, 6].Value = producto["COSTO"];

                    for (int col = 1; col <= 6; col++)
                    {
                        var celda = hoja.Cells[filaActual, col];
                        celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        celda.Style.Fill.BackgroundColor.SetColor(colorFondo);
                        celda.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        celda.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        celda.Style.Font.Color.SetColor(Color.Black);
                    }
                    filaActual++;
                }
                ultimoIdEntrada = entradaId;
            }

            hoja.Cells["F2:F" + (filaActual - 1)].Style.Numberformat.Format = "$#,##0.00";
            hoja.Cells["E2:E" + (filaActual - 1)].Style.Numberformat.Format = "0";

            hoja.Cells[hoja.Dimension.Address].AutoFitColumns();
            hoja.View.FreezePanes(2, 1);

            if (filaActual > 2)
            {
                var rangoTabla = hoja.Cells["A1:F" + (filaActual - 1)];
                var tabla = hoja.Tables.Add(rangoTabla, "DetalleEntradas");
                tabla.TableStyle = TableStyles.Light11;
                tabla.ShowHeader = true;
                tabla.ShowTotal = false;
                tabla.ShowFirstColumn = false;
            }
        }

        // ⭐ MÉTODO PARA LLENAR DETALLES DE SALIDAS CON FECHA
        private void LlenarDetallesSalidasConFecha(ExcelWorksheet hoja, DataTable salidas)
        {
            int filaActual = 2;
            int ultimoIdSalida = -1;

            string[] encabezados = { "ID SALIDA", "ID PRODUCTO", "NOMBRE", "CATEGORÍA", "PRECIO UNITARIO", "CANTIDAD", "TOTAL" };
            for (int i = 0; i < encabezados.Length; i++)
            {
                var celda = hoja.Cells[1, i + 1];
                celda.Value = encabezados[i];
                celda.Style.Font.Bold = true;
                celda.Style.Font.Size = 11;
                celda.Style.Font.Color.SetColor(Color.Black);
                celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                celda.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                celda.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            foreach (DataRow salida in salidas.Rows)
            {
                int idSalida = Convert.ToInt32(salida["ID"]);
                DataTable productos = localDM.getProductosFromSalida(idSalida);

                if (productos.Rows.Count == 0) continue;

                if (ultimoIdSalida != -1 && ultimoIdSalida != idSalida)
                {
                    filaActual++;
                }

                foreach (DataRow producto in productos.Rows)
                {
                    var colorFondo = Color.White;

                    hoja.Cells[filaActual, 1].Value = idSalida;
                    hoja.Cells[filaActual, 2].Value = producto["ID PRODUCTO"];
                    hoja.Cells[filaActual, 3].Value = producto["NOMBRE"];
                    hoja.Cells[filaActual, 4].Value = producto["CATEGORÍA"];
                    hoja.Cells[filaActual, 5].Value = producto["PRECIO"];
                    hoja.Cells[filaActual, 6].Value = producto["CANTIDAD"];
                    hoja.Cells[filaActual, 7].Value = Convert.ToDecimal(producto["PRECIO"]) * Convert.ToInt32(producto["CANTIDAD"]);

                    for (int col = 1; col <= 7; col++)
                    {
                        var celda = hoja.Cells[filaActual, col];
                        celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        celda.Style.Fill.BackgroundColor.SetColor(colorFondo);
                        celda.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        celda.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        celda.Style.Font.Color.SetColor(Color.Black);
                    }

                    filaActual++;
                }

                ultimoIdSalida = idSalida;
            }

            hoja.Cells["E2:E" + (filaActual - 1)].Style.Numberformat.Format = "$#,##0.00";
            hoja.Cells["G2:G" + (filaActual - 1)].Style.Numberformat.Format = "$#,##0.00";
            hoja.Cells["F2:F" + (filaActual - 1)].Style.Numberformat.Format = "0";

            if (filaActual > 2)
            {
                var rangoTabla = hoja.Cells["A1:G" + (filaActual - 1)];
                var tabla = hoja.Tables.Add(rangoTabla, "DetalleSalidas");
                tabla.TableStyle = TableStyles.Light11;
                tabla.ShowHeader = true;
                tabla.ShowTotal = false;
                tabla.ShowFirstColumn = false;
            }

            hoja.Cells[hoja.Dimension.Address].AutoFitColumns();
            hoja.View.FreezePanes(2, 1);
        }

        // ⭐ MÉTODO OPCIONAL: AÑADIR FILTRO DE FECHA TAMBIÉN A LA VISTA (no solo Excel)
        private void ActivarFiltroFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            this.fechaInicio = fechaInicio;
            this.fechaFin = fechaFin;
            this.usarFiltroFecha = true;

            // Resetear paginación
            currentPage = 1;
            offset = 0;

            // Recargar datos
            if (BoxTipo.SelectedIndex == 0)
            {
                CargarEntradasEnDataGrid();
            }
            else
            {
                CargarSalidasEnDataGrid();
            }
        }

        // ⭐ MÉTODOS ACTUALIZADOS PARA SOPORTAR FILTRO DE FECHA EN LA VISTA
        private void CargarEntradasEnDataGrid()
        {
            if (usarFiltroFecha && fechaInicio.HasValue && fechaFin.HasValue)
            {
                rowCount = localDM.getEntradasCountPorSucursalPorFecha(idSucursal, fechaInicio.Value, fechaFin.Value);
                calculateMaxPages(rowCount);
                tablaEntradas = localDM.getEntradasPorSucursalPorFecha(idSucursal, fechaInicio.Value, fechaFin.Value, offset, rowsPerPage);
            }
            else
            {
                rowCount = localDM.getEntradasCountPorSucursal(idSucursal);
                calculateMaxPages(rowCount);
                tablaEntradas = localDM.getEntradasPorSucursal(idSucursal, offset, rowsPerPage);
            }

            tablaEntradas.DefaultView.Sort = "id DESC";
            tablaEntradasySalidas.DataSource = tablaEntradas;
            tablaEntradasySalidas.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            tablaEntradasySalidas.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 18, FontStyle.Bold);
        }

        private void CargarSalidasEnDataGrid()
        {
            if (usarFiltroFecha && fechaInicio.HasValue && fechaFin.HasValue)
            {
                rowCount = localDM.getSalidasCountPorSucursalPorFecha(idSucursal, fechaInicio.Value, fechaFin.Value);
                calculateMaxPages(rowCount);
                tablaSalidas = localDM.getSalidasPorSucursalPorFecha(idSucursal, fechaInicio.Value, fechaFin.Value, offset, rowsPerPage);
            }
            else
            {
                rowCount = localDM.getSalidasCountPorSucursal(idSucursal);
                calculateMaxPages(rowCount);
                tablaSalidas = localDM.getSalidasPorSucursal(idSucursal, offset, rowsPerPage);
            }

            tablaSalidas.DefaultView.Sort = "id DESC";
            tablaEntradasySalidas.DataSource = tablaSalidas;
            tablaEntradasySalidas.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            tablaEntradasySalidas.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 18, FontStyle.Bold);
        }

        // Metodo incial creado por mi :ccc
        // Método para generar un archivo Excel con la información de las entradas y salidas
        public void GenerarExcel(int opc)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            DataTable EntradasTable = localDM.getEntradasPorSucursal(idSucursal);
            DataTable SalidasTable = localDM.getSalidasPorSucursal(idSucursal);
            DateTime localDate = DateTime.Now;
            string fecha = localDate.ToString("dd-MM-yyyy");

            // Validaciones para el documento.
            if (EntradasTable.Rows.Count == 0 && opc == 0)
            {
                MessageBox.Show("No hay Entradas de Productos disponibles para la sucursal actual.",
                                  "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (SalidasTable.Rows.Count == 0 && opc == 1)
            {
                MessageBox.Show("No hay Salidas de Productos disponibles para la sucursal actual.",
                                  "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (EntradasTable.Rows.Count == 0 && SalidasTable.Rows.Count == 0 && opc == 2)
            {
                MessageBox.Show("No hay información disponible para la sucursal actual.",
                                  "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Configuración de archivo
            string carpetaPrincipal = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CasaCejaDocs");
            string subcarpeta = Path.Combine(carpetaPrincipal, "Inventario");
            string nombre;

            switch (opc)
            {
                case 0:
                    nombre = "ListaEntradas ";
                    break;
                case 1:
                    nombre = "ListaSalidas ";
                    break;
                default:
                    nombre = "MovimientosCompletos ";
                    break;
            }

            string nombreArchivo = nombre + fecha + ".xlsx";
            string rutaArchivo = Path.Combine(subcarpeta, nombreArchivo);

            // Se crean las carpetas si no existen
            if (!Directory.Exists(carpetaPrincipal))
                Directory.CreateDirectory(carpetaPrincipal);

            if (!Directory.Exists(subcarpeta))
                Directory.CreateDirectory(subcarpeta);

            if (File.Exists(rutaArchivo))
            {
                var respuesta = MessageBox.Show("El archivo ya existe. ¿Deseas sobrescribirlo?",
                                          "Archivo Existente", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.No)
                    return;
            }

            try
            {
                using (ExcelPackage paquete = new ExcelPackage())
                {
                    // Hoja general de Entradas y hoja de detalle de Entradas
                    if (opc == 0 || opc == 2)
                    {
                        ExcelWorksheet hojaEntradas = paquete.Workbook.Worksheets.Add("Entradas");
                        CargarDatosEnHoja(hojaEntradas, EntradasTable);

                        ExcelWorksheet hojaDetalleEntradas = paquete.Workbook.Worksheets.Add("DetalleEntradas");
                        ConfigurarHojaDetalles(hojaDetalleEntradas);
                        LlenarDetallesEntradas(hojaDetalleEntradas, EntradasTable);
                    }

                    // Hojas de Salidas y Detalles
                    if (opc == 1 || opc == 2)
                    {
                        ExcelWorksheet hojaSalidas = paquete.Workbook.Worksheets.Add("Salidas");
                        CargarDatosEnHoja(hojaSalidas, SalidasTable);

                        ExcelWorksheet hojaDetalles = paquete.Workbook.Worksheets.Add("DetalleSalidas");
                        ConfigurarHojaDetalles(hojaDetalles);
                        LlenarDetallesSalidas(hojaDetalles, SalidasTable);
                    }

                    paquete.SaveAs(new FileInfo(rutaArchivo));
                    MessageBox.Show($"{nombreArchivo} generado correctamente",
                                  "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el archivo: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
            // Método para cargar datos generales en una hoja
            private void CargarDatosEnHoja(ExcelWorksheet hoja, DataTable tabla)
            {
            // Encabezados
            for (int i = 0; i < tabla.Columns.Count; i++)
            {
                hoja.Cells[1, i + 1].Value = tabla.Columns[i].ColumnName;
                hoja.Cells[1, i + 1].Style.Font.Bold = true;
                hoja.Cells[1, i + 1].Style.Font.Size = 12;
            }

            // Datos
            for (int fila = 0; fila < tabla.Rows.Count; fila++)
            {
                for (int col = 0; col < tabla.Columns.Count; col++)
                {
                    hoja.Cells[fila + 2, col + 1].Value = tabla.Rows[fila][col];
                }
            }

            hoja.Cells[hoja.Dimension.Address].AutoFitColumns();
            hoja.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        }       
      
        // Método para llenar la hoja de detalles de Salidas
        private void LlenarDetallesSalidas(ExcelWorksheet hoja, DataTable salidas)
        {
            int filaActual = 2;
            int ultimoIdSalida = -1;

            // Encabezados para salidas
            string[] encabezados = { "ID SALIDA", "ID PRODUCTO", "NOMBRE", "CATEGORÍA", "PRECIO UNITARIO", "CANTIDAD", "TOTAL" };
            for (int i = 0; i < encabezados.Length; i++)
            {
                var celda = hoja.Cells[1, i + 1];
                celda.Value = encabezados[i];
                celda.Style.Font.Bold = true;
                celda.Style.Font.Size = 11;
                celda.Style.Font.Color.SetColor(Color.Black);
                celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                celda.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                celda.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            foreach (DataRow salida in salidas.Rows)
            {
                int idSalida = Convert.ToInt32(salida["ID"]);
                DataTable productos = localDM.getProductosFromSalida(idSalida);

                if (productos.Rows.Count == 0) continue;

                if (ultimoIdSalida != -1 && ultimoIdSalida != idSalida)
                {
                    filaActual++;
                }

                foreach (DataRow producto in productos.Rows)
                {
                    var colorFondo = Color.White;

                    hoja.Cells[filaActual, 1].Value = idSalida;
                    hoja.Cells[filaActual, 2].Value = producto["ID PRODUCTO"];
                    hoja.Cells[filaActual, 3].Value = producto["NOMBRE"];
                    hoja.Cells[filaActual, 4].Value = producto["CATEGORÍA"];
                    hoja.Cells[filaActual, 5].Value = producto["PRECIO"];
                    hoja.Cells[filaActual, 6].Value = producto["CANTIDAD"];
                    hoja.Cells[filaActual, 7].Value = Convert.ToDecimal(producto["PRECIO"]) * Convert.ToInt32(producto["CANTIDAD"]);

                    for (int col = 1; col <= 7; col++)
                    {
                        var celda = hoja.Cells[filaActual, col];
                        celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        celda.Style.Fill.BackgroundColor.SetColor(colorFondo);
                        celda.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        celda.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        celda.Style.Font.Color.SetColor(Color.Black);
                    }

                    filaActual++;
                }

                ultimoIdSalida = idSalida;
            }

            hoja.Cells["E2:E" + (filaActual - 1)].Style.Numberformat.Format = "$#,##0.00";
            hoja.Cells["G2:G" + (filaActual - 1)].Style.Numberformat.Format = "$#,##0.00";
            hoja.Cells["F2:F" + (filaActual - 1)].Style.Numberformat.Format = "0";

            if (filaActual > 2)
            {
                var rangoTabla = hoja.Cells["A1:G" + (filaActual - 1)];
                var tabla = hoja.Tables.Add(rangoTabla, "DetalleSalidas");
                tabla.TableStyle = TableStyles.Light11;
                tabla.ShowHeader = true;
                tabla.ShowTotal = false;
                tabla.ShowFirstColumn = false;
            }

            hoja.Cells[hoja.Dimension.Address].AutoFitColumns();
            hoja.View.FreezePanes(2, 1);
        }
        
        // ★ MÉTODO LlenarDetallesEntradas CORREGIDO
        private void LlenarDetallesEntradas(ExcelWorksheet hoja, DataTable entradas)
        {
            int filaActual = 2;
            int ultimoIdEntrada = -1;

            // ★ CAMBIO: Ajustar encabezados a las columnas disponibles
            string[] encabezados = { "ENTRADA ID", "ID", "CÓDIGO", "NOMBRE", "CANTIDAD", "COSTO" };
            for (int i = 0; i < encabezados.Length; i++)
            {
                var celda = hoja.Cells[1, i + 1];
                celda.Value = encabezados[i];
                celda.Style.Font.Bold = true;
                celda.Style.Font.Size = 11;
                celda.Style.Font.Color.SetColor(Color.Black);
                celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                celda.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                celda.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            foreach (DataRow entrada in entradas.Rows)
            {
                int entradaId = Convert.ToInt32(entrada["ID"]);
                DataTable productos = localDM.getProductoEntradaInfo(entradaId);

                if (productos.Rows.Count == 0) continue;

                if (ultimoIdEntrada != -1 && ultimoIdEntrada != entradaId)
                {
                    filaActual++;
                }

                foreach (DataRow producto in productos.Rows)
                {
                    var colorFondo = Color.White;

                    // ★ CAMBIO: Usar las columnas que SÍ existen
                    hoja.Cells[filaActual, 1].Value = entradaId;           // ENTRADA ID (manual)
                    hoja.Cells[filaActual, 2].Value = producto["ID"];      // ID del producto_entrada
                    hoja.Cells[filaActual, 3].Value = producto["CODIGO"];  // CODIGO
                    hoja.Cells[filaActual, 4].Value = producto["NOMBRE"];  // NOMBRE
                    hoja.Cells[filaActual, 5].Value = producto["CANTIDAD"]; // CANTIDAD
                    hoja.Cells[filaActual, 6].Value = producto["COSTO"];   // COSTO

                    // ★ CAMBIO: Ajustar el loop para 6 columnas en lugar de 7
                    for (int col = 1; col <= 6; col++)
                    {
                        var celda = hoja.Cells[filaActual, col];
                        celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        celda.Style.Fill.BackgroundColor.SetColor(colorFondo);
                        celda.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        celda.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        celda.Style.Font.Color.SetColor(Color.Black);
                    }
                    filaActual++;
                }
                ultimoIdEntrada = entradaId;
            }

            // ★ CAMBIO: Aplicar formato de moneda a la columna COSTO (columna 6)
            hoja.Cells["F2:F" + (filaActual - 1)].Style.Numberformat.Format = "$#,##0.00";

            // ★ CAMBIO: Aplicar formato de número a la columna CANTIDAD (columna 5)
            hoja.Cells["E2:E" + (filaActual - 1)].Style.Numberformat.Format = "0";

            hoja.Cells[hoja.Dimension.Address].AutoFitColumns();
            hoja.View.FreezePanes(2, 1);

            if (filaActual > 2)
            {
                // ★ CAMBIO: Ajustar el rango de la tabla para 6 columnas (A1:F)
                var rangoTabla = hoja.Cells["A1:F" + (filaActual - 1)];
                var tabla = hoja.Tables.Add(rangoTabla, "DetalleEntradas");
                tabla.TableStyle = TableStyles.Light11;
                tabla.ShowHeader = true;
                tabla.ShowTotal = false;
                tabla.ShowFirstColumn = false;
            }
        }

        // ★ OPCIONAL: También corregir el método ConfigurarHojaDetalles para que sea más específico
        private void ConfigurarHojaDetalles(ExcelWorksheet hoja)
        {
            // Determinar qué tipo de hoja es por el nombre
            if (hoja.Name.Contains("Entrada"))
            {
                // Para entradas: ENTRADA ID, ID, CÓDIGO, NOMBRE, CANTIDAD, COSTO
                string[] encabezados = { "ENTRADA ID", "ID", "CÓDIGO", "NOMBRE", "CANTIDAD", "COSTO" };
                for (int i = 0; i < encabezados.Length; i++)
                {
                    var celda = hoja.Cells[1, i + 1];
                    celda.Value = encabezados[i];
                    celda.Style.Font.Bold = true;
                    celda.Style.Font.Size = 12;
                    celda.Style.Font.Color.SetColor(Color.Black);
                    celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    celda.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    celda.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
            }
            else
            {
                // Para salidas: mantener el formato original
                string[] encabezados = { "ID", "ID PRODUCTO", "NOMBRE", "CATEGORÍA", "PRECIO UNITARIO", "CANTIDAD", "TOTAL" };
                for (int i = 0; i < encabezados.Length; i++)
                {
                    var celda = hoja.Cells[1, i + 1];
                    celda.Value = encabezados[i];
                    celda.Style.Font.Bold = true;
                    celda.Style.Font.Size = 12;
                    celda.Style.Font.Color.SetColor(Color.Black);
                    celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    celda.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    celda.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
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

        private void TablaEntradasySalidas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Solo aplicar formato a la columna COMENTARIOS cuando estamos en Entradas
            if (type == 0 && tablaEntradasySalidas.Columns[e.ColumnIndex].Name == "COMENTARIOS")
            {
                if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    // Mostrar "📝 VER" si hay comentario
                    e.Value = "📝 VER";
                    e.CellStyle.ForeColor = Color.Blue;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    e.FormattingApplied = true;
                }
                else
                {
                    // Mostrar "-" si no hay comentario
                    e.Value = "-";
                    e.CellStyle.ForeColor = Color.Gray;
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    e.FormattingApplied = true;
                }
            }
        }

        // ⭐ NUEVO MÉTODO: Manejar clic en la celda de comentarios
        private void TablaEntradasySalidas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que no sea el header y que estemos en Entradas
            if (e.RowIndex < 0 || type != 0) return;

            // Verificar si se hizo clic en la columna COMENTARIOS
            if (tablaEntradasySalidas.Columns[e.ColumnIndex].Name == "COMENTARIOS")
            {
                // Obtener el comentario completo de la base de datos
                int entradaId = Convert.ToInt32(tablaEntradasySalidas.Rows[e.RowIndex].Cells["ID"].Value);
                string comentario = ObtenerComentarioEntrada(entradaId);

                if (!string.IsNullOrWhiteSpace(comentario))
                {
                    // Mostrar el comentario en un MessageBox
                    MessageBox.Show(comentario, "Comentario de la Entrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Esta entrada no tiene comentarios.", "Sin Comentarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // ⭐ NUEVO MÉTODO: Obtener el comentario completo de una entrada
        private string ObtenerComentarioEntrada(int entradaId)
        {
            return localDM.getComentarioEntrada(entradaId);
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
