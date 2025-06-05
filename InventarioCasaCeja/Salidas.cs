using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Storage;
using InventarioCasaCeja.Properties;
using System.Drawing.Printing;
using NetBarcode;
using Newtonsoft.Json;
using ZXing;
using System.Drawing.Imaging; // Agrega esta línea al inicio del archivo

namespace InventarioCasaCeja
{
    public partial class Salidas : Form
    {
        string tot, titulo, direccion, vendedor, fecha, pedido, fontName, printerName;
        PrintPreviewControl printPreviewControl1;
        int fontSize;
        CurrentData data;
        Producto currentProd = null;
        WebDataManager webDM;
        LocaldataManager localDM;
        BindingList<ProductoSalida> productosImprimir;
        List<Dictionary<string, object>> productosEnvio;
        BindingSource tablasource;
        bool hasTemporal = false;
        string destino;
        int idSalida;
        double total;
        string folio;
        int sucursal;
        private bool usarPrecioVendedor = false; // Nueva variable para controlar el modo de precio       

        private void Salidas_Load(object sender, EventArgs e)
        {
            DateTime localDate = DateTime.Now;
            txtcodigo.Focus();
            idSalida = localDM.nuevaSalida(data.sucursal.id);
            boxsucursales.DataSource = data.mapasucursales.Keys.ToList();
            folio = data.sucursal.id.ToString().PadLeft(2, '0') + localDate.Day.ToString().PadLeft(2, '0') + localDate.Month.ToString().PadLeft(2, '0') + localDate.Year + "5" + (idSalida % 1000).ToString().PadLeft(3, '0');
            txtfolio.Text = folio;
            txtfecha.Text = localDate.ToString("dd/MM/yyyy");
            txtSucOrig.Text = data.sucursal.razon_social;

            // Configurar el estado inicial del botón
            ActualizarEstadoBoton();
        }

        private PrintDocument docToPrint = new PrintDocument();
        PrintPreviewDialog previewDialog = new PrintPreviewDialog();

        public Salidas(CurrentData data)
        {
            InitializeComponent();
            this.ProductoSeleccionado += AgregarProductoDirectamente;
            printPreviewControl1 = new PrintPreviewControl();
            this.sucursal = data.sucursal.id;
            this.data = data;
            this.webDM = data.webDM;
            this.localDM = data.webDM.localDM;
            fontName = data.fontName;
            fontSize = data.fontSize;
            printerName = data.printerName;
            productosImprimir = new BindingList<ProductoSalida>();
            productosEnvio = new List<Dictionary<string, object>>();
            tablasource = new BindingSource();
            tablasource.DataSource = productosImprimir;
            tabla.DataSource = tablasource;
            total = 0;
            (previewDialog as Form).WindowState = FormWindowState.Maximized;
            tabla.Columns["idproducto"].HeaderText = "Importe";
            // Suscribir al evento CellFormatting para mostrar el importe calculado
            tabla.CellFormatting += Tabla_CellFormatting;

            // Configurar el estado inicial del botón
            ActualizarEstadoBoton();
        }

        // ***** NUEVOS MÉTODOS PARA PRECIO VENDEDOR *****
        // Método para actualizar la apariencia del botón
        private void ActualizarEstadoBoton()
        {
            if (usarPrecioVendedor)
            {
                Bpvendedor.Text = "P VENDEDOR ACTIVO";
                Bpvendedor.BackColor = Color.LightGreen;
                Bpvendedor.ForeColor = Color.DarkGreen;
                // Opcional: cambiar color de fondo de la tabla para indicar modo vendedor
                tabla.BackgroundColor = Color.LightYellow;
            }
            else
            {
                Bpvendedor.Text = "PRECIO VENDEDOR (F3)";
                Bpvendedor.BackColor = SystemColors.Control;
                Bpvendedor.ForeColor = SystemColors.ControlText;
                tabla.BackgroundColor = SystemColors.Window;
            }
        }

