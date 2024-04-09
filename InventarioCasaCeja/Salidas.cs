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
        List<ProductoSalida> productosImprimir;
        List<Dictionary<string, object>> productosEnvio;
        BindingSource tablasource;
        bool hasTemporal = false;
        string destino;
        int idSalida;
        double total;
        string folio;

        private void Salidas_Load(object sender, EventArgs e)
        {
            DateTime localDate = DateTime.Now;
            txtcodigo.Focus();
            idSalida = localDM.nuevaSalida(data.sucursal.id);
            boxsucursales.DataSource = data.mapasucursales.Keys.ToList();
            folio = data.sucursal.id.ToString().PadLeft(2, '0') + localDate.Day.ToString().PadLeft(2, '0') + localDate.Month.ToString().PadLeft(2, '0') + localDate.Year + "5" + (idSalida%1000).ToString().PadLeft(3, '0');
            txtfolio.Text = folio;
            txtfecha.Text = localDate.ToString("dd/MM/yyyy");
        }

        private PrintDocument docToPrint = new PrintDocument();
        PrintPreviewDialog previewDialog = new PrintPreviewDialog();
        public Salidas(CurrentData data)
        {            InitializeComponent();
            printPreviewControl1 = new PrintPreviewControl();
            
            this.data = data;
            this.webDM = data.webDM;
            this.localDM = data.webDM.localDM;
            fontName = data.fontName;
            fontSize = data.fontSize;
            printerName = data.printerName;
            productosImprimir = new List<ProductoSalida>();
            productosEnvio = new List<Dictionary<string, object>>();
            tablasource = new BindingSource();
            tablasource.DataSource = productosImprimir;
            tabla.DataSource = tablasource;
            total = 0;
            (previewDialog as Form).WindowState = FormWindowState.Maximized;
            //tabla.KeyDown += tabla_KeyDown;

        }
        private void cargarTicketCarta( string fechaSalida)
        {
            titulo = "CASA CEJA S.A. DE C.V.";
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
        }

        private void txtcodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (!txtcodigo.Text.Equals(""))
                    currentProd = localDM.GetProductByCode(txtcodigo.Text);
                if (currentProd == null)
                {
                    BuscarExistencia bs = new BuscarExistencia(webDM, agregarProd, data.sucursal.id, txtcodigo.Text);
                    bs.ShowDialog();
                }
                txtcantidad.Focus();
            }
            else
            {
                currentProd = null;
            }
        }

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
        private void AgregarProductoTabla(object sender, EventArgs e)
        {
            if (txtcodigo.Text.Equals(""))
            {
                MessageBox.Show("Debes ingresar todos los datos de producto", "Advertencia");
            }
            else
            {
                if (currentProd == null)
                    currentProd = localDM.GetProductByCode(txtcodigo.Text);
                if (currentProd == null)
                {
                    MessageBox.Show("Producto no encontrado, favor de dar de alta", "Advertencia");
                }
                else
                {
                    if (currentProd.id == 0)
                    {
                        hasTemporal = true;
                    }
                    int c = 0;

                    var productoExistente = productosImprimir.FirstOrDefault(p => p.idproducto == currentProd.id);
                    if (productoExistente != null)
                    {
                        MessageBox.Show("El producto ya se agrego previamente", "Advertencia");
                        return;
                    }
                    else
                    {
                        productosImprimir.Add(new ProductoSalida
                        {
                            idproducto = currentProd.id,
                            codigo = currentProd.codigo,
                            nombre = currentProd.nombre,
                            cantidad = c,
                            precio = currentProd.menudeo,
                            unidad = data.mapamedidasinv[currentProd.medida_id]
                        });
                    }

                    Dictionary<string, object> prod = new Dictionary<string, object>();
                    prod["idproducto"] = currentProd.id;
                    prod["cantidad"] = c;
                    prod["precio"] = Math.Round(currentProd.menudeo, 2);
                    productosEnvio.Add(prod);
                    tablasource.ResetBindings(false);
                    total += Math.Round(c * currentProd.menudeo, 2);
                    txtcantidad.Text = "";
                    txtcodigo.Text = "";
                    currentProd = null;
                }
            }
        }

        private void finish_Click(object sender, EventArgs e)
        {
            if (boxsucursales.SelectedIndex < 0)
            {
                MessageBox.Show("Favor de seleccionar la sucursal destino", "Advertencia");
            }
            else
            {
                if (productosEnvio.Count == 0)
                {
                    MessageBox.Show("Aún no se han agregado productos", "Advertencia");
                }
                else
                {
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
                    enviarSalida(salida);
                    cargarTicketCarta(localDate.ToString("HH:mm dd/MM/yyyy"));
                    //Aqui almaceno la lista de productos a imprimir para generar el
                    //List<ProductoSalida> listaProductos = productosImprimir;
                    //string json = JsonConvert.SerializeObject(listaProductos);

                    previewDialog.ShowDialog();
                    productosImprimir.Clear();
                    tablasource.ResetBindings(false);
                }                
            }            
        }
        async void enviarSalida(Salida salida)
        {
            if(await webDM.SendSalidaAsync(salida))
            {
                localDM.confirmarSalidaTemporal(idSalida);
                MessageBox.Show("Registro de salida enviado", "Éxito");
            }
            else
            {
                MessageBox.Show("No se pudo contactar al servidor, el registro se ha almacenado localmente", "Advertencia");
            }
        }
        private void numericInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
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
                        this.Close();
                        break;
                    case Keys.Enter:
                        if (v.Focused || finish.Focused || txtcodigo.Focused)
                            return base.ProcessDialogKey(keyData);
                        SendKeys.Send("{TAB}");
                        break;
                    case Keys.F1:
                        boxsucursales.Focus();
                        boxsucursales.DroppedDown = true;
                        break;
                    case Keys.F2:
                        txtcodigo.Focus();
                        break;
                    case Keys.F5:
                        v.PerformClick();
                        break;
                    case Keys.F6:
                        finish.PerformClick();
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

