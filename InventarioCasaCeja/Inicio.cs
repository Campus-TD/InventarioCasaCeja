using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventarioCasaCeja.Properties;
using Firebase.Database;

namespace InventarioCasaCeja
{
    public partial class Inicio : Form
    {
        FirebaseClient firebase;
        Usuario usuarioActivo = null;
        WebDataManager webDM;
        LocaldataManager localDM;
        Dictionary<string, int> mapamedidas;
        Dictionary<string, int> mapacategorias;
        DataTable tablacatalogo;
        Vercatalogo vercatalago;
        Visor vercategorias;
        Visor vermedidas;
        Visor verproveedores;
        DataTable tablacategorias;
        DataTable tablamedidas;
        DataTable tablaproveedores;
        int idsucursal;        
        string fontName;
        int fontSize;
        int printerType;
        Sucursal sucursalActual;
        Dictionary<string, int> mapasucucrsales;
        public Inicio()
        {
            InitializeComponent();
            localDM = new LocaldataManager();
            webDM = new WebDataManager(localDM, refreshData);
            tablacatalogo = new DataTable();
            tablacategorias = new DataTable();
            tablamedidas = new DataTable();
            vercategorias = new Visor(0, webDM);
            vermedidas = new Visor(1, webDM);
            verproveedores = new Visor(8, webDM);
            vercatalago = new Vercatalogo(webDM);
            //vercatalago.setData(tablacatalogo, mapamedidas, mapacategorias);
        }
        private async void loadData()
        {
            //var runningProcessByName = Process.GetProcessesByName("CCSync");
            //if (runningProcessByName.Length == 0)
            //{
                this.Enabled = false;
                LoadWindow lw = new LoadWindow();
                lw.Show(this);
                if (await webDM.GetProductos())
                {
                    lw.setData(10, "Sincronizando datos desde el servidor...");
                    await webDM.GetSucursales();
                    lw.setData(30, "Sincronizando datos desde el servidor...");
                    await webDM.GetMedidas();
                    lw.setData(50, "Sincronizando datos desde el servidor...");
                    await webDM.GetCategorias();
                    lw.setData(60, "Sincronizando datos desde el servidor...");
                    await webDM.GetUsuarios();
                    lw.setData(70, "Sincronizando datos desde el servidor...");
                    await webDM.GetProveedores();
                    lw.setData(80, "Sincronizando datos desde el servidor...");
                    await webDM.GetEntradas();
                    lw.setData(90, "Sincronizando datos desde el servidor...");
                    await webDM.GetEntradaProducto();
                    lw.setData(100, "Sincronizando datos desde el servidor...");
                    await webDM.GetSalidas();
                    await webDM.GetSalidasGral();
                }
                else
                {
                    lw.Dispose();
                    MessageBox.Show("No se pudo conectar con el servidor, favor de intentar más tarde", "Advertencia");

                }
                //try
                //{
                //    string path = Path.Combine(Directory.GetCurrentDirectory(), @"CCSync/CCSync.exe");
                //    Process.Start(path);
                //}
                //catch (Exception e)
                //{

                //}
                lw.Dispose();
                this.Enabled = true;
                this.Focus();
            //}
            getConfig();
            refreshData(0);
            
            if (usuarioActivo == null)
            {
                pedirUsuario();
            }
        }
        void startFirebase()
        {
            firebase = new FirebaseClient("https://papeleria-8d415-default-rtdb.firebaseio.com/");

            var observable = firebase
          .Child("variables")
          .AsObservable<string>()
          .Subscribe(d => startCCSync());

        }
        async void startCCSync()
        {
            var runningProcessByName = Process.GetProcessesByName("CCSync");
            if (runningProcessByName.Length == 0)
            {
                try
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"CCSync/CCSync.exe");
                    var process = Process.Start(path);
                    process.WaitForExit(60000);
                }
                catch (Exception e)
                {

                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            loadData();
            //startFirebase();

        }
        void setUser(Usuario user)
        {
            this.usuarioActivo = user;
            webDM.activeUser = user;
        }
        void pedirUsuario()
        {
            UserLogin login = new UserLogin(localDM, setUser, false);
            DialogResult response = login.ShowDialog();
            if(response != DialogResult.Yes)
            {
                this.Close();
            }
        }
        void refreshData (int type)
        {
            switch (type)
            {
                case 1:
                    mapamedidas = localDM.getIndicesMedidas();
                    tablamedidas = localDM.getMedidas();
                    vermedidas.setData(tablamedidas);
                    vercatalago.setData(tablacatalogo, mapamedidas, mapacategorias);
                    break;
                case 2:
                    tablacategorias = localDM.getCategorias();
                    vercategorias.setData(tablacategorias);
                    mapacategorias = localDM.getIndicesCategorias();
                    vercatalago.setData(tablacatalogo, mapamedidas, mapacategorias);
                    break;

                case 3:
                    tablacatalogo = localDM.getProductos("0");
                    vercatalago.setData(tablacatalogo, mapamedidas, mapacategorias);
                    break;
                //case 4:
                //    tablasucursales = localDM.getSucursales();
                //    versucursales.setData(tablasucursales);
                //    break;
                //case 5:
                //    tablaventas = localDM.getVentas();
                //    verventas.setData(tablaventas);
                //    break;
                //case 6:
                //    tablaoperaciones = localDM.getOperaciones();
                //    veroperaciones.setData(tablaoperaciones);
                //    break;
                //case 7:
                //    tablausuarios = localDM.getUsuarios();
                //    verusuarios.setData(tablausuarios);
                //    break;
                case 8:
                    tablaproveedores = localDM.getProveedores();
                    verproveedores.setData(tablaproveedores);
                    break;
                default:
                    mapamedidas = localDM.getIndicesMedidas();
                    mapacategorias = localDM.getIndicesCategorias();
                    mapasucucrsales = localDM.getIndicesSucursales();
                    tablacatalogo = localDM.getProductos("0");
                    tablacategorias = localDM.getCategorias();
                    tablamedidas = localDM.getMedidas();
                    tablaproveedores = localDM.getProveedores();
                    verproveedores.setData(tablaproveedores);
                    vercatalago.setData(tablacatalogo, mapamedidas, mapacategorias);
                    vercategorias.setData(tablacategorias);
                    vermedidas.setData(tablamedidas);
                    break;

            }
        }