        // Método para actualizar precios de productos existentes en la tabla
        private void ActualizarPreciosProductos()
        {
            foreach (var productoSalida in productosImprimir)
            {
                // Obtener el producto completo de la base de datos para tener acceso al precio vendedor
                Producto productoCompleto = localDM.GetProductByIdOrCode(productoSalida.idproducto.ToString());

                if (productoCompleto != null)
                {
                    // Validar precio vendedor si está activo
                    if (usarPrecioVendedor && !ValidarPrecioVendedor(productoCompleto))
                    {
                        continue; // Saltar este producto si no tiene precio vendedor válido
                    }

                    // Actualizar precio según el modo activo
                    double nuevoPrecio = ObtenerPrecioProducto(productoCompleto);
                    productoSalida.precio = nuevoPrecio;

                    // Actualizar también en productosEnvio
                    var productoEnvio = productosEnvio.FirstOrDefault(p => Convert.ToInt32(p["idproducto"]) == productoSalida.idproducto);
                    if (productoEnvio != null)
                    {
                        productoEnvio["precio"] = Math.Round(nuevoPrecio, 2);
                    }
                }
            }

            // Refrescar la tabla para mostrar los nuevos precios
            tablasource.ResetBindings(false);
        }

        // Método auxiliar para obtener el precio correcto
        private double ObtenerPrecioProducto(Producto producto)
        {
            return usarPrecioVendedor ? producto.vendedor : producto.menudeo;
        }

