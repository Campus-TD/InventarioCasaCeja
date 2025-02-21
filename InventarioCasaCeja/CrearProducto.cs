using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Windows.Input;

namespace InventarioCasaCeja
{
    public partial class CrearProducto : Form
    {
        Dropcustom drop_categoria;
        Dropcustom drop_unidad;
        List<NuevoProducto> productos;
        BindingSource source;
        WebDataManager webDM;
        Dictionary<string, int> mapamedidas;
        Dictionary<string, int> mapacategorias;
        private int selectedIndex = -1; // Variable para almacenar el índice seleccionado

        public CrearProducto(WebDataManager webDataManager, Dictionary<string, int> mapamedidas, Dictionary<string, int> mapacategorias)
        {
            InitializeComponent();
            drop_categoria = new Dropcustom();
            drop_unidad = new Dropcustom();
            drop_categoria.setLabelText("SELECCIONAR CATEGORÍA");
            drop_unidad.setLabelText("SELECCIONAR MEDIDA");
            productos = new List<NuevoProducto>();
            source = new BindingSource();
            this.mapamedidas = mapamedidas;
            this.mapacategorias = mapacategorias;
            source.DataSource = productos;
            tabla.DataSource = source;
            webDM = webDataManager;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtbarras.Focus();
        }

        private void mostrar_categorias_Click(object sender, EventArgs e)
        {
            if (drop_categoria.IsDisposed)
            {
                drop_categoria = new Dropcustom();
                drop_categoria.setLabelText("SELECCIONAR CATEGORÍA");
            }
            string[] categorias = new List<string>(mapacategorias.Keys).ToArray();

            drop_categoria.setItems(categorias, actualizarCategoria);
            drop_categoria.Show();
            drop_categoria.Focus();
            drop_categoria.WindowState = FormWindowState.Normal;
        }

        private void mostrar_medidas_Click(object sender, EventArgs e)
        {
            if (drop_unidad.IsDisposed)
            {
                drop_unidad = new Dropcustom();
                drop_unidad.setLabelText("SELECCIONAR MEDIDA");
            }
            string[] unidades = new List<string>(mapamedidas.Keys).ToArray();
            drop_unidad.setItems(unidades, actualizarUnidad);
            drop_unidad.Show();
            drop_unidad.Focus();
            drop_unidad.WindowState = FormWindowState.Normal;
        }

        public void actualizarCategoria(string value)
        {
            txtcategoria.Text = value;
            txtmenudeo.Focus();
        }

        public void actualizarUnidad(string value)
        {
            txtunidad.Text = value;
            txtpresentacion.Focus();
        }