        private void catalogo_Click(object sender, EventArgs e)
        {
            if (vercatalago.IsDisposed)
            {
                vercatalago = new Vercatalogo(webDM);
                vercatalago.setData(tablacatalogo, mapamedidas, mapacategorias);
            }
            vercatalago.Show();
            vercatalago.Focus();
        }

        private void proveedores_Click(object sender, EventArgs e)
        {
            if (verproveedores.IsDisposed)
            {
                verproveedores = new Visor(8, webDM);
                verproveedores.setData(tablaproveedores);
            }
            verproveedores.Show();
            verproveedores.Focus();
        }

        private void categorias_Click(object sender, EventArgs e)
        {
            if (vercategorias.IsDisposed)
            {
                vercategorias = new Visor(0, webDM);
                vercategorias.setData(tablacategorias);
            }
            vercategorias.Show();
            vercategorias.Focus();
        }

        private void medidas_Click(object sender, EventArgs e)
        {
            if (vermedidas.IsDisposed)
            {
                vermedidas = new Visor(1, webDM);
                vermedidas.setData(tablamedidas);
            }
            vermedidas.Show();
            vermedidas.Focus();
        }
        void getConfig()
        {
               
            webDM.sucursal_id = idsucursal;
                
            printerType = int.Parse(Settings.Default["printertype"].ToString());
            
            idsucursal = int.Parse(Settings.Default["sucursalid"].ToString());
            localDM.setImpresora(Settings.Default["printername"].ToString());
            sucursalActual = localDM.getSucursal(idsucursal);
            fontName = Settings.Default["fontName"].ToString();
            fontSize = int.Parse(Settings.Default["fontSize"].ToString());
        }

        private void configuraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigWindow cw = new ConfigWindow(localDM);
            DialogResult result = cw.ShowDialog();
            if(result == DialogResult.Yes)
            {
                getConfig();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var runningProcessByName = Process.GetProcessesByName("CCSync");
            if (runningProcessByName.Length == 0)
            {
                CurrentData cd = new CurrentData();
                cd.sucursal = sucursalActual;
                cd.printerType = printerType;
                cd.webDM = webDM;
                cd.fontName = fontName;
                cd.fontSize = fontSize;
                cd.mapamedidasinv = new Dictionary<int, string>();
                cd.mapasucursales = mapasucucrsales;
                foreach (var x in mapamedidas)
                {
                    cd.mapamedidasinv[x.Value] = x.Key;
                }
                Salidas s = new Salidas(cd);
                s.ShowDialog();
            }
            else
            {
                MessageBox.Show("El asistente de actualización se encuentra sincronizando la base de datos, favor de intentar mas tarde", "Advertencia");
            }
            
        }

        private void limpiarBDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            localDM.ClearDatabase();
            webDM.resetDates();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CrearEntrada ce = new CrearEntrada(webDM, localDM, sucursalActual.id);
            ce.ShowDialog();
        }

        private void sincronizarBaseDeDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var runningProcessByName = Process.GetProcessesByName("CCSync");
            if (runningProcessByName.Length == 0)
            {
                //DialogResult response = MessageBox.Show("¿Desea eliminar la informacion del catálogo antes de sincronizar?\n\nEsta acción puede tomar unos minutos.", "Advertencia", MessageBoxButtons.YesNoCancel);
                DialogResult response = MessageBox.Show("Al sincronizar, reemplazará la informacion local con la mas reciente en el servidor y se eliminara la informacion no enviada. \n\nEsta acción puede tomar unos minutos. ¿Desea continuar?", "Advertencia", MessageBoxButtons.YesNo);
                if (response == DialogResult.Yes)
                {
                    localDM.clearTabble("productos");
                    webDM.resetDates();
                }

                if (response != DialogResult.No)
                {
                    loadData();
                }
            }else
            {
                MessageBox.Show("El asistente de actualización ya se encuentra sincronizando la base de datos, favor de intentar mas tarde", "Advertencia");
            }
        }
        protected override void Dispose(bool disposing)
        {
            localDM.endDatabase();
            if (firebase != null)
                firebase.Dispose();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            
            base.Dispose(disposing);            
        }

        private void historialDeEntradasYSalidasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistEntradasSalidas hes = new HistEntradasSalidas(idsucursal);
            hes.ShowDialog();
        }
    }
}