        // Validación para cuando no hay precio vendedor
        private bool ValidarPrecioVendedor(Producto producto)
        {
            if (usarPrecioVendedor && (producto.vendedor <= 0))
            {
                MessageBox.Show($"El producto '{producto.nombre}' no tiene precio de vendedor configurado.",
                               "Precio no disponible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        // ***** EVENTO DEL BOTÓN PRECIO VENDEDOR *****
        private void Bpvendedor_Click(object sender, EventArgs e)
        {
            usarPrecioVendedor = !usarPrecioVendedor;
            ActualizarEstadoBoton();
            ActualizarPreciosProductos();

            // Mostrar mensaje informativo
            string mensaje = usarPrecioVendedor ?
                "Modo PRECIO VENDEDOR activado. Los nuevos productos usarán precio de vendedor." :
                "Modo PRECIO NORMAL activado. Los nuevos productos usarán precio de menudeo.";

            MessageBox.Show(mensaje, "Cambio de Precio", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ***** MÉTODOS MODIFICADOS *****

        private void Tabla_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {           
            if (tabla.Columns[e.ColumnIndex].Name == "idproducto")
            {
                // Obtener la fila actual
                DataGridViewRow fila = tabla.Rows[e.RowIndex];

                // Asegurarse de que la fila no sea la fila de encabezado y tenga datos
                if (fila.DataBoundItem != null)
                {
                    // Obtener el objeto ProductoSalida de la fila
                    ProductoSalida producto = (ProductoSalida)fila.DataBoundItem;
                    tabla.Columns["precio"].DefaultCellStyle.Format = "N2";

                    // Calcular el importe (cantidad * precio)
                    double importe = producto.cantidad * producto.precio;

                    // Formatear el valor para mostrarlo en la celda (opcional: formato moneda)
                    e.Value = importe.ToString("N2"); // "N2" formatea a número con 2 decimales
                    e.FormattingApplied = true; // Indicar que hemos aplicado el formato
                }
            }

            // Colorear la columna de precio según el modo activo
            if (tabla.Columns[e.ColumnIndex].Name == "precio")
            {
                if (usarPrecioVendedor)
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                    e.CellStyle.ForeColor = Color.DarkGreen;
                }
                else
                {
                    e.CellStyle.BackColor = SystemColors.Window;
                    e.CellStyle.ForeColor = SystemColors.WindowText;
                }
            }
        }

        private void AgregarProductoDirectamente(Producto producto)
        {
            tabla.EndEdit(); // Verificar y guardar cambios en la celda antes de agregar un nuevo producto.
            if (producto == null) return;

            if (producto.id == 0)
            {
                hasTemporal = true;
            }

            if (productosImprimir.Any(p => p.idproducto == producto.id))
            {
                MessageBox.Show("El producto ya se agregó previamente", "Advertencia");
                LimpiarEntrada();
                return;
            }

            // Validar precio vendedor si está activo
            if (usarPrecioVendedor && !ValidarPrecioVendedor(producto))
            {
                LimpiarEntrada();
                return;
            }

            int cantidad = 1; // Usar 1 como valor predeterminado.

            // Determinar qué precio usar según el estado del botón
            double precioAUsar = ObtenerPrecioProducto(producto);

            // Agregar a productosImprimir y productosEnvio con el precio correcto
            AgregarOActualizarProducto(producto.id, producto.codigo, producto.nombre, cantidad, precioAUsar, data.mapamedidasinv[producto.medida_id]);

            LimpiarEntrada();
        }

        private void tabla_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != tabla.Columns["cantidad"].Index) return;

            var fila = tabla.Rows[e.RowIndex];
            int idProducto = Convert.ToInt32(fila.Cells["idproducto"].Value);
            int cantidad = Convert.ToInt32(fila.Cells["cantidad"].Value);
            string codigo = fila.Cells["codigo"].Value.ToString();
            string nombre = fila.Cells["nombre"].Value.ToString();
            double precio = Convert.ToDouble(fila.Cells["precio"].Value);
            string unidad = fila.Cells["unidad"].Value.ToString();

            AgregarOActualizarProducto(idProducto, codigo, nombre, cantidad, precio, unidad);
        }

        private void AgregarOActualizarProducto(int id, string codigo, string nombre, int cantidad, double precio, string unidad)
        {
            var productoImprimir = productosImprimir.FirstOrDefault(p => p.idproducto == id);
            if (productoImprimir != null)
            {
                productoImprimir.cantidad = cantidad;
                // Mantener el precio actual si ya existe el producto
            }
            else
            {
                productosImprimir.Insert(0, new ProductoSalida
                {
                    idproducto = id,
                    codigo = codigo,
                    nombre = nombre,
                    cantidad = cantidad,
                    precio = precio,
                    unidad = unidad
                });
            }

            var productoEnvio = productosEnvio.FirstOrDefault(p => Convert.ToInt32(p["idproducto"]) == id);
            if (productoEnvio != null)
            {
                productoEnvio["cantidad"] = cantidad;
                // Actualizar precio en productosEnvio solo si es un producto nuevo o se cambió el modo
            }
            else
            {
                productosEnvio.Add(new Dictionary<string, object>
                {
                    { "idproducto", id },
                    { "cantidad", cantidad },
                    { "precio", Math.Round(precio, 2) }
                });
            }

            tablasource.ResetBindings(false);
        }

        private void LimpiarEntrada()
        {
            txtcodigo.Text = "";
            currentProd = null;
            if (tabla.Rows.Count > 0)
            {
                tabla.Focus();
                tabla.CurrentCell = tabla.Rows[0].Cells[3];
                tabla.BeginEdit(true);
            }
        }

        // ***** RESTO DEL CÓDIGO ORIGINAL *****

        private void cargarTicketCarta(string fechaSalida)
        {
            //titulo = "CASA CEJA S.A. DE C.V.";
            titulo = "CASA CEJA";
            direccion = data.sucursal.razon_social + " " + data.sucursal.direccion;
            vendedor = "DESTINO:\n" + destino;
            fecha = "FECHA:\n" + fechaSalida;
            pedido = "PEDIDO:\n" + folio;
            tot = "TOTAL: $" + total.ToString("0.00");

            createdoc();
        }

        private void createdoc()
        {

            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "test.txt");
            // Construct the PrintPreviewControl.

            //// Set location, name, and dock style for printPreviewControl1.
            //this.printPreviewControl1.Name = "printPreviewControl1";

            // Set the Document property to the PrintDocument 
            // for which the PrintPage event has been handled.
            this.previewDialog.Document = docToPrint;
            //this.previewDialog.Zoom = 2;
            // Set the document name. This will show be displayed when 
            // the document is loading into the control.
            this.previewDialog.Document.DocumentName = path;
            this.previewDialog.Document.PrinterSettings.PrinterName = data.webDM.localDM.impresora;
            this.previewDialog.Document.DefaultPageSettings.Landscape = true;

            // Set the UseAntiAlias property to true so fonts are smoothed
            // by the operating system.
            //this.printPreview.UseAntiAlias = true;
            // Add the control to the form.

            // Associate the event-handling method with the
            // document's PrintPage event.
            this.docToPrint.PrintPage +=
                new System.Drawing.Printing.PrintPageEventHandler(
                docToPrint_PrintPage);
        }
        private void agregarProd(Producto producto)
        {
            currentProd = producto;
            txtcodigo.Text = producto.codigo;
            ProductoSeleccionado?.Invoke(producto);
        }

        public delegate void ProductoSeleccionadoHandler(Producto producto);
        public event ProductoSeleccionadoHandler ProductoSeleccionado;

        private void docToPrint_PrintPage(
    object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            // Insert code to render the page here.
            // This code will be called when the control is drawn.

            // The following code will render a simple
            // message on the document in the control.
            string text1 = titulo;
            string text2 = direccion;
            string text3 = vendedor;
            string text4 = fecha;
            string text5 = pedido;
            string text6 = tot;
            //StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            //format.Alignment = StringAlignment.Center;
            //System.Drawing.Font printFont =
            //    new Font(fontName, fontSize, FontStyle.Regular);

            //e.Graphics.DrawString(text1, printFont,
            //    Brushes.Black, 50, 50);

            Font font = new Font(
               "Arial Black",
               16,
               FontStyle.Bold,
               GraphicsUnit.Point);
            Font font2 = new Font(
               "Arial",
               8,
               GraphicsUnit.Point);
            Font font3 = new Font(
               "Arial",
               8,
               FontStyle.Bold,
               GraphicsUnit.Point);
            Rectangle rect = new Rectangle(25, 25, 510, 120);
            StringFormat alignCenter = new StringFormat();
            alignCenter.Alignment = StringAlignment.Center;
            SolidBrush solidBrush = new SolidBrush(Color.Black);

            StringFormat alignRight = new StringFormat();
            alignRight.Alignment = StringAlignment.Far;

            StringFormat alignMiddle = new StringFormat();
            alignMiddle.Alignment = StringAlignment.Center;
            alignMiddle.LineAlignment = StringAlignment.Center;
            Font font4 = new Font(
               "Arial",
               6,
               FontStyle.Bold,
               GraphicsUnit.Point);
            Font font5 = new Font(
               "Arial",
               6,
               GraphicsUnit.Point);

            Font font6 = new Font(
               "Arial",
               10,
               FontStyle.Underline | FontStyle.Bold,
               GraphicsUnit.Point);

            e.Graphics.DrawString("1/2", font5, solidBrush, new Rectangle(25, 25, 35, 35), alignMiddle);

            e.Graphics.DrawString(text1, font, solidBrush, rect, alignCenter);
            e.Graphics.DrawString(text2, font2, solidBrush, new Rectangle(25, 55, rect.Width, 100), alignCenter);
            e.Graphics.DrawString(text3, font2, solidBrush, new Rectangle(25, 80, rect.Width / 3, 100), alignCenter);
            e.Graphics.DrawString(text4, font2, solidBrush, new Rectangle(rect.Width / 3 + 25, 80, rect.Width / 3, 100), alignCenter);
            e.Graphics.DrawString(text5, font3, solidBrush, new Rectangle(2 * (rect.Width / 3) + 25, 80, rect.Width / 3, 100), alignCenter);
            Pen pen = Pens.Black;
            Pen pen2 = Pens.Red;
            e.Graphics.DrawRectangle(pen, rect);
            e.Graphics.DrawLine(pen, rect.X + 5, 70, rect.Width + rect.X - 10, 70);

            Rectangle barRect = new Rectangle(2 * (rect.Width / 3) + 30, 107, rect.Width / 3 - 10, 33);
            Rectangle barRectD = new Rectangle(rect.Width + 2 * (rect.Width / 3) + 55, 107, rect.Width / 3 - 10, 33);
            var barcode = new Barcode(folio, NetBarcode.Type.Code128);
            Image bar = barcode.GetImage();
            e.Graphics.DrawImage(bar, barRect);
            e.Graphics.DrawImage(bar, barRectD);

            Rectangle rectA = new Rectangle(25, rect.Height + 45, (int)(rect.Width * 0.15), 10);
            Rectangle rectB = new Rectangle(25 + rectA.Width, rect.Height + 45, (int)(rect.Width * 0.10), 10);
            Rectangle rectC = new Rectangle(25 + rectA.Width + rectB.Width, rect.Height + 45, (int)(rect.Width * 0.35), 10);
            Rectangle rectD = new Rectangle(25 + rectA.Width + rectB.Width + rectC.Width, rect.Height + 45, (int)(rect.Width * 0.2), 10);
            Rectangle rectE = new Rectangle(25 + rectA.Width + rectB.Width + rectC.Width + rectD.Width, rect.Height + 45, (int)(rect.Width * 0.2), 10);

            e.Graphics.DrawString("CANTIDAD", font4, solidBrush, rectA, alignCenter);
            e.Graphics.DrawString("UNIDAD", font4, solidBrush, rectB, alignCenter);
            e.Graphics.DrawString("ARTICULO", font4, solidBrush, rectC, alignCenter);
            e.Graphics.DrawString("P.UNIT", font4, solidBrush, rectD, alignCenter);
            e.Graphics.DrawString("IMPORTE", font4, solidBrush, rectE, alignCenter);
            e.Graphics.DrawString(text6, font6, solidBrush, new Rectangle(330, 800, 200, 20), alignRight);

            Rectangle rectA1 = new Rectangle(rect.Width + 50, rect.Height + 45, (int)(rect.Width * 0.15), 10);
            Rectangle rectB1 = new Rectangle(rect.Width + 50 + rectA.Width, rect.Height + 45, (int)(rect.Width * 0.10), 10);
            Rectangle rectC1 = new Rectangle(rect.Width + 50 + rectA.Width + rectB.Width, rect.Height + 45, (int)(rect.Width * 0.35), 10);
            Rectangle rectD1 = new Rectangle(rect.Width + 50 + rectA.Width + rectB.Width + rectC.Width, rect.Height + 45, (int)(rect.Width * 0.2), 10);
            Rectangle rectE1 = new Rectangle(rect.Width + 50 + rectA.Width + rectB.Width + rectC.Width + rectD.Width, rect.Height + 45, (int)(rect.Width * 0.2), 10);
            e.Graphics.DrawString("CANTIDAD", font4, solidBrush, rectA1, alignCenter);
            e.Graphics.DrawString("UNIDAD", font4, solidBrush, rectB1, alignCenter);
            e.Graphics.DrawString("ARTICULO", font4, solidBrush, rectC1, alignCenter);
            e.Graphics.DrawString("P.UNIT", font4, solidBrush, rectD1, alignCenter);
            e.Graphics.DrawString("IMPORTE", font4, solidBrush, rectE1, alignCenter);
            e.Graphics.DrawString(text6, font6, solidBrush, new Rectangle(rect.Width + 25 + 330, 800, 200, 20), alignRight);

            for (int i = 0; i < productosImprimir.Count; i++)
            {
                Rectangle tabrect1 = new Rectangle(25, rect.Height + 55 + i * 20, (int)(rect.Width * 0.075), 20);
                Rectangle tabrect2 = new Rectangle(25 + tabrect1.Width, rect.Height + 55 + i * 20, (int)(rect.Width * 0.075), 20);
                Rectangle tabrect3 = new Rectangle(25 + rectA.Width, rect.Height + 55 + i * 20, (int)(rect.Width * 0.10), 20);
                Rectangle tabrect4 = new Rectangle(25 + rectA.Width + rectB.Width, rect.Height + 55 + i * 20, (int)(rect.Width * 0.35), 20);
                Rectangle tabrect5 = new Rectangle(25 + rectA.Width + rectB.Width + rectC.Width, rect.Height + 55 + i * 20, (int)(rect.Width * 0.2), 20);
                Rectangle tabrect6 = new Rectangle(25 + rectA.Width + rectB.Width + rectC.Width + rectD.Width, rect.Height + 55 + i * 20, (int)(rect.Width * 0.2), 20);
                e.Graphics.DrawRectangle(pen, tabrect1);
                e.Graphics.DrawRectangle(pen, tabrect2);
                e.Graphics.DrawRectangle(pen, tabrect3);
                e.Graphics.DrawRectangle(pen, tabrect4);
                e.Graphics.DrawRectangle(pen, tabrect5);
                e.Graphics.DrawRectangle(pen, tabrect6);
                e.Graphics.DrawString((i + 1).ToString(), font5, solidBrush, tabrect1, alignMiddle);
                e.Graphics.DrawString(productosImprimir[i].cantidad.ToString(), font5, solidBrush, tabrect2, alignMiddle);
                e.Graphics.DrawString(productosImprimir[i].unidad, font5, solidBrush, tabrect3, alignMiddle);
                e.Graphics.DrawString(productosImprimir[i].nombre, font5, solidBrush, tabrect4, alignMiddle);
                e.Graphics.DrawString(productosImprimir[i].precio.ToString("0.00"), font5, solidBrush, tabrect5, alignMiddle);
                e.Graphics.DrawString((productosImprimir[i].precio * productosImprimir[i].cantidad).ToString("0.00"), font5, solidBrush, tabrect6, alignMiddle);

                Rectangle tabrect2D = new Rectangle(rect.Width + 50 + tabrect1.Width, rect.Height + 55 + i * 20, (int)(rect.Width * 0.075), 20);
                Rectangle tabrect1D = new Rectangle(rect.Width + 50, rect.Height + 55 + i * 20, (int)(rect.Width * 0.075), 20);
                Rectangle tabrect3D = new Rectangle(rect.Width + 50 + rectA.Width, rect.Height + 55 + i * 20, (int)(rect.Width * 0.10), 20);
                Rectangle tabrect4D = new Rectangle(rect.Width + 50 + rectA.Width + rectB.Width, rect.Height + 55 + i * 20, (int)(rect.Width * 0.35), 20);
                Rectangle tabrect5D = new Rectangle(rect.Width + 50 + rectA.Width + rectB.Width + rectC.Width, rect.Height + 55 + i * 20, (int)(rect.Width * 0.2), 20);
                Rectangle tabrect6D = new Rectangle(rect.Width + 50 + rectA.Width + rectB.Width + rectC.Width + rectD.Width, rect.Height + 55 + i * 20, (int)(rect.Width * 0.2), 20);
                e.Graphics.DrawRectangle(pen, tabrect1D);
                e.Graphics.DrawRectangle(pen, tabrect2D);
                e.Graphics.DrawRectangle(pen, tabrect3D);
                e.Graphics.DrawRectangle(pen, tabrect4D);
                e.Graphics.DrawRectangle(pen, tabrect5D);
                e.Graphics.DrawRectangle(pen, tabrect6D);
                e.Graphics.DrawString((i + 1).ToString(), font5, solidBrush, tabrect1D, alignMiddle);
                e.Graphics.DrawString(productosImprimir[i].cantidad.ToString(), font5, solidBrush, tabrect2D, alignMiddle);
                e.Graphics.DrawString(productosImprimir[i].unidad, font5, solidBrush, tabrect3D, alignMiddle);
                e.Graphics.DrawString(productosImprimir[i].nombre, font5, solidBrush, tabrect4D, alignMiddle);
                e.Graphics.DrawString(productosImprimir[i].precio.ToString("0.00"), font5, solidBrush, tabrect5D, alignMiddle);
                e.Graphics.DrawString((productosImprimir[i].precio * productosImprimir[i].cantidad).ToString("0.00"), font5, solidBrush, tabrect6D, alignMiddle);
            }


            e.Graphics.DrawString("2/2", font5, solidBrush, new Rectangle(rect.Width + 50, 25, 35, 35), alignMiddle);
            Rectangle rect2 = new Rectangle(rect.Width + 50, 25, rect.Width, rect.Height);
            e.Graphics.DrawString(text1, font, solidBrush, rect2, alignCenter);
            e.Graphics.DrawString(text2, font2, solidBrush, new Rectangle(rect2.X, 55, rect.Width, 100), alignCenter);
            e.Graphics.DrawString(text3, font2, solidBrush, new Rectangle(rect2.X, 80, rect.Width / 3, 100), alignCenter);
            e.Graphics.DrawString(text4, font2, solidBrush, new Rectangle(rect.Width / 3 + rect2.X, 80, rect.Width / 3, 100), alignCenter);
            e.Graphics.DrawString(text5, font3, solidBrush, new Rectangle(2 * (rect.Width / 3) + rect2.X, 80, rect.Width / 3, 100), alignCenter);
            e.Graphics.DrawRectangle(pen, rect2);

            e.Graphics.DrawLine(pen, rect2.X + 5, 70, rect2.Width + rect2.X - 10, 70);

            // Obtener el ancho y alto del documento
            int documentWidth = e.PageBounds.Width;
            int documentHeight = e.PageBounds.Height;

            // Tamaño y márgenes para los códigos QR
            int qrWidth = 100;
            int qrHeight = 100;
            int margin = 10;
            int spaceBetweenQRs = 450;

            // Calcular la posición para el primer código QR
            int xPos1 = margin;
            // Ajusta esta posición según tus necesidades
            int yPos = documentHeight - qrHeight - margin;

            // Obtén la ruta de la carpeta donde se guardará la imagen
            string folderPath = Path.Combine(Application.StartupPath, "QRImages");

            // Verifica si la carpeta existe; si no, créala
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //SE ANEXA EL CODIGO QUE GENERA EL PRIMER QR
            string json1 = JsonConvert.SerializeObject(productosEnvio);
            Bitmap qrImage1 = GenerarQR(json1);

            // Nombre del archivo para el código QR, incluyendo el valor de la variable "folio"
            string qrFileName = $"QR_{folio}.png";
            string filePath = Path.Combine(folderPath, qrFileName);

            Rectangle qrBounds1 = new Rectangle(xPos1, yPos, qrWidth, qrHeight);
            e.Graphics.DrawImage(qrImage1, qrBounds1);
            qrImage1.Save(filePath, ImageFormat.Png);

            // Calcular la posición para el segundo código QR
            int xPos2 = xPos1 + qrWidth + spaceBetweenQRs;

            //SE ANEXA EL CODIGO QUE GENERA EL SEGUNDO QR
            string json2 = JsonConvert.SerializeObject(productosEnvio);
            // Reemplaza esto con el texto adecuado
            Bitmap qrImage2 = GenerarQR(json2);

            Rectangle qrBounds2 = new Rectangle(xPos2, yPos, qrWidth, qrHeight);
            e.Graphics.DrawImage(qrImage2, qrBounds2);

        }

        private Bitmap GenerarQR(string texto)
        {
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            barcodeWriter.Options = new ZXing.QrCode.QrCodeEncodingOptions
            {
                Width = 1500,
                Height = 1500
            };

            return new Bitmap(barcodeWriter.Write(texto));
        }

        // Método para guardar el documento en PDF usando Microsoft Print to PDF
        private void GuardarComoPDF(string rutaPDF)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(docToPrint_PrintPage);
            pd.PrinterSettings = new PrinterSettings
            {
                PrinterName = "Microsoft Print to PDF",
                PrintToFile = true,
                PrintFileName = rutaPDF
            };
            pd.DefaultPageSettings.Landscape = true;
            pd.Print();
        }

