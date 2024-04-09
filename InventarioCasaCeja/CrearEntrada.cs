using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace InventarioCasaCeja
{
    public partial class CrearEntrada : Form
    {
        WebDataManager webDM;
        LocaldataManager localDM;
        List<ProductoEntrada> productos;
        BindingSource tablasource;
        bool hasTemporal = false;
        int sucursal;
        Producto currentProd = null;
        public CrearEntrada(WebDataManager webDataManager, LocaldataManager localdataManager, int idsucursal)
        {
            InitializeComponent();
            this.webDM = webDataManager;
            this.localDM = localdataManager;
            productos = new List<ProductoEntrada>();
            tablasource = new BindingSource();
            tablasource.DataSource = productos;
            tabla.DataSource = tablasource;
            this.sucursal = idsucursal;
            tabla.KeyDown += tabla_KeyDown; //agregar a salidas
        }

        private void AgregarProductoTabla(object sender, EventArgs e) //agregar a salidas, no todo, solo lo marcado
        {
            if (txtcodigo.Text.Equals("")) //agregar/modificar
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
                    //agregar 
                    // Buscar si el producto ya existe en la lista
                    var productoExistente = productos.FirstOrDefault(p => p.id == currentProd.id);
                    if (productoExistente != null)
                    {
                        MessageBox.Show("El producto ya se agrego previamente", "Advertencia");
                        return;
                    }
                    else
                    {
                        productos.Add(new ProductoEntrada
                        {//agregar/modificar
                            id = currentProd.id,
                            codigo = currentProd.codigo,
                            nombre = currentProd.nombre,
                        });
                    }
                    tablasource.ResetBindings(false);
                    txtcantidad.Text = "";
                    txtcodigo.Text = "";
                    txtcosto.Text = "";
                    currentProd = null;
                }
            }
        }
        private void agregarProd(Producto producto)
        {
            currentProd = producto;
            txtcodigo.Text = producto.codigo;
        }

        private void finish_Click(object sender, EventArgs e)
        {
            if (productos.Count == 0)
            {
                MessageBox.Show("No se han agregado productos", "Advertencia");
            }
            else
            {
                CompletarEntrada ce = new CompletarEntrada(webDM, productos, hasTemporal, sucursal);
                DialogResult res = ce.ShowDialog();
                if (res == DialogResult.Yes)
                {
                    this.Close();
                }
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
                    case Keys.F1:
                        //agregar 
                        BuscarExistencia bs = new BuscarExistencia(webDM, agregarProd, sucursal, txtcodigo.Text);
                        bs.ShowDialog();
                        txtcodigo.Focus();
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
        private void CrearEntrada_Load(object sender, EventArgs e)
        {
            txtcodigo.Focus();
        }

        private void SeleccionarImagen(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                // Configura el lector de códigos QR.
                BarcodeReader barcodeReader = new BarcodeReader();

                try
                {
                    var resultado = barcodeReader.Decode(new Bitmap(filePath));

                    if (resultado != null)
                    {
                        string jsonFromQR = resultado.Text;

                        // Verifica si el JSON es válido.
                        if (!string.IsNullOrEmpty(jsonFromQR))
                        {
                            // Deserializa el JSON a una lista de productos.
                            List<Dictionary<string, object>> listaProductos = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonFromQR);

                            // Agrega los productos a la lista existente.
                            foreach (Dictionary<string, object> producto in listaProductos)
                            {
                                string idProducto = producto["idproducto"].ToString();
                                int cantidad = Convert.ToInt32(producto["cantidad"]);
                                decimal precio = Convert.ToDecimal(producto["precio"]);

                                // Obtener detalles del producto.
                                Producto productoDetallado = localDM.GetProductByIdOrCode(idProducto);

                                // Agrega la lógica para agregar los datos a la lista 'productos'.
                                productos.Add(new ProductoEntrada
                                {
                                    id = Convert.ToInt32(idProducto),
                                    codigo = productoDetallado.codigo, // Agregar código del producto
                                    nombre = productoDetallado.nombre, // Agregar nombre del producto
                                                                       // Agrega otros campos según la estructura de ProductoEntrada.
                                    cantidad = cantidad,
                                    costo = Convert.ToDouble(precio),
                                });
                            }

                            // Refresca el DataGridView con la nueva información.
                            tablasource.DataSource = null; // Establecer a null antes de volver a asignar la lista.
                            tablasource.DataSource = productos;
                            tabla.DataSource = tablasource;

                            // Esto debería ser suficiente para forzar una actualización.
                            tabla.Refresh();
                        }
                    }
                    else
                    {
                        MessageBox.Show("La imagen seleccionada no contiene un código QR válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al intentar leer el código QR: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void txtcodigo_KeyDown(object sender, KeyEventArgs e) //agregar a salidas
        {
            if (e.KeyData == Keys.Down)
            {
                tabla.Focus();
            }
            if (e.KeyData == Keys.Enter)
            {
                if (!txtcodigo.Text.Equals(""))
                {
                    AgregarProductoTabla(sender, e);
                }
            }
            else
            {
                currentProd = null;
            }
        }
        private void tabla_KeyDown(object sender, KeyEventArgs e) //agregar a salidas
        { 
            if (e.KeyCode == Keys.Enter)
            {
               int rowIndex = tabla.CurrentCell.RowIndex;
                tabla.CurrentCell = tabla.Rows[rowIndex].Cells[3];
                tabla.BeginEdit(true);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

    }
}