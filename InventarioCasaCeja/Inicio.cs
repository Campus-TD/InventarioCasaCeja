﻿using System;
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
            sucursalActual = new Sucursal();
            //vercatalago.setData(tablacatalogo, mapamedidas, mapacategorias);
        }

        private async void loadData()
        {
            this.Enabled = false;
            LoadWindow lw = new LoadWindow();
            lw.Show(this);

            try
            {
                getConfig();
                int idsucursalParaEyS;
                int sucursalIdSettingParaEyS = Settings.Default.sucursalid; // Obtener el valor int directamente

                if (sucursalIdSettingParaEyS == 0) // Cambiar 0 al valor predeterminado correcto si es diferente
                {
                    idsucursalParaEyS = 1; // Usar sucursal 1 si es la primera vez
                }
                else
                {
                    idsucursalParaEyS = sucursalIdSettingParaEyS; // Usar la configurada
                }
                // Lógica para bases precargadas
                if (localDM.IsCatalogPreloaded)
                {
                    Debug.WriteLine("Base de datos precargada detectada. Sincronizando datos esenciales primero...");

                    // 1. Sincronizar datos básicos
                    lw.setData(10, "Sincronizando datos básicos...");
                    await webDM.GetSucursales();
                    await webDM.GetUsuarios();

                    // 2. Actualizar catálogo con cambios recientes
                    lw.setData(30, "Actualizando catálogo...");
                    await webDM.GetProductos();

                    // 3. Sincronizar datos específicos de inventario
                    lw.setData(50, "Sincronizando proveedores...");
                    await webDM.GetProveedores();

                    lw.setData(70, "Sincronizando entradas...");
                    await webDM.GetEntradas(idsucursalParaEyS);

                    lw.setData(80, "Sincronizando relación productos-entradas...");
                    await webDM.GetEntradaProducto();

                    lw.setData(90, "Sincronizando salidas...");
                    await webDM.GetSalidas(idsucursalParaEyS);
                    await webDM.GetSalidasGral(idsucursalParaEyS);
                }
                else
                {
                    Debug.WriteLine("No se detectó base precargada. Sincronización completa desde servidor...");

                    // Flujo completo para instalación nueva
                    if (await webDM.GetProductos())
                    {
                        lw.setData(10, "Sincronizando datos básicos...");
                        await webDM.GetSucursales();

                        lw.setData(30, "Obteniendo unidades de medida...");
                        await webDM.GetMedidas();

                        lw.setData(50, "Cargando categorías...");
                        await webDM.GetCategorias();

                        lw.setData(70, "Sincronizando usuarios...");
                        await webDM.GetUsuarios();

                        lw.setData(80, "Actualizando proveedores...");
                        await webDM.GetProveedores();

                        lw.setData(90, "Cargando entradas...");
                        await webDM.GetEntradas(idsucursalParaEyS);

                        lw.setData(95, "Relacionando productos con entradas...");
                        await webDM.GetEntradaProducto();

                        lw.setData(100, "Sincronizando movimientos...");
                        await webDM.GetSalidas(idsucursalParaEyS);
                        await webDM.GetSalidasGral(idsucursalParaEyS);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en sincronización: {ex.Message}");
                MessageBox.Show("Error al sincronizar datos. Verifique su conexión.");
            }
            finally
            {
                lw.Dispose();
                this.Enabled = true;
                this.Focus();
            }

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
            Console.WriteLine(webDM.sucursal_id);
            printerType = int.Parse(Settings.Default["printertype"].ToString());

            int sucursalIdSetting = Settings.Default.sucursalid; // Obtener el valor int directamente

            // Verificar si la configuración de sucursalid es el valor predeterminado (asumimos que es 0)
            if (sucursalIdSetting == 0) // Cambia 0 si tu valor predeterminado es otro
            {
                // Si es la primera vez o el valor predeterminado, establecer idsucursal a 1
                idsucursal = 1;
                Settings.Default["sucursalid"] = 1; // Guardar 1 como int (o como string "1" si prefieres, ¡lee la nota abajo!)
                Settings.Default.Save(); // Guardar la configuración
            }
            else
            {
                // Si ya existe una configuración válida, cargarla normalmente
                idsucursal = sucursalIdSetting;
            }

            // **Eliminar la línea `idsucursal = 1;` que estaba sobreescribiendo el valor**
            // idsucursal = 1;

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

            sucursalActual = localDM.getSucursal(Settings.Default.sucursalid);
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

                    sucursalActual = localDM.getSucursal(Settings.Default.sucursalid);
                    localDM.ClearDatabase();
                    webDM.resetDates();
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