        private void finish_Click(object sender, EventArgs e)
        {
            string destino = boxsucursales.SelectedItem.ToString();
            if (destino == data.sucursal.razon_social)
            {
                MessageBox.Show("No es posible enviar productos a la misma sucursal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (boxsucursales.SelectedIndex < 0)
            {
                MessageBox.Show("Favor de seleccionar la sucursal destino.", "Advertencia");
            }
            else
            {
                if (productosEnvio.Count == 0)
                {
                    MessageBox.Show("Aún no se han agregado productos.", "Advertencia");
                }
                else
                {
                    total = 0;
                    foreach (var producto in productosImprimir)
                    {
                        total += producto.cantidad * producto.precio;
                    }

                    DateTime localDate = DateTime.Now;
                    destino = boxsucursales.SelectedItem.ToString();
                    Salida salida = new Salida
                    {
                        id_sucursal_origen = data.sucursal.id,
                        id_sucursal_destino = data.mapasucursales[destino],
                        folio = folio,
                        fecha_salida = localDate.ToString("yyyy/MM/dd HH:mm:ss"),
                        usuario_id = webDM.activeUser.id,
                        total_importe = total,
                        productos = JsonConvert.SerializeObject(productosEnvio),
                    };
                    localDM.guardarSalidaTemporal(salida, idSalida);
                    localDM.GuardarSalidaLocal(salida);
                    enviarSalida(salida);
                    cargarTicketCarta(localDate.ToString("HH:mm dd/MM/yyyy"));

                    // Mostrar la vista previa
                    previewDialog.ShowDialog();

                    // Configuración de archivo
                    string carpetaPrincipal = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CasaCejaDocs");
                    string subcarpeta = Path.Combine(carpetaPrincipal, "Inventario");
                    string nombrePDF = $"Salida_{folio}.pdf";
                    string rutaPDF = Path.Combine(subcarpeta, nombrePDF);

                    // Crear la carpeta principal y la subcarpeta si no existen
                    if (!Directory.Exists(carpetaPrincipal))
                        Directory.CreateDirectory(carpetaPrincipal);

                    if (!Directory.Exists(subcarpeta))
                        Directory.CreateDirectory(subcarpeta);

                    // Validar si el archivo ya existe
                    if (File.Exists(rutaPDF))
                    {
                        var respuesta = MessageBox.Show("El archivo ya existe. ¿Deseas sobrescribirlo?",
                                                          "Archivo Existente", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (respuesta == DialogResult.No)
                            return;
                    }

                    // Si existe, eliminar el archivo para sobrescribirlo
                    if (File.Exists(rutaPDF))
                    {
                        File.Delete(rutaPDF);
                    }

                    // Llamar al método para guardar el documento como PDF
                    GuardarComoPDF(rutaPDF);
                    MessageBox.Show($"{nombrePDF} generado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Limpiar la lista de productos e inicializar el binding nuevamente
                    productosImprimir.Clear();
                    tablasource.ResetBindings(false);
                }
            }
        }

        async void enviarSalida(Salida salida)
        {
            if (await webDM.SendSalidaAsync(salida))
            {
                localDM.confirmarSalidaTemporal(idSalida);
                MessageBox.Show("Registro de salida enviado", "Éxito");

                // Restar existencias de los productos en la sucursal de origen
                foreach (var producto in productosImprimir)
                {
                    bool resultado = await webDM.restarExistencia(salida.id_sucursal_origen, producto.idproducto, producto.cantidad);
                    if (!resultado)
                    {
                        MessageBox.Show($"No se pudo restar la existencia para el producto ID: {producto.idproducto}", "Error");
                    }
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("No se pudo contactar al servidor, el registro se ha almacenado localmente", "Advertencia");
                this.Close();
            }
        }

        private void tabla_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= Control_KeyPress; // Remueve el manejador de eventos previo para evitar duplicados
            if (tabla.CurrentCell.ColumnIndex == 3)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += Control_KeyPress;
                }
            }
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Esta seguro que desea salir?", "Advertencia", MessageBoxButtons.YesNo))
            {
                this.Close();
            }
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
                        exit_button.PerformClick();
                        break;
                    case Keys.F1:
                        txtcodigo.Focus();
                        break;
                    case Keys.F2:
                        BuscarExistencia bs = new BuscarExistencia(webDM, agregarProd, sucursal, txtcodigo.Text);
                        bs.ShowDialog();
                        tabla.Focus();
                        SendKeys.Send("{RIGHT}");
                        SendKeys.Send("{DOWN}");
                        break;
                    case Keys.F3: // Nueva tecla para alternar precio vendedor
                        Bpvendedor_Click(this, new EventArgs());
                        break;
                    case Keys.F5:
                        boxsucursales.Focus();
                        boxsucursales.DroppedDown = true;
                        break;
                    case Keys.F6:
                        finish.PerformClick();
                        break;
                    case Keys.Down:
                        tabla.Focus();
                        break;
                    case Keys.Delete:
                        quitarProdButton_Click(this, new EventArgs());
                        break;
                    default:
                        return base.ProcessDialogKey(keyData);
                }
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void txtcodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                tabla.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Producto producto = localDM.GetProductByCode(txtcodigo.Text);
                if (producto != null)
                {
                    AgregarProductoDirectamente(producto);
                }
                else
                {
                    MessageBox.Show("El código ingresado no se encontró o no existe.", "Producto no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtcodigo.Focus();
                }
            }
        }

        private void tabla_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                txtcodigo.Focus();
            }
            if (e.KeyCode == Keys.F2)
            {
                BuscarExistencia bs = new BuscarExistencia(webDM, agregarProd, sucursal, txtcodigo.Text);
                bs.ShowDialog();
                tabla.Focus();
                SendKeys.Send("{RIGHT}");
                SendKeys.Send("{DOWN}");
            }
            if (e.KeyCode == Keys.F3) // Nueva tecla para alternar precio vendedor
            {
                Bpvendedor_Click(this, new EventArgs());
            }
            //if (e.KeyCode == Keys.Enter){
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                if (tabla.CurrentCell != null && tabla.RowCount > 0)
                {
                    int rowIndex = tabla.CurrentCell.RowIndex;
                    tabla.CurrentCell = tabla.Rows[rowIndex].Cells[3];
                    tabla.BeginEdit(true);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            if (e.KeyCode == Keys.Delete)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    quitarProdButton_Click(sender, e);
                }
            }
        }

        private void boxsucursales_keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tabla.Focus();
            }
        }

        private void quitarProdButton_Click(object sender, EventArgs e)
        {
            if (tabla.CurrentRow?.DataBoundItem != null)
            {
                ProductoSalida productoSeleccionado = (ProductoSalida)tabla.CurrentRow.DataBoundItem;

                // Elimina el producto de productosImprimir
                productosImprimir.Remove(productoSeleccionado);

                // Encuentra y elimina el producto correspondiente de productosEnvio
                var productoAQuitar = productosEnvio.FirstOrDefault(p => Convert.ToInt32(p["idproducto"]) == productoSeleccionado.idproducto);
                if (productoAQuitar != null)
                {
                    productosEnvio.Remove(productoAQuitar);
                }

                // Actualiza el BindingSource
                tablasource.ResetBindings(false);
                Console.WriteLine("producto cosa", productoSeleccionado.cantidad);
            }
        }

        private void tabla_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tabla.CurrentCell != null && tabla.RowCount > 0)
            {
                int rowIndex = tabla.CurrentCell.RowIndex;
                tabla.CurrentCell = tabla.Rows[rowIndex].Cells[3];
                tabla.BeginEdit(true);
            }
        }
    }
}