using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace InventarioCasaCeja
{
    public partial class Vercatalogo : Form
    {
        public bool active;
        Dictionary<string, int> mapamedidas;
        Dictionary<string, int> mapacategorias;
        WebDataManager webDM;
        Dictionary<string, string> urls;
        int offset;
        int currentPage;
        int maxPages;
        int rowsPerPage;
        int currentcat;
        int currentmed;
        public Vercatalogo(WebDataManager webDataManager)
        {
            InitializeComponent();
            this.webDM = webDataManager;
            active = false;
            mapacategorias = new Dictionary<string, int>();
            mapamedidas = new Dictionary<string, int>();
            offset = 0;
            currentcat = 0;
            currentmed = 0;
            currentPage = 1;
            maxPages = 1;
            rowsPerPage = 21;
            urls = new Dictionary<string, string>();
            this.catalogo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.catalogo_KeyDown);
            this.txtbuscar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbuscar_KeyDown);

        }

        public void setData(DataTable tablacatalogo, Dictionary<string, int> mapamedidas, Dictionary<string, int> mapacategorias)
        {
            this.mapamedidas = mapamedidas;
            this.mapacategorias = mapacategorias;
            
            boxcategoria.Items.Clear();
            boxmedida.Items.Clear();
            boxcategoria.Items.Add("Seleccionar categoría");
            boxmedida.Items.Add("Seleccionar medida");
            boxcategoria.Items.AddRange(mapacategorias.Keys.ToArray());
            boxmedida.Items.AddRange(mapamedidas.Keys.ToArray());
            boxcategoria.SelectedIndex = currentcat;
            boxmedida.SelectedIndex = currentmed;
            int rowCount = webDM.localDM.getTableRowCount("productos");
            calculateMaxPages(rowCount);
            if (active)
            {
            catalogo.Invoke(new Action(() => {
                
                loadData();
            }));
            }
            else
            {
                catalogo.DataSource = tablacatalogo;
                
            }            
        }

        private void Vercatalago_Load(object sender, EventArgs e)
        {
            active = true;
            //generateImgRow();
        }

        private void Vercatalago_FormClosed(object sender, FormClosedEventArgs e)
        {
            active = false;
        }

        private void catalogo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            modify();
        }
        private void modify()
        {
            int rowIndex = catalogo.SelectedCells[0].RowIndex;
            DataGridViewRow row = catalogo.Rows[rowIndex];
            ModificarProducto mp = new ModificarProducto(webDM, mapamedidas, mapacategorias);
            mp.setData(row);
            mp.Show();
        }

        private void catalogo_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Enter:
                    //modify();
                    break;
                case Keys.F1:
                    txtbuscar.Focus();
                    break;
                case Keys.F2:
                    boxcategoria.DroppedDown = true;
                    boxcategoria.Focus();
                    break;
                case Keys.F3:
                    boxmedida.DroppedDown = true;
                    boxmedida.Focus();
                    break;
                case Keys.F5:
                    alta();
                    break;
                case Keys.E:
                    if (e.Modifiers == Keys.Alt)
                        verExistencia();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void altaDeProducto_Click(object sender, EventArgs e)
        {
            alta();
        }
        private void alta()
        {
            CrearProducto crear = new CrearProducto(webDM, mapamedidas, mapacategorias);
            crear.Show();
        }

        void loadData()
        {
            DataTable tablacatalogo;
            string arg = "";
            int rowCount;
            if (boxcategoria.SelectedIndex > 0)
            {
                arg += "AND productos.categoria_id = " + mapacategorias[boxcategoria.SelectedItem.ToString()]+" ";
            }
            if (boxmedida.SelectedIndex > 0)
            {
                arg += "AND productos.medida_id = " + mapamedidas[boxmedida.SelectedItem.ToString()]+ " ";
            }
            if (txtbuscar.Text.Equals("")){
                rowCount = webDM.localDM.getProductosRowCount(arg);
                calculateMaxPages(rowCount);
                tablacatalogo = webDM.localDM.getProductos(offset.ToString(), arg);
                
            }
            else
            {
                rowCount = webDM.localDM.getProductosRowCount(arg, txtbuscar.Text);
                calculateMaxPages(rowCount);
                tablacatalogo = webDM.localDM.getProductos(offset.ToString(), arg, txtbuscar.Text);
                
            }            
            catalogo.DataSource = tablacatalogo;
            //generateImgRow();
        }
        /*
        private void generateImgRow()
        {
            urls.Clear();
            for (int i = 0; i < catalogo.Rows.Count; i++)
            {
                urls[catalogo[0, i].Value.ToString()] = catalogo[12, i].Value.ToString();
                catalogo[12, i].Value = "Ver imagen";
            }
            DataGridViewCellStyle cell_style = new DataGridViewCellStyle();
            cell_style.ForeColor = Color.Blue;
            cell_style.Font= new Font("Segoe UI", 12, FontStyle.Underline);
            catalogo.Columns[12].DefaultCellStyle = cell_style;
        }
        
        private void catalogo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == catalogo.Columns["IMAGEN"].Index)
            {
                //Do something with your button.
                MessageBox.Show("Imagen no encontrada", "Advertencia");

            }
        }
        */
        private void calculateMaxPages( int rowCount)
        {
            maxPages = ((rowCount % rowsPerPage) == 0) ? rowCount / rowsPerPage : rowCount / rowsPerPage + 1;
            if (maxPages == 0)
                maxPages++;
            if (maxPages < currentPage)
            {
                currentPage = maxPages;
                offset = (currentPage - 1) * rowsPerPage;
            }
            pageLabel.Text = "Página " + currentPage + "/" + maxPages;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                offset -= rowsPerPage;
                currentPage--;
                loadData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           if (currentPage < maxPages)
            {
                offset += rowsPerPage;
                currentPage++;
                loadData();
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
                    //case Keys.Enter:
                    //    if(button1.Focused|| button2.Focused || button3.Focused || button4.Focused || catalogo.Focused)
                    //        return base.ProcessDialogKey(keyData);
                    //    SendKeys.Send("{TAB}");
                    //    break;
                    case Keys.F1:
                        txtbuscar.Focus();
                        break;
                    case Keys.F2:
                        boxcategoria.DroppedDown = true;
                        boxcategoria.Focus();
                        break;
                    case Keys.F3:
                        boxmedida.DroppedDown = true;
                        boxmedida.Focus();
                        break;
                    case Keys.F5:
                        alta();
                        break;
                    case Keys.F6:
                        modify();
                        break;
                    case Keys.Down:
                        catalogo.Focus();
                        SendKeys.Send("{DOWN}");
                        break;
                    case Keys.Up:                        
                        catalogo.Focus();
                        SendKeys.Send("{UP}");
                        break;
                    default:
                        return base.ProcessDialogKey(keyData);
                }
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void txtbuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                catalogo.Focus();
                SendKeys.Send("{DOWN}");
            }

            if (e.KeyData == Keys.Up)
            {                
                catalogo.Focus();
                SendKeys.Send("{UP}");
            }
            if(e.KeyData == Keys.Enter)
            {
                modify();
            }
            if (e.Modifiers == Keys.Alt && e.KeyData == Keys.E)
            {
                verExistencia();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void boxcategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentcat = boxcategoria.SelectedIndex;
            loadData();

        }

        private void txtbuscar_TextChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void boxmedida_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentmed = boxmedida.SelectedIndex;
            loadData();
        }

        private void Bsalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void catalogo_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            catalogo.Cursor = Cursors.Default;
        }

        private void Bmodificar_Click(object sender, EventArgs e)
        {
            modify();
        }

        private void BaltaProducto_Click(object sender, EventArgs e)
        {
            alta();
        }

        private void details_Click(object sender, EventArgs e)
        {
            if (catalogo.SelectedRows.Count > 0)
            {
                verExistencia();
            }
            else
            {
                MessageBox.Show("Favor de seleccionar un producto", "Advertencia");
            }
        }
        private async void verExistencia()
        {           
            string idprod = catalogo.SelectedRows[0].Cells[0].Value.ToString();
            string titulo = "Producto: " + catalogo.SelectedRows[0].Cells[2].Value.ToString();
            List<ProductoExistencia> prodex = await webDM.getProductoExistencia(idprod);
            if (prodex != null)
            {
                //foreach(ProductoExistencia p in prodex)
                //{
                //    if (p.RAZON_SOCIAL.Equals(sucursal))
                //    {
                //        prodex.Remove(p);
                //        prodex.Insert(0, p);
                //        break;
                //    }
                //}
                if (prodex.Count == 0)
                {
                    MessageBox.Show("No se encontraron existencias de este producto, favor de intentar más tarde", "Advertencia");
                }
                else
                {                    
                    VerExistencia ve = new VerExistencia(prodex, titulo);
                    ve.ShowDialog();
                }

            }
            else MessageBox.Show("No se pudo conectar con el servidor, favor de intentar más tarde", "Advertencia");
        }

    }
}
