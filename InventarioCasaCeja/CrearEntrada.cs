using Newtonsoft.Json;
using Polly.Caching;
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
            tabla.KeyDown += tabla_KeyDown;
        }

        /*
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
                    BuscarExistencia bs = new BuscarExistencia(webDM, agregarProd, sucursal, txtcodigo.Text);
                    bs.ShowDialog();
                    txtcodigo.Focus();
                }
                else
                {
                    if (currentProd.id == 0)
                    {
                        hasTemporal = true;
                    }
                    // Buscar si el producto ya existe en la lista
                    var productoExistente = productos.FirstOrDefault(p => p.id == currentProd.id);
                    if (productoExistente != null)
                    {
                        MessageBox.Show("El producto ya se agrego previamente", "Advertencia");
                        return;
                    }
                    else
                    {
                        productos.Insert(0, new ProductoEntrada
                        {
                            id = currentProd.id,
                            codigo = currentProd.codigo,
                            nombre = currentProd.nombre,
                        });
                    }
                    tablasource.ResetBindings(false);
                    txtcodigo.Text = "";
                    currentProd = null;
                }
            }
        }
        */
        private void agregarProd(Producto producto)
        {
            currentProd = producto;
            txtcodigo.Text = producto.codigo;
            AgregarProductoDirectamente(producto);
        }

        private void AgregarProductoDirectamente(Producto producto)
        {
            tabla.EndEdit(); // EndEdit se encarga de verificar si hay cambios en la celda y los guarda antes de agregar un nuevo producto.            
            if (producto != null)
            {
                if (producto.id == 0)
                {
                    hasTemporal = true;
                }
                var productoExistente = productos.FirstOrDefault(p => p.id == producto.id);
                if (productoExistente != null)
                {
                    MessageBox.Show("El producto ya se agrego previamente", "Advertencia");
                    txtcodigo.Text = "";
                    return;
                }
                else
                {
                    productos.Insert(0, new ProductoEntrada
                    {
                        id = producto.id,
                        codigo = producto.codigo,
                        nombre = producto.nombre,
                        // Aquí puedes agregar más propiedades según sea necesario
                    });
                    tablasource.ResetBindings(false);
                    txtcodigo.Text = "";
                    currentProd = null;
                }
            }
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
                        exit_button.PerformClick();
                        break;
                    case Keys.F1:
                        BuscarExistencia bs = new BuscarExistencia(webDM, agregarProd, sucursal, txtcodigo.Text);
                        bs.ShowDialog();
                        txtcodigo.Focus();
                        break;
                    case Keys.F2:
                        SeleccionarImagen(this, new EventArgs());
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

                            // Lista para almacenar los productos que ya están en la tabla
                            List<string> productosDuplicados = new List<string>();

                            // Agrega los productos a la lista existente.
                            foreach (Dictionary<string, object> producto in listaProductos)
                            {
                                string idProducto = producto["idproducto"].ToString();
                                int cantidad = Convert.ToInt32(producto["cantidad"]);
                                decimal precio = Convert.ToDecimal(producto["precio"]);

                                // Obtener detalles del producto.
                                Producto productoDetallado = localDM.GetProductByIdOrCode(idProducto);

                                // Verifica si el producto ya está en la lista
                                if (productos.Any(p => p.id == productoDetallado.id))
                                {
                                    // Si el producto ya está en la lista, lo agrega a la lista de productos duplicados
                                    productosDuplicados.Add(productoDetallado.nombre);
                                }
                                else
                                {
                                    // Si el producto no está en la lista, lo agrega a la lista de productos
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
                            }

                            // Refresca el DataGridView con la nueva información.
                            tablasource.DataSource = null; // Establecer a null antes de volver a asignar la lista.
                            tablasource.DataSource = productos;
                            tabla.DataSource = tablasource;

                            // Esto debería ser suficiente para forzar una actualización.
                            tabla.Refresh();

                            // Si hay productos duplicados, muestra un MessageBox con los nombres de los productos
                            if (productosDuplicados.Count > 0)
                            {
                                MessageBox.Show("Los siguientes productos ya existen en la tabla, por lo tanto no fueron agregados: " + string.Join(", ", productosDuplicados), "Advertencia");
                            }
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

        private void txtcodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                tabla.Focus();
            }
        }

        private void tabla_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                SeleccionarImagen(sender, e);
            }
            //if (e.KeyCode == Keys.Enter){
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up) {
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
                quitarProdButton_Click(sender, e);
            }
        }

        private void quitarProdButton_Click(object sender, EventArgs e)
        {
            if (tabla.SelectedRows.Count > 0)
            {
                int rowIndex = tabla.SelectedRows[0].Index;
                productos.RemoveAt(rowIndex);
                tablasource.ResetBindings(false);
            }
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Esta seguro que desea salir?", "Advertencia", MessageBoxButtons.YesNo))
            {
                this.Close();
            }
        }
    }
}