        private void txtunidad_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                mostrar_medidas_Click(sender, e);
            }
        }

        private void txtcategoria_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                mostrar_categorias_Click(sender, e);
            }
        }

        void limpiarCampos()
        {
            txtbarras.Clear();
            txtnombre.Clear();
            txtunidad.Clear();
            txtpresentacion.Clear();
            txtcategoria.Clear();
            txtmenudeo.Clear();
            txtmayoreo.Clear();
            txtespecial.Clear();
            txtvendedor.Clear();
            txtcantmay.Clear();
            MessageBox.Show("Producto Agregado ✔", "Éxito");
            txtbarras.Focus();
        }

        private async void enviar(List<NuevoProducto> data)
        {
            if (await webDM.SendProductosAsync(data))
            {
                productos.Clear();
                source.ResetBindings(false);
                MessageBox.Show("Los productos han sido enviados", "Completado");
                txtbarras.Focus();
            }
            else
            {
                MessageBox.Show("Hubo un problema al enviar los productos, se han almacenado localmente", "Advertencia");
            }
        }

        private void upload_Click(object sender, EventArgs e)
        {
            if (productos.Count > 0)
            {
                webDM.localDM.altaTemporal(productos);
                enviar(productos);
            }
            else
            {
                MessageBox.Show("No se han agregado productos", "Advertencia");
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            if (txtbarras.Text.Equals("") || txtnombre.Text.Equals("") || txtunidad.Text.Equals("") || txtcategoria.Text.Equals(""))
            {
                MessageBox.Show("Debes llenar todos los campos requeridos", "Advertencia");
            }
            else
            {
                // Verificar si el producto ya existe en la base de datos local en base al codigo de barras y obtiene el nombre si existe.
                (bool productoExiste, string nombreProducto) = webDM.localDM.ProductoExiste(txtbarras.Text);
                if (productoExiste)
                {
                    string nombre = txtbarras.Text;
                    MessageBox.Show($"El codigo de barras " + nombre + " ya existe en la base de datos y pertenece a: " + nombreProducto, "Advertencia");
                    txtbarras.Focus();
                }
                else
                {
                    // verifica si el codigo de barras ya esta en el datagridview.
                    string codigoBarras = txtbarras.Text;
                    bool codigoDuplicado = false;
                    foreach (NuevoProducto producto in productos)
                    {
                        if (producto.codigo.Equals(codigoBarras))
                        {
                            codigoDuplicado = true;
                            break;
                        }
                    }
                    if (codigoDuplicado)
                    {
                        MessageBox.Show("Ya se agrego un producto con el codigo de barras: " + codigoBarras, "Advertencia");
                        txtbarras.Focus();
                    }
                    else
                    {
                        // Agregar el producto a la lista si no existe
                        productos.Insert(0,
                    new NuevoProducto
                    {
                        codigo = txtbarras.Text,
                        nombre = txtnombre.Text,
                        presentacion = txtpresentacion.Text,
                        menudeo = double.Parse(txtmenudeo.Text.Equals("") || txtmenudeo.Text.Equals(".") ? "0" : txtmenudeo.Text),
                        mayoreo = double.Parse(txtmayoreo.Text.Equals("") || txtmayoreo.Text.Equals(".") ? "0" : txtmayoreo.Text),
                        cantidad_mayoreo = int.Parse(txtcantmay.Text.Equals("") ? "0" : txtcantmay.Text),
                        especial = double.Parse(txtespecial.Text.Equals("") || txtespecial.Text.Equals(".") ? "0" : txtespecial.Text),
                        vendedor = double.Parse(txtvendedor.Text.Equals("") || txtvendedor.Text.Equals(".") ? "0" : txtvendedor.Text),
                        imagen = "...",
                        medida_id = mapamedidas[txtunidad.Text],
                        categoria_id = mapacategorias[txtcategoria.Text]
                    });
                        source.ResetBindings(false);
                        limpiarCampos();
                    }
                }
            }
        }

        private void NoSpaces_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
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

        private void Bsalir_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Esta seguro que desea salir?", "Advertencia", MessageBoxButtons.YesNo))
            {
                this.Close();
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            Keys key = keyData & Keys.KeyCode;
            if ((keyData & Keys.Shift) == Keys.Shift)
            {
                if (key == Keys.F5)
                { 
                    Bsalir.PerformClick();
                    return true;
                }
            }
            if (Form.ModifierKeys == Keys.None)
            {
                switch (keyData)
                {
                    case Keys.Delete:
                        Beliminar_Click(this, new EventArgs());
                        break;
                    case Keys.Escape:
                        activarCampos();
                        break;
                    case Keys.F5:
                        add.PerformClick();
                        break;
                    case Keys.F6:
                        upload.PerformClick();
                        break;
                    default:
                        return base.ProcessDialogKey(keyData);
                }
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void tabla_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                Beliminar_Click(sender, e);
            }
            if (e.KeyCode == Keys.Escape)
            {
                activarCampos();
            }
        }

        // Método para obtener el índice de la fila seleccionada
        private void tabla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < productos.Count)
            {
                selectedIndex = e.RowIndex;
            }
        }

        private void Beliminar_Click(object sender, EventArgs e)
        {
            if (selectedIndex >= 0 && selectedIndex < productos.Count)
            {
                productos.RemoveAt(selectedIndex);
                source.ResetBindings(false);
                // Actualizar el índice seleccionado después de la eliminación
                if (productos.Count > 0)
                {
                    selectedIndex = Math.Min(selectedIndex, productos.Count - 1);
                    tabla.Rows[selectedIndex].Selected = true;
                }
                    selectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Seleccione un producto para eliminar", "Advertencia");
            }
        }

        private string ObtenerClavePorValor(Dictionary<string, int> diccionario, int valor)
        {
            foreach (var item in diccionario)
            {
                if (item.Value == valor)
                    return item.Key;
            }
            return null;
        }

        private void tabla_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (tabla.SelectedRows.Count > 0)
            {
                var selectedRow = tabla.SelectedRows[0];
                if (selectedRow != null)
                {
                    txtbarras.Text = selectedRow.Cells["codigo"].Value.ToString();
                    txtnombre.Text = selectedRow.Cells["nombre"].Value.ToString();
                    txtunidad.Text = ObtenerClavePorValor(mapamedidas, Convert.ToInt32(selectedRow.Cells["medida_id"].Value));
                    txtpresentacion.Text = selectedRow.Cells["presentacion"].Value.ToString();
                    txtcategoria.Text = ObtenerClavePorValor(mapacategorias, Convert.ToInt32(selectedRow.Cells["categoria_id"].Value));
                    txtmenudeo.Text = selectedRow.Cells["menudeo"].Value.ToString();
                    txtmayoreo.Text = selectedRow.Cells["mayoreo"].Value.ToString();
                    txtcantmay.Text = selectedRow.Cells["cantidad_mayoreo"].Value.ToString();
                    txtespecial.Text = selectedRow.Cells["especial"].Value.ToString();
                    txtvendedor.Text = selectedRow.Cells["vendedor"].Value.ToString();

                    // desactivar los campos.
                    txtbarras.Enabled = false;
                    txtnombre.Enabled = false;
                    txtunidad.Enabled = false;
                    txtpresentacion.Enabled = false;
                    txtcategoria.Enabled = false;
                    txtmenudeo.Enabled = false;
                    txtmayoreo.Enabled = false;
                    txtcantmay.Enabled = false;
                    txtespecial.Enabled = false;
                    txtvendedor.Enabled = false;
                    add.Enabled = false;
                    upload.Enabled = false;
                }
            }
        }

        private void activarCampos()
        {
            txtbarras.Text = "";
            txtnombre.Text = "";
            txtunidad.Text = "";
            txtpresentacion.Text = "";
            txtcategoria.Text = "";
            txtmenudeo.Text = "";
            txtmayoreo.Text = "";
            txtcantmay.Text = "";
            txtespecial.Text = "";
            txtvendedor.Text = "";

            txtbarras.Enabled = true;
            txtnombre.Enabled = true;
            txtunidad.Enabled = true;
            txtpresentacion.Enabled = true;
            txtcategoria.Enabled = true;
            txtmenudeo.Enabled = true;
            txtmayoreo.Enabled = true;
            txtcantmay.Enabled = true;
            txtespecial.Enabled = true;
            txtvendedor.Enabled = true;
            add.Enabled = true;
            upload.Enabled = true;
            txtbarras.Focus();
        }
    }
}