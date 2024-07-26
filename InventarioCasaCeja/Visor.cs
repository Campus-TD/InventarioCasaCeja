using System;
using System.Data;
using System.Windows.Forms;

namespace InventarioCasaCeja
{
    public partial class Visor : Form
    {
        bool active;
        WebDataManager webDM;
        int type;

        public Visor(int Type, WebDataManager webDataManager)
        {
            InitializeComponent();
            this.webDM = webDataManager;
            this.type = Type;
            active = false;
            switch (type)
            {
                case 0:
                    this.Text = "Categorías";
                    crear.Text = "NUEVA CATEGORÍA (F2)";
                    break;
                case 1:
                    this.Text = "Unidades de medida";
                    crear.Text = "NUEVA MEDIDA (F2)";
                    break;
                case 2:
                    this.Text = "Operaciones";
                    crear.Visible = false;
                    break;
                case 3:
                    this.Text = "Usuarios";
                    crear.Text = "NUEVO USUARIO (F2)";
                    break;
                case 4:
                    this.Text = "Sucursales";
                    crear.Text = "NUEVA SUCURSAL (F2)";
                    break;
                case 5:
                    this.Text = "Historial de ventas";
                    crear.Visible = false;
                    break;
                case 6:
                    this.Text = "Productos vendidos";
                    crear.Visible = false;
                    break;
                case 7:
                    this.Text = "Existencias disponibles";
                    crear.Visible = false;
                    break;
                case 8:
                    this.Text = "Proveedores registrados";
                    crear.Text = "NUEVO PROVEEDOR (F2)";
                    break;
            }
        }
        public void setData(DataTable Data)
        {
            if (active)
            {
                tabla.Invoke(new Action(() => { tabla.DataSource = Data; tabla.Refresh(); }));
            }
            else
            {
                tabla.DataSource = Data;
            }

        }
        public void loadData()
        {
            DataTable data = new DataTable();
            if (txtbuscar.Text.Equals(""))
            {
                switch (type)
                {
                    case 0:
                        this.Text = "Categorías";
                        data = webDM.localDM.getCategorias();
                        break;
                    case 1:
                        this.Text = "Unidades de medida";
                        data = webDM.localDM.getMedidas();
                        break;
                    case 2:
                        this.Text = "Operaciones";
                        break;
                    case 3:
                        this.Text = "Usuarios";
                        data = webDM.localDM.getUsuarios();
                        break;
                    case 4:
                        this.Text = "Sucursales";
                        data = webDM.localDM.getSucursales();
                        break;
                    case 5:
                        this.Text = "Historial de ventas";
                        data = webDM.localDM.getVentas();
                        break;
                    //case 6:
                    //    this.Text = "Productos vendidos";
                    //    break;
                    //case 7:
                    //    this.Text = "Existencias disponibles";
                    //    break;
                    case 8:
                        data = webDM.localDM.getProveedores();
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case 0:
                        this.Text = "Categorías";
                        data = webDM.localDM.getCategorias(txtbuscar.Text);
                        break;
                    case 1:
                        this.Text = "Unidades de medida";
                        data = webDM.localDM.getMedidas(txtbuscar.Text);
                        break;
                    case 2:
                        this.Text = "Operaciones";
                        break;
                    case 3:
                        this.Text = "Usuarios";
                        data = webDM.localDM.getUsuarios(txtbuscar.Text);
                        break;
                    case 4:
                        this.Text = "Sucursales";
                        data = webDM.localDM.getSucursales(txtbuscar.Text);
                        break;
                    case 5:
                        this.Text = "Historial de ventas";
                        data = webDM.localDM.getVentas(txtbuscar.Text);
                        break;
                    //case 6:
                    //    this.Text = "Productos vendidos";
                    //    break;
                    //case 7:
                    //    this.Text = "Existencias disponibles";
                    //    break;
                    case 8:
                        data = webDM.localDM.getProveedores(txtbuscar.Text);
                        break;
                }
            }
            tabla.DataSource = data;
        }
        public void setConfig(string Title)
        {
            this.Text = Title;
        }

        private void Visor_Load(object sender, EventArgs e)
        {
            active = true;
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void crearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (type < 2)
            {

                CatMedCreator createCM = new CatMedCreator(type, webDM);

                createCM.Show();
            }
            //else if (type == 3)
            //{

            //    CrearUsuario createUsr = new CrearUsuario(webDM);

            //    createUsr.Show();
            //}
            //else if (type == 4)
            //{
            //    CrearSucursal cs = new CrearSucursal(webDM);
            //    cs.Show();
            //}
            else if (type == 8)
            {
                CrearProveedor cs = new CrearProveedor(webDM);
                cs.Show();
            }

        }

        private void Visor_FormClosed(object sender, FormClosedEventArgs e)
        {
            active = false;
        }
        private void modify()
        {
            int rowIndex = tabla.SelectedCells[0].RowIndex;
            DataGridViewRow row = tabla.Rows[rowIndex];
            if (type < 2)
            {
                
                ModCatMed mcm = new ModCatMed(type, row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), webDM);
                mcm.Show();
            //}else if (type == 3)
            //{
            //    ModificarUsuario mu = new ModificarUsuario(webDM);
            //    mu.setData(row);
            //    mu.Show();
            //}else if (type == 4)
            //{
            //    ModificarSucursal ms = new ModificarSucursal(webDM);
            //    ms.setData(row);
            //    ms.Show();
            }
            else if (type == 5)
            {
                Visor prods = new Visor(6, webDM);
                prods.setData(webDM.localDM.getProductoVentas(row.Cells[0].Value.ToString()));
                prods.Show();
            }
            else if (type == 8)
            {
                ModificarProveedor ms = new ModificarProveedor(webDM);
                ms.setData(row);
                ms.Show();
            }


        }

        private void tabla_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                crear.PerformClick();
            }
            if (e.KeyCode == Keys.F3)
            {
                modify();
            }
        }

        private void tabla_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            modify();
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
                    //    if (crear.Focused || salir.Focused || tabla.Focused)
                    //        return base.ProcessDialogKey(keyData);
                    //    SendKeys.Send("{TAB}");
                    //    break;
                    case Keys.F1:
                        txtbuscar.Focus();
                        break;
                    case Keys.F2:
                        crear.PerformClick();
                        break;
                    case Keys.F3:
                        modify();
                        break;
                    case Keys.Down:
                        tabla.Focus();
                        SendKeys.Send("{DOWN}");
                        break;
                    case Keys.Up:                        
                        tabla.Focus();
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
                tabla.Focus();
                SendKeys.Send("{DOWN}");
            }

            if (e.KeyData == Keys.Up)
            {
                SendKeys.Send("{UP}");
                tabla.Focus();
            }
        }

        private void txtbuscar_TextChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void Bmodificar_Click(object sender, EventArgs e)
        {
            modify();
        }
    }

}
