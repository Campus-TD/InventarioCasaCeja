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

                // ★ NUEVA LÓGICA: Sincronizar sucursales PRIMERO y obtener la primera disponible
                int idsucursalParaEyS = await ObtenerSucursalParaInventario(lw);

                Console.WriteLine($"★ Usando sucursal para entradas/salidas: {idsucursalParaEyS}");

                // Lógica para bases precargadas
                if (localDM.IsCatalogPreloaded)
                {
                    Console.WriteLine("Base de datos precargada detectada. Sincronizando datos esenciales primero...");

                    // 1. Sincronizar datos básicos
                    lw.setData(10, "Sincronizando datos básicos...");
                    await webDM.GetSucursales();
                    await webDM.GetUsuarios();

                    // 2. AGREGAR: Sincronizar categorías y medidas (CRÍTICO)
                    lw.setData(20, "Sincronizando categorías...");
                    await webDM.GetCategorias();

                    lw.setData(25, "Sincronizando medidas...");
                    await webDM.GetMedidas();

                    // 3. Actualizar catálogo con cambios recientes
                    lw.setData(30, "Actualizando catálogo...");
                    await webDM.GetProductos();

                    // 4. Sincronizar datos específicos de inventario
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
                        lw.setData(20, "Obteniendo unidades de medida...");
                        await webDM.GetMedidas();

                        lw.setData(30, "Cargando categorías...");
                        await webDM.GetCategorias();

                        lw.setData(50, "Sincronizando usuarios...");
                        await webDM.GetUsuarios();

                        lw.setData(60, "Actualizando proveedores...");
                        await webDM.GetProveedores();

                        lw.setData(70, "Cargando entradas...");
                        await webDM.GetEntradas(idsucursalParaEyS);

                        lw.setData(80, "Relacionando productos con entradas...");
                        await webDM.GetEntradaProducto();

                        lw.setData(90, "Sincronizando movimientos...");
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
                lw.Close();
                this.Enabled = true;

                Console.WriteLine("★ === DIAGNÓSTICO POST-LOADDATA ===");

                // Verificar datos en base de datos
                try
                {
                    var countCategorias = localDM.getTableRowCount("categorias");
                    var countMedidas = localDM.getTableRowCount("medidas");
                    var countProductos = localDM.getTableRowCount("productos");

                    Console.WriteLine($"★ Registros en BD - Categorías: {countCategorias}, Medidas: {countMedidas}, Productos: {countProductos}");

                    // Probar cargar datos directamente
                    var testCategorias = localDM.getCategorias();
                    var testMedidas = localDM.getMedidas();
                    var testProductos = localDM.getProductos("0");

                    Console.WriteLine($"★ Test getData - Categorías: {testCategorias.Rows.Count}, Medidas: {testMedidas.Rows.Count}, Productos: {testProductos.Rows.Count}");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"★ Error en diagnóstico: {ex.Message}");
                }

                Console.WriteLine("★ === FIN DIAGNÓSTICO ===");

                // Llamar refreshData para inicializar todo
                refreshData(0);
            }
        }

        // ====================================================================
        // AGREGAR NUEVO MÉTODO: ObtenerSucursalParaInventario
        // ====================================================================

        private async Task<int> ObtenerSucursalParaInventario(LoadWindow lw)
        {
            try
            {
                lw.setData(10, "Sincronizando sucursales...");

                // 1. Sincronizar sucursales desde el servidor
                bool sucursalesOk = await webDM.GetSucursales();

                if (!sucursalesOk)
                {
                    Console.WriteLine("★ WARNING: No se pudieron sincronizar las sucursales desde el servidor");
                    // Fallback: usar configuración por defecto
                    return Settings.Default.sucursalid > 0 ? Settings.Default.sucursalid : 1;
                }

                // 2. Obtener sucursales desde la base de datos local (ya sincronizadas)
                DataTable sucursales = localDM.getSucursales();

                if (sucursales == null || sucursales.Rows.Count == 0)
                {
                    Console.WriteLine("★ WARNING: No hay sucursales disponibles en la base de datos local");
                    return 1; // Fallback
                }

                Console.WriteLine($"★ Se encontraron {sucursales.Rows.Count} sucursales disponibles:");

                // 3. Mostrar todas las sucursales disponibles
                foreach (DataRow row in sucursales.Rows)
                {
                    int id = Convert.ToInt32(row["id"]);
                    string razonSocial = row["razon_social"].ToString();
                    Console.WriteLine($"   - ID: {id}, Nombre: {razonSocial}");
                }

                // 4. Determinar qué sucursal usar
                int sucursalSeleccionada;

                // Si ya hay una configuración guardada y existe en las sucursales disponibles
                int sucursalConfig = Settings.Default.sucursalid;
                bool existeSucursalConfig = sucursales.AsEnumerable()
                    .Any(row => Convert.ToInt32(row["id"]) == sucursalConfig);

                if (sucursalConfig > 0 && existeSucursalConfig)
                {
                    sucursalSeleccionada = sucursalConfig;
                    Console.WriteLine($"★ Usando sucursal configurada: {sucursalSeleccionada}");
                }
                else
                {
                    // Tomar la primera sucursal disponible
                    sucursalSeleccionada = Convert.ToInt32(sucursales.Rows[0]["id"]);
                    string nombrePrimera = sucursales.Rows[0]["razon_social"].ToString();

                    Console.WriteLine($"★ Usando primera sucursal disponible: {sucursalSeleccionada} ({nombrePrimera})");

                    // Guardar esta sucursal como configuración por defecto
                    Settings.Default.sucursalid = sucursalSeleccionada;
                    Settings.Default.Save();

                    Console.WriteLine($"★ Sucursal {sucursalSeleccionada} guardada como configuración por defecto");
                }

                // 5. Actualizar variables globales
                idsucursal = sucursalSeleccionada;

                // 6. Obtener información completa de la sucursal seleccionada
                sucursalActual = localDM.getSucursal(sucursalSeleccionada);
                if (sucursalActual != null)
                {
                    Console.WriteLine($"★ Sucursal actual: {sucursalActual.razon_social}");
                }

                return sucursalSeleccionada;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"★ ERROR en ObtenerSucursalParaInventario: {ex.Message}");

                // En caso de error, usar fallback
                int fallback = Settings.Default.sucursalid > 0 ? Settings.Default.sucursalid : 1;
                Console.WriteLine($"★ Usando sucursal fallback: {fallback}");
                return fallback;
            }
        }
        
        private void VerificarDatosSincronizados()
        {
            try
            {
                var sucursalActual = idsucursal;

                // Obtener conteos usando los métodos existentes
                var totalEntradas = localDM.getEntradasCountPorSucursal(sucursalActual);
                var totalSalidas = localDM.getSalidasCountPorSucursal(sucursalActual);

                Console.WriteLine($"★ VERIFICACIÓN POST-SINCRONIZACIÓN:");
                Console.WriteLine($"   - Sucursal actual: {sucursalActual}");
                Console.WriteLine($"   - Entradas sincronizadas: {totalEntradas}");
                Console.WriteLine($"   - Salidas sincronizadas: {totalSalidas}");

                if (totalEntradas == 0 && totalSalidas == 0)
                {
                    Console.WriteLine($"★ INFO: No hay entradas/salidas para sucursal {sucursalActual}");
                    Console.WriteLine($"★ Esto es normal si es una sucursal nueva o sin movimientos");
                }
                else
                {
                    Console.WriteLine($"★ SUCCESS: Datos de entradas/salidas sincronizados correctamente");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"★ Error en verificación: {ex.Message}");
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
        private async void Form1_Load(object sender, EventArgs e)
        {
            await webDM.GetUsuarios();
            // ★ RESTAURAR: Pedir login ANTES de cargar datos
            if (usuarioActivo == null)
            {
                pedirUsuario();
            }

            // Solo cargar datos si hay usuario válido
            if (usuarioActivo != null)
            {
                loadData();
            }
            else
            {
                // Si no hay usuario, cerrar la aplicación
                MessageBox.Show("Se requiere un usuario válido para usar el sistema.", "Login Requerido");
                this.Close();
            }
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

        void refreshData(int type)
        {
            switch (type)
            {
                case 0: // NUEVO CASO: Inicialización completa después de sincronización
                    Console.WriteLine($"★ Caso 0: Inicialización completa de datos");

                    // Cargar mapas de índices
                    mapamedidas = localDM.getIndicesMedidas();
                    mapacategorias = localDM.getIndicesCategorias();

                    // Cargar datos para las tablas principales
                    tablacatalogo = localDM.getProductos("0");
                    tablacategorias = localDM.getCategorias();
                    tablamedidas = localDM.getMedidas();

                    Console.WriteLine($"★ Datos cargados - Productos: {tablacatalogo.Rows.Count}, Categorías: {tablacategorias.Rows.Count}, Medidas: {tablamedidas.Rows.Count}");

                    // Actualizar visores si están abiertos y activos
                    if (vercategorias != null && vercategorias.active)
                    {
                        vercategorias.setData(tablacategorias);
                    }

                    if (vermedidas != null && vermedidas.active)
                    {
                        vermedidas.setData(tablamedidas);
                    }

                    if (vercatalago != null && vercatalago.active)
                    {
                        vercatalago.setData(tablacatalogo, mapamedidas, mapacategorias);
                    }
                    break;

                case 1: // MANTENER código existente
                    mapamedidas = localDM.getIndicesMedidas();
                    tablamedidas = localDM.getMedidas();
                    vermedidas.setData(tablamedidas);
                    vercatalago.setData(tablacatalogo, mapamedidas, mapacategorias);
                    break;

                case 2: // MANTENER código existente
                    tablacategorias = localDM.getCategorias();
                    vercategorias.setData(tablacategorias);
                    mapacategorias = localDM.getIndicesCategorias();
                    vercatalago.setData(tablacatalogo, mapamedidas, mapacategorias);
                    break;

                case 3: // MANTENER código existente
                    tablacatalogo = localDM.getProductos("0");
                    vercatalago.setData(tablacatalogo, mapamedidas, mapacategorias);
                    break;

                case 8: // Proveedores
                    Console.WriteLine("★ Caso 8: Actualizando proveedores");
                    tablaproveedores = localDM.getProveedores();

                    if (verproveedores != null && verproveedores.active)
                    {
                        verproveedores.setData(tablaproveedores);
                        Console.WriteLine($"★ Proveedores actualizados en visor: {tablaproveedores.Rows.Count} registros");
                    }
                    break;

                default:
                    Console.WriteLine($"★ Caso default: {type} - No hay acción específica definida");
                    break;

            }
        }


        private void categorias_Click(object sender, EventArgs e)
        {
            Console.WriteLine("★ Abriendo vista de categorías");

            if (vercategorias.IsDisposed)
            {
                vercategorias = new Visor(0, webDM);
            }

            // ★ ASEGURAR que los datos estén cargados
            if (tablacategorias == null || tablacategorias.Rows.Count == 0)
            {
                Console.WriteLine("★ Cargando categorías porque están vacías");
                tablacategorias = localDM.getCategorias();
            }

            Console.WriteLine($"★ Enviando {tablacategorias.Rows.Count} categorías al visor");
            vercategorias.setData(tablacategorias);
            vercategorias.Show();
            vercategorias.Focus();
        }

        // MODIFICAR medidas_Click en Inicio.cs
        private void medidas_Click(object sender, EventArgs e)
        {
            Console.WriteLine("★ Abriendo vista de medidas");

            if (vermedidas.IsDisposed)
            {
                vermedidas = new Visor(1, webDM);
            }

            // ★ ASEGURAR que los datos estén cargados
            if (tablamedidas == null || tablamedidas.Rows.Count == 0)
            {
                Console.WriteLine("★ Cargando medidas porque están vacías");
                tablamedidas = localDM.getMedidas();
            }

            Console.WriteLine($"★ Enviando {tablamedidas.Rows.Count} medidas al visor");
            vermedidas.setData(tablamedidas);
            vermedidas.Show();
            vermedidas.Focus();
        }

        // MODIFICAR catalogo_Click en Inicio.cs
        private void catalogo_Click(object sender, EventArgs e)
        {
            Console.WriteLine("★ Abriendo vista de catálogo");

            if (vercatalago.IsDisposed)
            {
                vercatalago = new Vercatalogo(webDM);
            }

            // ★ ASEGURAR que los datos estén cargados
            if (tablacatalogo == null || tablacatalogo.Rows.Count == 0)
            {
                Console.WriteLine("★ Cargando catálogo porque está vacío");
                tablacatalogo = localDM.getProductos("0");
            }

            if (mapamedidas == null)
            {
                Console.WriteLine("★ Cargando mapa de medidas");
                mapamedidas = localDM.getIndicesMedidas();
            }

            if (mapacategorias == null)
            {
                Console.WriteLine("★ Cargando mapa de categorías");
                mapacategorias = localDM.getIndicesCategorias();
            }

            Console.WriteLine($"★ Enviando catálogo con {tablacatalogo.Rows.Count} productos al visor");
            vercatalago.setData(tablacatalogo, mapamedidas, mapacategorias);
            vercatalago.Show();
            vercatalago.Focus();
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

                if (mapasucucrsales == null)
                {
                    mapasucucrsales = localDM.getIndicesSucursales();
                }
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

        private void HistEntradasSalidas_Click(object sender, EventArgs e)
        {
            HistEntradasSalidas hes = new HistEntradasSalidas(idsucursal);
            hes.ShowDialog();
        }

        private void abrirCarpetaDocumentosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Configuración de carpetas
            string carpetaPrincipal = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CasaCejaDocs");            

            // Verificar si las carpetas existen
            if (!Directory.Exists(carpetaPrincipal))
            {
                MessageBox.Show("La carpeta 'CasaCejaDocs' no existe. Esta carpeta se genera automáticamente al realizar una operación.",
                                "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }           

            // Abrir la carpeta QrSalidas
            System.Diagnostics.Process.Start("explorer.exe", carpetaPrincipal);
        }
    }
}
