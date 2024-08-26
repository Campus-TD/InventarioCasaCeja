using Newtonsoft.Json;
using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Windows.Storage;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace InventarioCasaCeja
{
    public class WebDataManager
    {
        static HttpClient client;
        public LocaldataManager localDM;
        string url;
        string productos_lastupdate;
        string categorias_lastupdate;
        string medidas_lastupdate;
        string usuarios_lastupdate;
        string proveedores_lastupdate;
        string sucursales_lastupdate;
        string entradas_lastupdate;
        string entrada_producto_lastupdate;
        string salidas_temporal_lastupdate;
        public int sucursal_id;
        Action<int> refreshData;
        public Usuario activeUser;
        public WebDataManager(LocaldataManager localdataManager, Action<int> RefreshData)
        {
            this.localDM = localdataManager;
            this.refreshData = RefreshData;
            //url = "https://jorobadonciador/";
            //url = "https://f00c-187-254-98-104.ngrok.io/";
            url = "https://cm-papeleria.com/public/";
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            productos_lastupdate = localDM.getTableLastUpdate("productos");
            categorias_lastupdate = localDM.getTableLastUpdate("categorias");
            medidas_lastupdate = localDM.getTableLastUpdate("medidas");
            usuarios_lastupdate = localDM.getTableLastUpdate("usuarios");
            proveedores_lastupdate = localDM.getTableLastUpdate("proveedores");
            sucursales_lastupdate = localDM.getTableLastUpdate("sucursales");
            entradas_lastupdate = localDM.getTableLastUpdate("entradas");
            entrada_producto_lastupdate = localDM.getTableLastUpdate("entradas");
            salidas_temporal_lastupdate = localDM.getTableLastUpdate("salidas_temporal");
            
        }
        public void resetDates()
        {
            productos_lastupdate = localDM.getTableLastUpdate("productos");
            categorias_lastupdate = localDM.getTableLastUpdate("categorias");
            medidas_lastupdate = localDM.getTableLastUpdate("medidas");
            usuarios_lastupdate = localDM.getTableLastUpdate("usuarios");
            proveedores_lastupdate = localDM.getTableLastUpdate("proveedores");
            sucursales_lastupdate = localDM.getTableLastUpdate("sucursales");
            entradas_lastupdate = localDM.getTableLastUpdate("entradas");
            entrada_producto_lastupdate = localDM.getTableLastUpdate("entradas");
            salidas_temporal_lastupdate = localDM.getTableLastUpdate("salidas_temporal");
        }
        //public async Task<bool> PingServerAsync()
        //{
        //    Ping ping = new Ping();
        //    PingReply result = await ping.SendPingAsync(url);
        //    return result.Status == IPStatus.Success;
        //}
        public async Task<bool> GetCategorias()
        {
            string res="";
            Dictionary<string, string> date = new Dictionary<string, string>();
            date["fecha_de_actualizacion"] = categorias_lastupdate;
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url + "api/categorias/sincronizar", date);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    res = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["data"].ToString());
                        var categorias = JsonConvert.DeserializeObject<List<Categoria>>(data["categorias"].ToString());

                        localDM.saveCategorias(categorias);
                        categorias_lastupdate = localDM.getTableLastUpdate("categorias");
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }

                }
                else
                {
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;

        }
        public async Task<bool> GetMedidas()
        {
            string res = "";
            Dictionary<string, string> date = new Dictionary<string, string>();
            date["fecha_de_actualizacion"] = medidas_lastupdate;
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url + "api/medidas/sincronizar", date);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    res = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["data"].ToString());
                        var medidas = JsonConvert.DeserializeObject<List<Medida>>(data["medidas"].ToString());

                        localDM.saveMedidas(medidas);
                        medidas_lastupdate = localDM.getTableLastUpdate("medidas");
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }

                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }
        public async Task<bool> GetEntradaProducto()
        {
            string res = "";
            Dictionary<string, string> date = new Dictionary<string, string>();
            date["fecha_de_actualizacion"] = entrada_producto_lastupdate;

            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url + "api/entrada_producto/sincronizar", date);
                res = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        // Imprime todo el JSON recibido para verificar la estructura
                        Console.WriteLine("Respuesta JSON completa: " + result["data"].ToString());

                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["data"].ToString());

                        try
                        {
                            // Verifica si la clave es correcta
                            if (data.ContainsKey("Entrada"))
                            {
                                var entradaProductos = JsonConvert.DeserializeObject<List<EntradaProducto>>(data["Entrada"].ToString());
                                Console.WriteLine("Datos recibidos:");
                                foreach (var entradaProducto in entradaProductos)
                                {
                                    Console.WriteLine($"Entrada ID: {entradaProducto.entrada_id}, Producto ID: {entradaProducto.producto_id}, Cantidad: {entradaProducto.cantidad}, Costo: {entradaProducto.costo}");
                                }

                                localDM.saveEntradaProductos(entradaProductos);
                                entrada_producto_lastupdate = localDM.getTableLastUpdate("entradas");
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Clave 'entradaProducto' no encontrada en el JSON.");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error al deserializar o guardar los datos de EntradaProducto:");
                            Console.WriteLine(e.Message);
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error en la respuesta del servidor:");
                        Console.WriteLine(result["status"].ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Error en la conexión:");
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(res);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepción durante la solicitud HTTP:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return false;
        }
        public async Task<bool> GetEntradas()
        {
            string res = "";
            Dictionary<string, string> date = new Dictionary<string, string>();
            date["fecha_de_actualizacion"] = entradas_lastupdate;
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url + "api/entradas/sincronizar", date);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["data"].ToString());
                        var entradas = JsonConvert.DeserializeObject<List<Entrada>>(data["Entrada"].ToString());

                        localDM.saveEntradas(entradas);
                        entradas_lastupdate = localDM.getTableLastUpdate("entradas");
                        return true;
                    }
                    else
                    {
                        // Manejo de error
                    }
                }
                else
                {
                    // Manejo de error en la conexión
                }
            }
            catch (Exception e)
            {
                // Manejo de excepción
            }
            return false;
        }

        public async Task<bool> GetProveedores()
        {
            string res = "";
            Dictionary<string, string> date = new Dictionary<string, string>();
            date["fecha_de_actualizacion"] = proveedores_lastupdate;
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url + "api/proveedores/sincronizar", date);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    res = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["data"].ToString());
                        var proveedores = JsonConvert.DeserializeObject<List<Proveedor>>(data["proveedores"].ToString());

                        localDM.saveProveedores(proveedores);
                        proveedores_lastupdate = localDM.getTableLastUpdate("proveedores");
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }

                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }
        public async Task<Dictionary<string, object>> GetExistencias(string idSucursal, string arg)
        {
            string res = "";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + "api/sucursales/"+idSucursal+"/productos/"+arg);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    res = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["data"].ToString());
                        return data;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }

                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return null;
        }
        public async Task<bool> GetProductos()
        {
            string res = "";
            Dictionary<string, string> date = new Dictionary<string, string>();
            date["fecha_de_actualizacion"] = productos_lastupdate;
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url + "api/productos/sincronizar", date);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                if (result["status"].ToString().Equals("success"))

                {
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["data"].ToString());
                    var productos = JsonConvert.DeserializeObject<List<Producto>>(data["productos"].ToString());
                    localDM.saveProductos(productos);
                    productos_lastupdate = localDM.getTableLastUpdate("productos");
                        return true;
                }
                else
                {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }

            return false;
        }
        public async Task<bool> GetUsuarios()
        {
            string res = "";
            Dictionary<string, string> date = new Dictionary<string, string>();
            date["fecha_de_actualizacion"] = usuarios_lastupdate;
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url + "api/usuarios/sincronizar", date);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    res = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["data"].ToString());
                        var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(data["usuarios"].ToString());

                        localDM.saveUsuarios(usuarios);
                        usuarios_lastupdate = localDM.getTableLastUpdate("usuarios");
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }

                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }
        
        public async Task<bool> GetSucursales()
        {
            string res = "";
            Dictionary<string, string> date = new Dictionary<string, string>();
            date["fecha_de_actualizacion"] = sucursales_lastupdate;
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url + "api/sucursales/sincronizar", date);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    res = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["data"].ToString());
                        var sucursales = JsonConvert.DeserializeObject<List<Sucursal>>(data["sucursales"].ToString());

                        localDM.saveSucursales(sucursales);
                        sucursales_lastupdate = localDM.getTableLastUpdate("sucursales");
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }

                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }
        
        public async Task<bool> SendProductosAsync(List<NuevoProducto>productos)
        {
            string res = "";
            try {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data["usuario_id"] = activeUser.id.ToString();
                data["nuevos_productos"] = productos.ToArray();
                HttpResponseMessage response = await client.PostAsJsonAsync(
                url+"api/productos", data);
                if (response.IsSuccessStatusCode)
                {
                    res = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        //MessageBox.Show("Los productos se han almacenado con exito", "Completado");

                        await GetProductos();
                        refreshData(3);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "No se pudo establecer la conexion con el servidor");
            }
            return false;

        }
        public async Task<bool> SendSalidaAsync(Salida salida)
        {
            string res = "";
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    url + "api/salidas", salida);
                if (response.IsSuccessStatusCode)
                {
                    res = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }
        public async Task<bool> SendMedidaAsync(string nombreMedida)
        {
            string res = "";
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data["usuario_id"] = activeUser.id.ToString();
                data["nombre"] = nombreMedida;
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    url + "api/medidas", data);
                if (response.IsSuccessStatusCode)
                {
                    res = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Medida creada", "Completado");
                        await GetMedidas();
                        refreshData(1);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }
        public async Task<bool> SendCategoriaAsync(string categoria)
        {
            string res = "";
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data["usuario_id"] = activeUser.id.ToString();
                data["nombre"] = categoria;
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    url + "api/categorias", data);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Categoria creada", "Completado");
                        await GetCategorias();
                        refreshData(2);
                        return true;
                    }
                    else
                    {
                    //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }
        public async Task<bool> SendUsuarioAsync(Dictionary<string, string> usuario)
        {
            string res = "";
            try
            {                
                usuario["usuario_id"]= activeUser.id.ToString();
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    url + "api/usuarios", usuario);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Usuario creado", "Completado");
                        await GetUsuarios();
                        refreshData(7);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }
        public async Task<bool> SendProveedor(Dictionary<string, string> proveedor)
        {
            string res = "";
            try
            {
                proveedor["usuario_id"] = activeUser.id.ToString();
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    url + "api/proveedores", proveedor);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Registro de proveedor creado", "Completado");
                        await GetProveedores();
                        refreshData(8);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }
        public async Task<bool> SendSucursalAsync(Dictionary<string, string> sucursal)
        {
            string res = "";
            try
            {
                sucursal["usuario_id"] = activeUser.id.ToString();
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    url + "api/sucursales", sucursal);
                res = await response.Content.ReadAsStringAsync();
                //if (response.IsSuccessStatusCode)
                //{
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Sucursal creada", "Completado");
                        await GetSucursales();
                        refreshData(4);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show(result["data"].ToString(),"Error");
                    }
                //}
                //else
                //{
                //    MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                //}

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }
        public async Task<bool> SendCorte(Dictionary<string, string> data)
        {
            string res = "";
            try
            {

                HttpResponseMessage response = await client.PostAsJsonAsync(
                    url + "api/cortes", data);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);

                    if (result["status"].ToString().Equals("success"))
                    {
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show(result["data"].ToString());
                    }
                }
        }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }
        public async Task<bool> SendVentaAsync(int id, bool hasTemporal, Dictionary<string, object> venta, List<ProductoVenta> productos)
        {
            string res = "";
            bool success = true;
            if (hasTemporal)
            {
                if(await enviarAltaTemporal())
                {
                    productos = localDM.getCarrito(id);
                }
                else
                {
                    success = false;
                }

            }
            if (success)
            {
                venta["productos"] = productos;
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        url + "api/ventas", venta);
                    res = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                        if (result["status"].ToString().Equals("success"))
                        {
                            refreshData(5);
                            return true;
                        }
                        //else
                        //{
                        ////    MessageBox.Show("Error", result["data"].ToString());
                        //}
                    }
                    //else
                    //{
                    //    MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                    //}

                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
                }
            }
            
            return false;
        }
        public async Task<bool> ModifyProductoAsync(string id, Dictionary<string, string> producto)
        {
            string res = "";
            try
            {
                producto["usuario_id"] = activeUser.id.ToString();
                HttpResponseMessage response = await client.PutAsJsonAsync(
                    url + "api/productos/" + id, producto);
                response.EnsureSuccessStatusCode();
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Producto modificado", "Completado");
                        await GetProductos();
                        refreshData(3);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;

        }
        public async Task<bool> ModifyCategoriaAsync(string id, Dictionary<string, string> categoria)
        {
            string res = "";
            try
            {
                categoria["usuario_id"]= activeUser.id.ToString();
                HttpResponseMessage response = await client.PutAsJsonAsync(
                    url + "api/categorias/" + id, categoria);
                response.EnsureSuccessStatusCode();
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Categoría modificada", "Completado");
                        await GetCategorias();
                        refreshData(2);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;

        }
        public async Task<bool> ModifyMedidaAsync(string id, Dictionary<string, string> medida)
        {
            string res = "";
            try { 
                medida["usuario_id"] = activeUser.id.ToString();
                HttpResponseMessage response = await client.PutAsJsonAsync(
                url + "api/medidas/" + id, medida);
            response.EnsureSuccessStatusCode();
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Medida modificada", "Completado");
                        await GetMedidas();
                        refreshData(1);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;

        }
        public async Task<bool> ModifyUsuarioAsync(string id, Dictionary<string, string> usuario)
        {
            string res = "";
            try
            {
                usuario["usuario_id"] = activeUser.id.ToString();
                HttpResponseMessage response = await client.PutAsJsonAsync(
                    url + "api/usuarios/" + id, usuario);
                response.EnsureSuccessStatusCode();
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Registro de usuario modificado", "Completado");
                        await GetUsuarios();
                        refreshData(7);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;

        }
        public async Task<bool> ModifyProveedorAsync(string id, Dictionary<string, string> proveedor)
        {
            string res = "";
            try
            {
                proveedor["usuarior_id"] = activeUser.id.ToString();
                HttpResponseMessage response = await client.PutAsJsonAsync(
                    url + "api/proveedores/" + id, proveedor);
                response.EnsureSuccessStatusCode();
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Registro de proveedor modificado", "Completado");
                        await GetProveedores();
                        refreshData(8);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;

        }
        public async Task<bool> ModifySucursalAsync(string id, Dictionary<string, string> sucursal)
        {
            string res = "";
            try
            {
                sucursal["usuario_id"] = activeUser.id.ToString();
                HttpResponseMessage response = await client.PutAsJsonAsync(
                    url + "api/sucursales/" + id, sucursal);
                response.EnsureSuccessStatusCode();
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Registro de sucursal modificado", "Completado");
                        await GetSucursales();
                        refreshData(4);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;

        }
        public async Task<List<Usuario>> GetUsuariosSucursal(string id)
        {
            string res = "";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + "api/sucursales/"+id+"/usuarios/");
                if (response.IsSuccessStatusCode)
                {
                    res = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["data"].ToString());
                        var users = JsonConvert.DeserializeObject<List<Usuario>>(data["sucursales"].ToString());
                       
                        
                        return users;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }

                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return null;
        }
        public async Task<bool> PostUsuarioSucursal(string idSucursal, string idUsuario)
        {
            string res = "";
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data["usuario_id"] = activeUser.id.ToString();
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    url + "api/sucursales/" + idSucursal+"/usuarios/"+idUsuario, data);
                response.EnsureSuccessStatusCode();
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;

        }
        public async Task<bool> RemoveUsuarioSucursal(string idSucursal, string idUsuario)
        {
            string res = "";
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data["_method"] = "delete";
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    url + "api/sucursales/" + idSucursal + "/usuarios/" + idUsuario, data);
                response.EnsureSuccessStatusCode();
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;

        }
        public async Task<bool> DisableProductoAsync(string id)
        {
            string res = "";
            try
            {
                Dictionary<string, string> user_id = new Dictionary<string, string>();
                user_id["usuario_id"] = activeUser.id.ToString();
                HttpResponseMessage response = await client.PostAsJsonAsync(
                url + "api/productos/desactivar/" + id, user_id);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Producto eliminado", "Completado");
                        await GetProductos();
                        refreshData(3);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;

        }
        public async Task<bool> DisableCategoriaAsync(string id)
        {
            string res = "";
            try
            {
                Dictionary<string, string> user_id = new Dictionary<string, string>();
                user_id["usuario_id"] = activeUser.id.ToString();
                HttpResponseMessage response = await client.PostAsJsonAsync(
                url + "api/categorias/desactivar/" + id, user_id);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Categoria eliminada", "Completado");
                        await GetCategorias();
                        refreshData(2);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }
        public async Task<bool> DisableMedidaAsync(string id)
        {
            string res = "";
            try
            {
                Dictionary<string, string> user_id = new Dictionary<string, string>();
                user_id["usuario_id"] = activeUser.id.ToString();
                HttpResponseMessage response = await client.PostAsJsonAsync(
                url + "api/medidas/desactivar/" + id, user_id);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Medida eliminada", "Completado");
                        await GetMedidas();
                        refreshData(1);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }
        public async Task<bool> DisableUsuarioAsync(string id)
        {
            string res = "";
            try
            {
                Dictionary<string, string> user_id = new Dictionary<string, string>();
                user_id["usuario_id"] = activeUser.id.ToString();
                HttpResponseMessage response = await client.PostAsJsonAsync(
                url + "api/usuarios/desactivar/" + id, user_id);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Usuario eliminado", "Completado");
                        await GetUsuarios();
                        refreshData(7);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }
        public async Task<bool> DisableProveedorAsync(string id)
        {
            string res = "";
            try
            {
                Dictionary<string, string> user_id = new Dictionary<string, string>();
                user_id["usuario_id"] = activeUser.id.ToString();
                HttpResponseMessage response = await client.PostAsJsonAsync(
                url + "api/proveedores/desactivar/" + id, user_id);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Proveedor eliminado", "Completado");
                        await GetProveedores();
                        refreshData(8);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }
        public async Task<bool> DisableSucursalAsync(string id)
        {
            string res = "";
            try
            {
                Dictionary<string, string> user_id = new Dictionary<string, string>();
                user_id["usuario_id"] = activeUser.id.ToString();
                HttpResponseMessage response = await client.PostAsJsonAsync(
                url + "api/sucursales/desactivar/" + id, user_id);
                res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        MessageBox.Show("Sucursal eliminada", "Completado");
                        await GetSucursales();
                        refreshData(4);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return false;
        }

        public async Task<bool> enviarAltaTemporal()
        {
            List<NuevoProducto> productos = localDM.GetProductosTemporales();
            if(await SendProductosAsync(productos))
            {
                localDM.reconnectProductosVenta(productos);
                localDM.reconnectProductosEntrada(productos);
                localDM.clearAltaTemporal();
                return true;
            }
            return false;
        }
        public async Task<bool> SendEntrada(int id, bool hasTemporal, Dictionary<string, object> entrada, List<ProductoEntrada> productos)
        {
            string res;
            bool success = true;
            if (hasTemporal)
            {
                if (await enviarAltaTemporal())
                {
                    productos = localDM.getListaEntrada(id);
                }
                else
                {
                    success = false;
                }

            }
            if (success)
            {
                entrada["entrada_productos"] = productos;
                try
                {

                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        url + "api/entradas", entrada);
                    res = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                        if (result["status"].ToString().Equals("success"))
                        {
                            MessageBox.Show("Entrada enviada", "Completado");
                            return true;
                        }
                        else
                        {
                            //MessageBox.Show("Error", result["data"].ToString());
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor, favor de intentar mas tarde", "Error");
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
                }
            }

            return false;
        }
        public async Task<List<ProductoExistencia>> getProductoExistencia (string idproducto)
        {
            string res;
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + "api/productos/" + idproducto + "/existencias/");
                if (response.IsSuccessStatusCode)
                {
                    res = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (result["status"].ToString().Equals("success"))
                    {
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["data"].ToString());
                        var prods = JsonConvert.DeserializeObject<List<ProductoExistencia>>(data["existencias"].ToString());


                        return prods;
                    }
                    else
                    {
                        //MessageBox.Show("Error", result["data"].ToString());
                    }

                }
                else
                {
                    //MessageBox.Show("Hubo un problema al establecer la conexion con el servidor", "Error");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Hubo un problema al establecer la conexion con el servidor");
            }
            return null;
        }
        public async Task<bool> sumarExistencia(int sucursalId, int productoId, double cantidad)
        {
            string apiUrl = $"{url}api/sucursales/{sucursalId}/productos/{productoId}/add"; // Asegúrate de que esta ruta coincida con la que definiste en Laravel
            Console.WriteLine($"Making request to: {apiUrl}");
            var requestData = new
            {
                cantidad = cantidad
            };

            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(apiUrl, requestData);
                string res = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response status code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Producto actualizado con éxito.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Error al actualizar el producto.");
                    Console.WriteLine(res);
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> restarExistencia(int sucursalId, int productoId, double cantidad)
        {
            string apiUrl = $"{url}api/sucursales/{sucursalId}/productos/{productoId}";
            //Console.WriteLine($"Making request to: {apiUrl}");
            var requestData = new
            {
                cantidad = cantidad
            };

            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(apiUrl, requestData);
                string res = await response.Content.ReadAsStringAsync();

                //Console.WriteLine($"Response status code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("Producto actualizado con éxito.");
                    return true;
                }
                else
                {
                    //Console.WriteLine("Error al actualizar el producto.");
                    //Console.WriteLine(res);
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
