using System.Collections.Generic;
using System.Data.SQLite;
using Windows.Storage;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using System;

namespace InventarioCasaCeja
{
    public class LocaldataManager
    {
        public string impresora = "";
        string dbpath;
        public SQLiteConnection connection;
        public LocaldataManager()
        {
            dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"CasaCeja\DataBase\catalogo.db");

            if (!File.Exists(dbpath))
            {
                string subPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"CasaCeja\DataBase");
                Directory.CreateDirectory(subPath);

                SQLiteConnection.CreateFile(dbpath);

                connection = new SQLiteConnection(@"data source=" + dbpath);

                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE 'abonos_apartado' (    'id'    INTEGER,	'folio' TEXT,	'metodo_pago'   TEXT,	'total_abonado' REAL,	'fecha' TEXT,	'id_apartado'   INTEGER,	'folio_corte'   TEXT,	'id_cajero' INTEGER,	'created_at'    TEXT,	'updated_at'    TEXT,	FOREIGN KEY('id_apartado') REFERENCES 'apartados'('id'),	FOREIGN KEY('id_cajero') REFERENCES 'usuarios'('id'))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'abonos_apartado_temporal' (    'id'    INTEGER NOT NULL,	'folio' TEXT,	'metodo_pago'   TEXT,	'total_abonado' REAL,	'fecha' TEXT,	'folio_apartado'    TEXT,	'id_apartado'   INTEGER,	'folio_corte'   TEXT,	'id_cajero' INTEGER,	PRIMARY KEY('id' AUTOINCREMENT))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'abonos_credito' (    'id'    INTEGER,	'folio' TEXT,	'metodo_pago'   TEXT,	'total_abonado' REAL,	'fecha' TEXT,	'id_credito'    INTEGER,	'folio_corte'   TEXT,	'id_cajero' INTEGER,	'created_at'    TEXT,	'updated_at'    TEXT,	FOREIGN KEY('id_credito') REFERENCES 'creditos'('id'),	FOREIGN KEY('id_cajero') REFERENCES 'usuarios'('id'))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'abonos_credito_temporal' (    'id'    INTEGER NOT NULL,	'folio' TEXT,	'metodo_pago'   TEXT,	'total_abonado' REAL,	'fecha' TEXT,	'folio_credito' TEXT,	'id_credito'    INTEGER,	'folio_corte'   TEXT,	'id_cajero' INTEGER,	PRIMARY KEY('id' AUTOINCREMENT))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'alta_temporal' (    'id'    INTEGER NOT NULL,	'codigo'    TEXT,	'nombre'    TEXT,	'presentacion'  TEXT,	'menudeo'   REAL,	'mayoreo'   REAL,	'cantidad_mayoreo'  INTEGER,	'especial'  REAL,	'vendedor'  REAL,	'medida_id' INTEGER,	'categoria_id'  INTEGER,	'estado'    INTEGER,	'detalles'  TEXT,	FOREIGN KEY('medida_id') REFERENCES 'medidas'('id'),	PRIMARY KEY('id' AUTOINCREMENT),	FOREIGN KEY('categoria_id') REFERENCES 'categorias'('id'))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'apartados' (    'id'    INTEGER,	'productos' TEXT,	'total' REAL,	'total_pagado'  REAL,	'fecha_apartado'    TEXT,	'folio_corte' TEXT,	'fecha_entrega' TEXT,	'estado'    INTEGER,	'cliente_creditos_id'   INTEGER,	'id_cajero_registro'    INTEGER,	'id_cejero_entrega' INTEGER,	'sucursal_id'   INTEGER,	'observaciones' TEXT,	'created_at'    TEXT,	'updated_at'    TEXT,	FOREIGN KEY('id_cajero_registro') REFERENCES 'usuarios'('id'),	FOREIGN KEY('id_cejero_entrega') REFERENCES 'usuarios'('id'),	PRIMARY KEY('id' AUTOINCREMENT))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'apartados_temporal' (    'id'    INTEGER,	'productos' TEXT,	'total' REAL,	'total_pagado'  REAL,	'fecha_apartado'    TEXT,	'folio_corte' TEXT,	'fecha_entrega' TEXT,	'estado'    INTEGER,	'cliente_creditos_id'   INTEGER,	'id_cajero_registro'    INTEGER,	'id_cajero_entrega' INTEGER,	'sucursal_id'   INTEGER,	'temporal'  INTEGER,	'observaciones' TEXT,	PRIMARY KEY('id' AUTOINCREMENT),	FOREIGN KEY('id_cajero_entrega') REFERENCES 'usuarios'('id'),	FOREIGN KEY('sucursal_id') REFERENCES 'sucursales'('id'),	FOREIGN KEY('id_cajero_registro') REFERENCES 'usuarios'('id'))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'categorias' (    'id'    INTEGER NOT NULL,	'nombre'    TEXT,	'activo'    INTEGER,	'created_at'    TEXT,	'updated_at'    TEXT,	PRIMARY KEY('id'))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'clientes' (    'id'    INTEGER,	'nombre'    TEXT,	'rfc'   TEXT,	'calle' TEXT,	'no_exterior'   TEXT,	'no_interior'   TEXT,	'cp'    TEXT,	'colonia'   TEXT,	'ciudad'    TEXT,	'telefono'  TEXT,	'correo'    TEXT,	'activo'    INTEGER,	'created_at'    TEXT,	'updated_at'    TEXT,	PRIMARY KEY('id'))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'clientes_temporal' (    'id'    INTEGER,	'nombre'    TEXT,	'rfc'   TEXT,	'calle' TEXT,	'no_exterior'   TEXT,	'no_interior'   TEXT,	'cp'    TEXT,	'colonia'   TEXT,	'ciudad'    TEXT,	'telefono'  TEXT,	'correo'    TEXT,	PRIMARY KEY('id' AUTOINCREMENT))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'cortes' (    'id'    INTEGER NOT NULL UNIQUE,    'folio_corte' TEXT,	'fondo_apertura'    REAL,	'total_efectivo'    REAL,	'total_tarjetas_debito'  REAL,	'total_tarjetas_credito' REAL,	'total_cheques' REAL,	'total_transferencias'  REAL,	'efectivo_apartados'    REAL,	'efectivo_creditos' REAL,	'gastos'    TEXT,	'ingresos'  TEXT,	'sobrante'  REAL,	'fecha_apertura_caja'    TEXT,	'fecha_corte_caja'   TEXT,	'sucursal_id'   INTEGER,	'usuario_id'    INTEGER,	'estado'    INTEGER, 'detalles'  TEXT, 'created_at'    TEXT, 'updated_at'	TEXT, FOREIGN KEY('sucursal_id') REFERENCES 'sucursales'('id'),	FOREIGN KEY('usuario_id') REFERENCES 'usuarios'('id'),	PRIMARY KEY('id' AUTOINCREMENT))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'creditos' (    'id'    INTEGER,	'productos' TEXT,	'total' REAL,	'total_pagado'  REAL,	'fecha_de_credito'  TEXT,	'folio' TEXT,	'estado'    INTEGER,	'cliente_creditos_id'   INTEGER,	'id_cajero_registro'    INTEGER,	'sucursal_id'   INTEGER,	'observaciones' TEXT,	'created_at'    TEXT,	'updated_at'    TEXT,	FOREIGN KEY('sucursal_id') REFERENCES 'sucursales'('id'),	FOREIGN KEY('id_cajero_registro') REFERENCES 'usuarios'('id'),	PRIMARY KEY('id'))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'creditos_temporal' (    'id'    INTEGER,	'productos' TEXT,	'total' REAL,	'total_pagado'  REAL,	'fecha_de_credito'  TEXT,	'folio' TEXT,	'estado'    INTEGER,	'cliente_creditos_id'   INTEGER,	'id_cajero_registro'    INTEGER,	'sucursal_id'   INTEGER,	'temporal'  INTEGER,	'observaciones' TEXT,	FOREIGN KEY('id_cajero_registro') REFERENCES 'usuarios'('id'),	FOREIGN KEY('sucursal_id') REFERENCES 'sucursales'('id'),	PRIMARY KEY('id' AUTOINCREMENT))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'entradas' (    'id'    INTEGER NOT NULL,	'fecha_factura' TEXT,	'total_factura' REAL,	'folio_factura' TEXT,	'usuario_id'    INTEGER,	'sucursal_id'   INTEGER,	'proveedor_id'  INTEGER,	'cancelacion'   INTEGER,	'estado'    INTEGER,	'detalles'  TEXT, 'created_at'    TEXT, 'updated_at'    TEXT,	PRIMARY KEY('id' AUTOINCREMENT))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'medidas' (    'id'    INTEGER NOT NULL,	'nombre'    TEXT,	'activo'    INTEGER,	'created_at'    TEXT,	'updated_at'    TEXT,	PRIMARY KEY('id'))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'operaciones' (    'id'    INTEGER NOT NULL,	'accion'    TEXT,	'confirmar' INTEGER,	'created_at'    TEXT,	'updated_at'    TEXT,	'producto_id'   INTEGER,	'usuario_id'    INTEGER,	PRIMARY KEY('id'))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'producto_entrada' (    'id'    INTEGER NOT NULL,	'entrada_id'    INTEGER,	'producto_id'   INTEGER,	'codigo'    INTEGER,	'cantidad'  INTEGER,	'costo' REAL,	'estado'    INTEGER,	'detalles'  TEXT, 'created_at'    TEXT, 'updated_at'    TEXT,	PRIMARY KEY('id' AUTOINCREMENT))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'producto_venta' (    'id'    INTEGER NOT NULL,	'venta_id'  INTEGER,	'producto_id'   INTEGER,	'codigo'    TEXT,	'cantidad'  INTEGER,	'precio_venta'  REAL,	'estado'    INTEGER,	'detalles'  TEXT,	PRIMARY KEY('id' AUTOINCREMENT),	FOREIGN KEY('producto_id') REFERENCES 'productos'('id'),	FOREIGN KEY('venta_id') REFERENCES 'ventas'('id'))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'productos' (    'id'    INTEGER NOT NULL,	'codigo'    TEXT,	'nombre'    TEXT,	'presentacion'  TEXT,	'iva'   REAL,	'menudeo'   REAL,	'mayoreo'   REAL,	'cantidad_mayoreo'  INTEGER,	'especial'  REAL,	'vendedor'  REAL,	'imagen'    TEXT,	'activo'    INTEGER,	'created_at'    TEXT,	'updated_at'    TEXT,	'medida_id' INTEGER,	'categoria_id'  INTEGER,	FOREIGN KEY('categoria_id') REFERENCES 'categorias'('id'),	PRIMARY KEY('id'),	FOREIGN KEY('medida_id') REFERENCES 'medidas'('id'))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'proveedores' (    'id'    INTEGER NOT NULL,	'nombre'    TEXT,	'direccion' TEXT,	'correo'    TEXT,	'telefono'  TEXT,	'descripcion'   TEXT,	'activo'    INTEGER,	'created_at'    TEXT,	'updated_at'    TEXT,	PRIMARY KEY('id'))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'salidas_temporal' (    'id'    INTEGER NOT NULL,	'id_sucursal_origen'    INTEGER,	'id_sucursal_destino'   INTEGER,	'productos' TEXT,	'folio' TEXT,	'fecha_salida'  TEXT,	'usuario_id'    INTEGER,	'total_importe' REAL,	'estado'    INTEGER, 'created_at'    TEXT, 'updated_at'    TEXT,	PRIMARY KEY('id' AUTOINCREMENT))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'sucursales' (    'id'    INTEGER NOT NULL,	'puerta_enlace1'    TEXT,	'puerta_enlace2'    TEXT,	'puerta_enlace3'    TEXT,	'puerta_enlace4'    TEXT,	'razon_social'  TEXT,	'direccion' TEXT,	'correo'    TEXT,	'activo'    INTEGER,	'created_at'    TEXT,	'updated_at'    TEXT,	PRIMARY KEY('id' AUTOINCREMENT))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'usuarios' (    'id'    INTEGER NOT NULL,	'nombre'    TEXT,	'correo'    TEXT,	'confirmacion'  INTEGER,	'telefono'  TEXT,	'imagen'    TEXT,	'usuario'   TEXT,	'clave' TEXT,	'is_root'   INTEGER,	'activo'    INTEGER,	'created_at'    TEXT,	'updated_at'    TEXT,	PRIMARY KEY('id'))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE 'ventas' (    'id'    INTEGER NOT NULL,	'total' REAL,	'folio' TEXT,	'folio_corte'   TEXT,	'fecha_venta'   TEXT,	'metodo_pago'   TEXT,	'tipo'  INTEGER,	'sucursal_id'   INTEGER,	'usuario_id'    INTEGER,	'cancelacion'   TEXT,	'estado'    INTEGER,	'detalles'  TEXT,	FOREIGN KEY('usuario_id') REFERENCES 'usuarios'('id'),	FOREIGN KEY('sucursal_id') REFERENCES 'sucursales'('id'),	PRIMARY KEY('id' AUTOINCREMENT))";
                command.ExecuteNonQuery();
            }
            else
            {
                connection = new SQLiteConnection(@"data source=" + dbpath);
                connection.Open();
            }
        }

        // verifica si un producto ya existe en la bd antes de agregarlo a la lista de alta productos.
        public (bool, string) ProductoExiste(string codigoBarras)
        {
            string query = "SELECT Nombre FROM Productos WHERE codigo = @codigo";
            using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@codigo", codigoBarras);
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string producto = reader.GetString(0);
                        connection.Close();
                        return (true, producto);
                    }
                    else
                    {
                        connection.Close();
                        return (false, null);
                    }
                }
            }
        }

        public void setImpresora(string impresora)
        {
            this.impresora = impresora;
        }
        public bool IsTableEmpty(string table)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) AS RowCnt FROM " + table;
            command.Parameters.AddWithValue("setTable", table);
            SQLiteDataReader result = command.ExecuteReader();
            if (result.Read())
            {
                if (result.GetInt32(0) == 0)
                {
                    return true;
                }
            }
            
            return false;
        }
        public int getTableRowCount(string table)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) AS RowCnt FROM "+table+" WHERE activo=1";
            command.Parameters.AddWithValue("setTable", table);
            SQLiteDataReader result = command.ExecuteReader();
            if (result.Read())
            {
                int count = result.GetInt32(0);
                return count;
            }
            return 0;
        }
        public string getTableLastUpdate(string table)
        {
            string last_update = "2000-01-01T00:00:00Z";
            if(!IsTableEmpty(table))
            {
                string query = "SELECT updated_at FROM " + table+" ORDER BY updated_at DESC LIMIT 1";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader result = command.ExecuteReader();
                if (result.Read())
                {
                }
            }
            return last_update;
        }
        public Dictionary<string, int> getIndicesAdministradores()
        {
            Dictionary<string, int> map = new Dictionary<string, int>();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id, nombre FROM usuarios WHERE is_root = 1 AND activo = 1";
            SQLiteDataReader result = command.ExecuteReader();
            while (result.Read())
            {
                map.Add(result.GetString(1), result.GetInt32(0));
            }
            return map;
        }
        public Dictionary<string, int> getIndicesMedidas()
        {
            Dictionary<string, int> map = new Dictionary<string, int>();

            string query = "SELECT * FROM medidas WHERE activo=1";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader result = command.ExecuteReader();
            while (result.Read())
            {
                map.Add(result.GetString(1), result.GetInt32(0));
            }
            return map;
        }
        public Dictionary<string, int> getIndicesCategorias()
        {
            Dictionary<string, int> map = new Dictionary<string, int>();
            string query = "SELECT * FROM categorias WHERE activo=1";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader result = command.ExecuteReader();
            while (result.Read())
            {
                string key = result.GetString(1);
                if (!map.ContainsKey(key))
                    map.Add(key, result.GetInt32(0));
            }
            return map;
        }
        public Dictionary<string, int> getIndicesSucursales()
        {
            Dictionary<string, int> map = new Dictionary<string, int>();

            string query = "SELECT * FROM sucursales WHERE activo = 1";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader result = command.ExecuteReader();
            while (result.Read())
            {
                map.Add(result.GetString(5), result.GetInt32(0));
            }
            return map;
        }
        /*
         * Codigo anterior
         * 
        public Dictionary<string, int> getIndicesProveedores()
        {
            Dictionary<string, int> map = new Dictionary<string, int>();

            string query = "SELECT * FROM proveedores";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader result = command.ExecuteReader();
            while (result.Read())
            {
                map.Add(result.GetString(1), result.GetInt32(0));
            }
            return map;
        }
        */
        public Dictionary<string, int> getIndicesProveedores() // Codigo Actual
        {
            Dictionary<string, int> map = new Dictionary<string, int>();

            string query = "SELECT * FROM proveedores WHERE activo = 1";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader result = command.ExecuteReader();
            while (result.Read())
            {
                string key = result.GetString(1);
                int value = result.GetInt32(0);

                if (!map.ContainsKey(key))
                {
                    map.Add(key, value);
                }
                else
                {
                    Console.WriteLine("La clave duplicada fue encontrada: " + key);
                }
            }
            return map;
        }

        public Sucursal getSucursal(int id)
        {
            Sucursal s = null;
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM sucursales WHERE id = @setId LIMIT 1";
            command.Parameters.AddWithValue("setId", id);
            SQLiteDataReader result = command.ExecuteReader();
            if (result.Read())
            {
                s = new Sucursal
                {
                    id = result.GetInt32(0),
                    puerta_enlace1 = result.IsDBNull(1) ? "" : result.GetString(1),
                    puerta_enlace2 = result.IsDBNull(2) ? "" : result.GetString(2),
                    puerta_enlace3 = result.IsDBNull(3) ? "" : result.GetString(3),
                    puerta_enlace4 = result.IsDBNull(4) ? "" : result.GetString(4),
                    razon_social = result.IsDBNull(5) ? "" : result.GetString(5),
                    direccion = result.IsDBNull(6) ? "" : result.GetString(6),
                    correo = result.IsDBNull(7) ? "" : result.GetString(7),
                    activo = result.GetInt32(8),
                };
                
            }
            return s;
        }

        public void guardarSalidaTemporal(Salida salida, int idsalida)
        {
            int maxRetries = 3;
            int retryDelayMilliseconds = 100;

            for (int retryCount = 0; retryCount < maxRetries; retryCount++)
            {
                try
                {
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE salidas_temporal SET id_sucursal_origen = @setOrigen, id_sucursal_destino = @setDestino, productos = @setProductos, folio = @setFolio," +
                        " fecha_salida = @setFecha, usuario_id = @setUsuario, total_importe = @setTotal, estado = @setEstado WHERE id = @setId";
                    command.Parameters.AddWithValue("setOrigen", salida.id_sucursal_origen);
                    command.Parameters.AddWithValue("setDestino", salida.id_sucursal_destino);
                    command.Parameters.AddWithValue("setProductos", salida.productos);
                    command.Parameters.AddWithValue("setFolio", salida.folio);
                    command.Parameters.AddWithValue("setFecha", salida.fecha_salida);
                    command.Parameters.AddWithValue("setUsuario", salida.usuario_id);
                    command.Parameters.AddWithValue("setTotal", salida.total_importe);
                    command.Parameters.AddWithValue("setEstado", 1);
                    command.Parameters.AddWithValue("setId", idsalida);
                    command.ExecuteNonQuery();

                    // If the update operation succeeds, exit the retry loop
                    return;
                }
                catch (SQLiteException ex) when (ex.Message.Contains("database is locked"))
                {
                    // If the database is still locked, wait for a short period before retrying
                    System.Threading.Thread.Sleep(retryDelayMilliseconds);
                }
            }

            // If the maximum number of retries is reached and the update operation still fails, you can handle the error or throw an exception
            throw new Exception("Error al actualizar la tabla salidas_temporal debido a un bloqueo en la base de datos.");
        }
        public void confirmarSalidaTemporal(int idsalida)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE salidas_temporal SET estado = 2 WHERE id = @setID";
            command.Parameters.AddWithValue("setId", idsalida);
            command.ExecuteNonQuery();
        }
        public DataTable getProductos( string offset)
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText= "SELECT productos.id AS ID, productos.codigo AS CODIGO, productos.nombre AS NOMBRE, categorias.nombre AS CATEGORIA, medidas.nombre AS MEDIDA, productos.presentacion AS PRESENTACION, productos.iva AS IVA," +
                " productos.menudeo AS MENUDEO, productos.mayoreo AS MAYOREO, productos.cantidad_mayoreo AS 'CANTIDAD MAYOREO', productos.especial AS ESPECIAL, productos.vendedor AS VENDEDOR" +
                " FROM productos INNER JOIN categorias ON productos.categoria_id = categorias.id INNER JOIN medidas ON productos.medida_id = medidas.id WHERE productos.activo=1 LIMIT 21 OFFSET @setOffset";
            command.Parameters.AddWithValue("setOffset", offset);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public DataTable getProductos(string offset, string arg)
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT productos.id AS ID, productos.codigo AS CODIGO, productos.nombre AS NOMBRE, categorias.nombre AS CATEGORIA, medidas.nombre AS MEDIDA, productos.presentacion AS PRESENTACION, productos.iva AS IVA," +
                " productos.menudeo AS MENUDEO, productos.mayoreo AS MAYOREO, productos.cantidad_mayoreo AS 'CANTIDAD MAYOREO', productos.especial AS ESPECIAL, productos.vendedor AS VENDEDOR" +
                " FROM productos INNER JOIN categorias ON productos.categoria_id = categorias.id INNER JOIN medidas ON productos.medida_id = medidas.id WHERE productos.activo=1 "+arg+" LIMIT 21 OFFSET @setOffset";
            command.Parameters.AddWithValue("setOffset", offset);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public int getProductosRowCount(string arg)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) AS RowCnt FROM productos WHERE activo=1 "+ arg;
            SQLiteDataReader result = command.ExecuteReader();
            if (result.Read())
            {
                int count = result.GetInt32(0);
                return count;
            }
            return 0;
        }
        public int getProductosRowCount(string arg, string arg2)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) AS RowCnt FROM productos WHERE activo=1 AND (productos.codigo LIKE @setCodigo OR productos.nombre LIKE @setNombre) " + arg;
            command.Parameters.AddWithValue("setCodigo", "%" + arg2 + "%");
            command.Parameters.AddWithValue("setNombre", "%" + arg2 + "%");
            SQLiteDataReader result = command.ExecuteReader();
            if (result.Read())
            {
                int count = result.GetInt32(0);
                return count;
            }
            return 0;
        }
        public DataTable getProductos(string offset, string arg, string arg2)
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT productos.id AS ID, productos.codigo AS CODIGO, productos.nombre AS NOMBRE, categorias.nombre AS CATEGORIA, medidas.nombre AS MEDIDA, productos.presentacion AS PRESENTACION, productos.iva AS IVA," +
                " productos.menudeo AS MENUDEO, productos.mayoreo AS MAYOREO, productos.cantidad_mayoreo AS 'CANTIDAD MAYOREO', productos.especial AS ESPECIAL, productos.vendedor AS VENDEDOR" +
                " FROM productos INNER JOIN categorias ON productos.categoria_id = categorias.id INNER JOIN medidas ON productos.medida_id = medidas.id WHERE productos.activo=1 AND (productos.codigo LIKE @setCodigo OR productos.nombre LIKE @setNombre) " + arg + " LIMIT 21 OFFSET @setOffset";
            command.Parameters.AddWithValue("setOffset", offset);
            command.Parameters.AddWithValue("setCodigo", "%"+arg2+"%");
            command.Parameters.AddWithValue("setNombre", "%" + arg2 + "%");
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }

        public DataTable getOperaciones()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM operaciones";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public DataTable getSucursales()
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id AS ID, razon_social AS RAZON_SOCIAL, direccion AS DIRECCION, correo AS CORREO, puerta_enlace1 AS PUERTA_DE_ENLACE_1, puerta_enlace2 AS PUERTA_DE_ENLACE_2," +
                " puerta_enlace3 AS PUERTA_DE_ENLACE_3, puerta_enlace4 AS PUERTA_DE_ENLACE_4 FROM sucursales WHERE activo = 1";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public DataTable getSucursales(string arg)
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id AS ID, razon_social AS RAZON_SOCIAL, direccion AS DIRECCION, correo AS CORREO, puerta_enlace1 AS PUERTA_DE_ENLACE_1, puerta_enlace2 AS PUERTA_DE_ENLACE_2," +
                " puerta_enlace3 AS PUERTA_DE_ENLACE_3, puerta_enlace4 AS PUERTA_DE_ENLACE_4 FROM sucursales WHERE activo = 1 AND razon_social LIKE @setRazon";
            command.Parameters.AddWithValue("setRazon", "%" + arg + "%");
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public Usuario getLoginUser(string usuario, string contraseña)
        {
            Usuario usr = null;
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM usuarios WHERE usuario = @setUsuario AND clave = @setClave AND activo = 1";
            command.Parameters.AddWithValue("setUsuario", usuario);
            command.Parameters.AddWithValue("setClave", contraseña);
            SQLiteDataReader result = command.ExecuteReader();
            if (result.Read())
            {
                usr = new Usuario();
                usr.id = result.GetInt32(0);
                usr.nombre = result.GetString(1);
                usr.correo = result.GetString(2);
                usr.telefono = result.GetString(4);
                usr.imagen = result.IsDBNull(5)?"":result.GetString(5);
                usr.usuario = result.GetString(6);
                usr.clave = result.GetString(7);
                usr.es_raiz = result.GetInt32(8);
            }
            return usr;
        }
        public void clearTabble (string table)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM "+table;
            command.ExecuteNonQuery();
        }
        public void ClearDatabase()
        {
            
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM categorias";
            command.ExecuteScalar();
            command.CommandText = "DELETE FROM medidas";
            command.ExecuteScalar();
            command.CommandText = "DELETE FROM productos";
            command.ExecuteScalar();
            command.CommandText = "DELETE FROM usuarios";
            command.ExecuteScalar();
            command.CommandText = "DELETE FROM operaciones";
            command.ExecuteScalar();
            command.CommandText = "DELETE FROM producto_venta";
            command.ExecuteScalar();
            command.CommandText = "DELETE FROM sucursales";
            command.ExecuteScalar();
            command.CommandText = "DELETE FROM ventas";
            command.ExecuteScalar();
            command.CommandText = "DELETE FROM entradas";
            command.ExecuteScalar();
            command.CommandText = "DELETE FROM producto_entrada";
            command.ExecuteScalar();
            command.CommandText = "DELETE FROM alta_temporal";
            command.ExecuteScalar();
            command.CommandText = "DELETE FROM proveedores";
            command.ExecuteScalar();
            command.CommandText = "DELETE FROM cortes";
            command.ExecuteScalar();
            command.CommandText = "DELETE FROM salidas_temporal";
            command.ExecuteScalar();
        }
        public DataTable getCategorias()
        {
            DataTable dt = new DataTable();
            string query = "SELECT id AS ID, nombre AS CATEGORIA FROM categorias WHERE activo = 1";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public DataTable getCategorias(string arg)
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id AS ID, nombre AS CATEGORIA FROM categorias WHERE activo=1 AND nombre LIKE @setNombre";
            command.Parameters.AddWithValue("setNombre", "%"+arg+"%");
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public DataTable getVentas()
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT ventas.id AS ID, ventas.total AS TOTAL, ventas.folio AS FOLIO, ventas.fecha_venta AS FECHA_DE_VENTA, sucursales.razon_social AS SUCURSAL FROM ventas INNER JOIN sucursales ON ventas.sucursal_id=sucursales.id";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        
        public DataTable getVentas(string arg)
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT ventas.id AS ID, ventas.total AS TOTAL, ventas.folio AS FOLIO, ventas.fecha_venta AS FECHA_DE_VENTA, sucursales.razon_social AS SUCURSAL FROM ventas INNER JOIN sucursales ON ventas.sucursal_id=sucursales.id WHERE folio LIKE @setFolio";
            command.Parameters.AddWithValue("setFolio", "%" + arg + "%");
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public DataTable getProveedores()
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM proveedores WHERE activo = 1";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public DataTable getProveedores(string arg)
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM proveedores WHERE nombre LIKE @setNombre";
            command.Parameters.AddWithValue("setNombre", "%" + arg + "%");
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public DataTable getProductoVentas(string id_venta)
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT producto_venta.id AS ID, ventas.folio AS FOLIO, productos.codigo AS CODIGO, productos.nombre AS NOMBRE_PRODUCTO, producto_venta.cantidad AS CANTIDAD, producto_venta.precio_venta AS PRECIO_VENTA FROM producto_venta INNER JOIN ventas ON producto_venta.venta_id=ventas.id INNER JOIN productos ON producto_venta.producto_id=productos.id WHERE producto_venta.venta_id = @setId";
            command.Parameters.AddWithValue("setId", id_venta);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public DataTable getMedidas()
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id AS ID, nombre AS MEDIDA FROM medidas WHERE activo=1";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public DataTable getMedidas(string arg)
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id AS ID, nombre AS MEDIDA FROM medidas WHERE activo=1 AND nombre LIKE @setNombre";
            command.Parameters.AddWithValue("setNombre", "%" + arg + "%");
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }

        public DataTable getUsuarios()
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id AS ID, nombre AS NOMBRE, correo AS CORREO, confirmacion AS CONFIRMACION, telefono AS TELEFONO, imagen AS FOTOGRAFIA, usuario as USUARIO, clave AS CLAVE," +
                "is_root AS NIVEL FROM usuarios WHERE activo=1 AND is_root > 0";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public DataTable getUsuarios(string arg)
        {
            DataTable dt = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id AS ID, nombre AS NOMBRE, correo AS CORREO, confirmacion AS CONFIRMACION, telefono AS TELEFONO, imagen AS FOTOGRAFIA, usuario as USUARIO, clave AS CLAVE," +
                "is_root AS NIVEL FROM usuarios WHERE activo=1 AND is_root > 0 AND nombre LIKE @setNombre";
            command.Parameters.AddWithValue("setNombre", "%" + arg + "%");
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
        public void saveMedidas( List<Medida> medidas)
        {
            foreach (Medida medida in medidas)
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT OR REPLACE INTO medidas (id, nombre, activo, created_at, updated_at) " +
                "VALUES(@setId, @setNombre, @setActivo, @setCreated_at, @setUpdated_at) ";
                command.Parameters.AddWithValue("setId", medida.id);
                command.Parameters.AddWithValue("setNombre", medida.nombre);
                command.Parameters.AddWithValue("setActivo", medida.activo);
                command.Parameters.AddWithValue("setCreated_at", medida.created_at);
                command.Parameters.AddWithValue("setUpdated_at", medida.updated_at);
                command.ExecuteScalar();
            }
        }
        public void saveCategorias(List<Categoria> categorias)
        {
            foreach (Categoria categoria in categorias)
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT OR REPLACE INTO categorias (id, nombre, activo, created_at, updated_at) " +
                "VALUES(@setId, @setNombre, @setActivo, @setCreated_at, @setUpdated_at) ";
                command.Parameters.AddWithValue("setId", categoria.id);
                command.Parameters.AddWithValue("setNombre", categoria.nombre);
                command.Parameters.AddWithValue("setActivo", categoria.activo);
                command.Parameters.AddWithValue("setCreated_at", categoria.created_at);
                command.Parameters.AddWithValue("setUpdated_at", categoria.updated_at);
                command.ExecuteScalar();
            }
        }
        public void saveProveedores(List<Proveedor> proveedores)
        {
            foreach (Proveedor proveedor in proveedores)
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT OR REPLACE INTO proveedores (id, nombre, direccion, correo, telefono, descripcion, activo, created_at, updated_at) " +
                "VALUES(@setId, @setNombre, @setDireccion, @setCorreo, @setTelefono, @setDescripcion, @setActivo, @setCreated_at, @setUpdated_at) ";
                command.Parameters.AddWithValue("setId", proveedor.id);
                command.Parameters.AddWithValue("setNombre", proveedor.nombre);
                command.Parameters.AddWithValue("setDireccion", proveedor.direccion);
                command.Parameters.AddWithValue("setCorreo", proveedor.correo);
                command.Parameters.AddWithValue("setTelefono", proveedor.telefono);
                command.Parameters.AddWithValue("setDescripcion", proveedor.descripcion);
                command.Parameters.AddWithValue("setActivo", proveedor.activo);
                command.Parameters.AddWithValue("setCreated_at", proveedor.created_at);
                command.Parameters.AddWithValue("setUpdated_at", proveedor.updated_at);
                command.ExecuteScalar();
            }
        }
        public DataTable getEntradas()
        {
            DataTable dtEntradas = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = @"
        SELECT entradas.id AS ID,
               entradas.folio_factura AS 'FOLIO FACTURA',
               entradas.total_factura AS 'TOTAL FACTURA',
               usuarios.nombre AS 'USUARIO',
               sucursales.razon_social AS 'SUCURSAL',
               proveedores.nombre AS 'PROVEEDOR',
               entradas.fecha_factura AS 'FECHA FACTURA' 
        FROM entradas
        JOIN usuarios ON entradas.usuario_id = usuarios.id
        JOIN sucursales ON entradas.sucursal_id = sucursales.id
        JOIN proveedores ON entradas.proveedor_id = proveedores.id
        ORDER BY entradas.id ASC";

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dtEntradas);

            return dtEntradas;
        }
        public DataTable getEntradasPorSucursal(int sucursalId)
        {
            DataTable dtEntradas = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = @"
SELECT entradas.id AS ID,
       entradas.folio_factura AS 'FOLIO FACTURA',
       entradas.total_factura AS 'TOTAL FACTURA',
       usuarios.nombre AS 'USUARIO',
       sucursales.razon_social AS 'SUCURSAL',
       proveedores.nombre AS 'PROVEEDOR',
       entradas.fecha_factura AS 'FECHA FACTURA' 
FROM entradas
JOIN usuarios ON entradas.usuario_id = usuarios.id
JOIN sucursales ON entradas.sucursal_id = sucursales.id
JOIN proveedores ON entradas.proveedor_id = proveedores.id
WHERE sucursales.id = @sucursalId
ORDER BY entradas.id DESC";

            command.Parameters.AddWithValue("@sucursalId", sucursalId);

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dtEntradas);

            return dtEntradas;
        }
        public DataTable getSalidasPorSucursal(int sucursalId, int offset, int rowsPerPage)
        {
            DataTable dtSalidas = new DataTable();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
        SELECT salidas_temporal.id AS ID,
               origen.razon_social AS 'SUCURSAL ORIGEN',
               destino.razon_social AS 'SUCURSAL DESTINO',
               salidas_temporal.folio AS 'FOLIO',
               salidas_temporal.fecha_salida AS 'FECHA SALIDA',
               usuarios.nombre AS 'USUARIO',
               salidas_temporal.total_importe AS 'TOTAL IMPORTE'
        FROM salidas_temporal
        JOIN usuarios ON salidas_temporal.usuario_id = usuarios.id
        JOIN sucursales AS origen ON salidas_temporal.id_sucursal_origen = origen.id
        JOIN sucursales AS destino ON salidas_temporal.id_sucursal_destino = destino.id
        WHERE salidas_temporal.id_sucursal_origen = @sucursalId
        ORDER BY salidas_temporal.fecha_salida DESC
                LIMIT @rowsPerPage OFFSET @offset";

                command.Parameters.AddWithValue("@sucursalId", sucursalId);
                command.Parameters.AddWithValue("@rowsPerPage", rowsPerPage);
                command.Parameters.AddWithValue("@offset", offset);

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                {
                    adapter.Fill(dtSalidas);
                }
            }
            return dtSalidas;
        }
        public DataTable getProductoEntradaInfo(int entradaId)
        {
            DataTable dtProductoEntrada = new DataTable();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
SELECT 
    producto_entrada.entrada_id AS 'ENTRADA ID',
    producto_entrada.producto_id AS 'PRODUCTO ID',
    producto_entrada.cantidad AS 'CANTIDAD',
    productos.codigo AS 'CÓDIGO',
    productos.nombre AS 'NOMBRE',
    categorias.nombre AS 'CATEGORÍA',
    productos.presentacion AS 'PRESENTACIÓN'
FROM 
    producto_entrada
JOIN 
    productos ON producto_entrada.producto_id = productos.id
JOIN 
    categorias ON productos.categoria_id = categorias.id
WHERE 
    producto_entrada.entrada_id = @entradaId";

                command.Parameters.AddWithValue("@entradaId", entradaId);

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                {
                    adapter.Fill(dtProductoEntrada);
                }
            }
            return dtProductoEntrada;
        }
        public DataTable getProductosFromSalida(int salidaId)
        {
            DataTable dtProductos = new DataTable();
            dtProductos.Columns.Add("ID PRODUCTO", typeof(int));
            dtProductos.Columns.Add("NOMBRE", typeof(string));
            dtProductos.Columns.Add("CATEGORÍA", typeof(string));
            dtProductos.Columns.Add("PRECIO", typeof(decimal));
            dtProductos.Columns.Add("CANTIDAD", typeof(int));

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT productos FROM salidas_temporal WHERE id = @salidaId";
                command.Parameters.AddWithValue("@salidaId", salidaId);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string productosJson = reader.GetString(0);

                        var productos = JsonConvert.DeserializeObject<List<dynamic>>(productosJson);

                        foreach (var producto in productos)
                        {
                            string nombreProducto = "";
                            string categoriaProducto = "";

                            using (SQLiteCommand productCommand = connection.CreateCommand())
                            {
                                productCommand.CommandText = @"
                        SELECT productos.nombre, categorias.nombre 
                        FROM productos 
                        JOIN categorias ON productos.categoria_id = categorias.id 
                        WHERE productos.id = @productoId";
                                productCommand.Parameters.AddWithValue("@productoId", (int)producto.idproducto);

                                using (SQLiteDataReader productReader = productCommand.ExecuteReader())
                                {
                                    if (productReader.Read())
                                    {
                                        nombreProducto = productReader.GetString(0);
                                        categoriaProducto = productReader.GetString(1);
                                    }
                                }
                            }
                            DataRow row = dtProductos.NewRow();
                            row["ID PRODUCTO"] = (int)producto.idproducto;
                            row["NOMBRE"] = nombreProducto;
                            row["CATEGORÍA"] = categoriaProducto;
                            row["PRECIO"] = (decimal)producto.precio;
                            row["CANTIDAD"] = (int)producto.cantidad;
                            dtProductos.Rows.Add(row);
                        }
                    }
                }
            }

            return dtProductos;
        }


        public void saveEntradaProductos(List<EntradaProducto> entradaProductos)
        {
            foreach (EntradaProducto entradaProducto in entradaProductos)
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT OR REPLACE INTO producto_entrada (entrada_id, producto_id, codigo, cantidad, costo, estado, detalles) " +
                                      "VALUES(@setEntradaId, @setProductoId, 1, @setCantidad, @setCosto, 1, 'Enviado')";
                command.Parameters.AddWithValue("setEntradaId", entradaProducto.entrada_id);
                command.Parameters.AddWithValue("setProductoId", entradaProducto.producto_id);
                command.Parameters.AddWithValue("setCantidad", entradaProducto.cantidad);
                command.Parameters.AddWithValue("setCosto", entradaProducto.costo);
                command.ExecuteNonQuery();
            }
        }


        public void saveEntradas(List<Entrada> entradas)
        {
            foreach (Entrada entrada in entradas)
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT OR REPLACE INTO entradas (id, folio_factura, total_factura, fecha_factura, usuario_id, sucursal_id, proveedor_id, created_at, updated_at) " +
                "VALUES(@setId, @setFolioFactura, @setTotalFactura, @setFechaFactura, @setUsuarioId, @setSucursalId, @setProveedorId, @setCreatedAt, @setUpdatedAt)";

                command.Parameters.AddWithValue("setId", entrada.id);
                command.Parameters.AddWithValue("setFolioFactura", entrada.folio_factura);
                command.Parameters.AddWithValue("setTotalFactura", entrada.total_factura);
                command.Parameters.AddWithValue("setFechaFactura", entrada.fecha_factura);
                command.Parameters.AddWithValue("setUsuarioId", entrada.usuario_id);
                command.Parameters.AddWithValue("setSucursalId", entrada.sucursal_id);
                command.Parameters.AddWithValue("setProveedorId", entrada.proveedor_id);
                string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                command.Parameters.AddWithValue("setCreatedAt", currentDateTime);
                command.Parameters.AddWithValue("setUpdatedAt", currentDateTime);

                command.ExecuteNonQuery();
            }
        }
        public void saveSalidas(List<Salida> salidas)
        {
            foreach (Salida salida in salidas)
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT OR REPLACE INTO salidas_temporal (id_sucursal_origen, id_sucursal_destino, productos, folio, fecha_salida, usuario_id, total_importe, created_at, updated_at) " +
                "VALUES(@setIdSucursalOrigen, @setIdSucursalDestino, @setProductos, @setFolio, @setFechaSalida, @setUsuarioId, @setTotalImporte, @setCreatedAt, @setUpdatedAt)";
                command.Parameters.AddWithValue("setIdSucursalOrigen", salida.id_sucursal_origen);
                command.Parameters.AddWithValue("setIdSucursalDestino", salida.id_sucursal_destino);
                command.Parameters.AddWithValue("setProductos", salida.productos);
                command.Parameters.AddWithValue("setFolio", salida.folio);
                command.Parameters.AddWithValue("setFechaSalida", salida.fecha_salida);
                command.Parameters.AddWithValue("setUsuarioId", salida.usuario_id);
                command.Parameters.AddWithValue("setTotalImporte", salida.total_importe);
                command.Parameters.AddWithValue("setCreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("setUpdatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
            }
        }



        public void saveProductos(List<Producto> productos)
        {
            foreach (Producto producto in productos)
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT OR REPLACE INTO productos (id, codigo, nombre, presentacion, iva, menudeo, mayoreo, cantidad_mayoreo, especial, vendedor, imagen, activo, created_at, updated_at, medida_id, categoria_id) " +
                "VALUES(@setId, @setCodigo, @setNombre, @setPresentacion, @setIva, @setMenudeo, @setMayoreo, @setCantidad_mayoreo, @setEspecial, @setVendedor, @setImagen, @setActivo, @setCreated_at, @setUpdated_at, @setMedida_id, @setCategoria_id) ";
                command.Parameters.AddWithValue("setId", producto.id);
                command.Parameters.AddWithValue("setCodigo", producto.codigo);
                command.Parameters.AddWithValue("setNombre", producto.nombre);
                command.Parameters.AddWithValue("setPresentacion", producto.presentacion);
                command.Parameters.AddWithValue("setIva", producto.iva);
                command.Parameters.AddWithValue("setMenudeo", producto.menudeo);
                command.Parameters.AddWithValue("setMayoreo", producto.mayoreo);
                command.Parameters.AddWithValue("setCantidad_mayoreo", producto.cantidad_mayoreo);
                command.Parameters.AddWithValue("setEspecial", producto.especial);
                command.Parameters.AddWithValue("setVendedor", producto.vendedor);
                command.Parameters.AddWithValue("setImagen", producto.imagen);
                command.Parameters.AddWithValue("setActivo", producto.activo);
                command.Parameters.AddWithValue("setCreated_at", producto.created_at);
                command.Parameters.AddWithValue("setUpdated_at", producto.updated_at);
                command.Parameters.AddWithValue("setMedida_id", producto.medida_id);
                command.Parameters.AddWithValue("setCategoria_id", producto.categoria_id);
                command.ExecuteScalar();
            }
        }
        public void saveUsuarios(List<Usuario> usuarios)
        {
            foreach (Usuario usuario in usuarios)
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT OR REPLACE INTO [usuarios] (id, nombre, correo, confirmacion, telefono, imagen, usuario, clave, is_root, activo, created_at, updated_at) " +
                "VALUES(@setId, @setNombre, @setCorreo, @setConfirmacion, @setTelefono, @setImagen, @setUsuario, @setClave, @setIs_root, @setActivo, @setCreated_at, @setUpdated_at) ";
                command.Parameters.AddWithValue("setId", usuario.id);
                command.Parameters.AddWithValue("setNombre", usuario.nombre);
                command.Parameters.AddWithValue("setCorreo", usuario.correo);
                command.Parameters.AddWithValue("setConfirmacion", usuario.confirmacion);
                command.Parameters.AddWithValue("setTelefono", usuario.telefono);
                command.Parameters.AddWithValue("setImagen", usuario.imagen);
                command.Parameters.AddWithValue("setUsuario", usuario.usuario);
                command.Parameters.AddWithValue("setClave", usuario.clave);
                command.Parameters.AddWithValue("setIs_root", usuario.es_raiz);
                command.Parameters.AddWithValue("setActivo", usuario.activo);
                command.Parameters.AddWithValue("setCreated_at", usuario.created_at);
                command.Parameters.AddWithValue("setUpdated_at", usuario.updated_at);
                command.ExecuteScalar();
            }
        }
        //public void saveOperaciones(List<Operacion> operaciones)
        //{
        //    foreach (Operacion operacion in operaciones)
        //    {
        //        SQLiteCommand command = connection.CreateCommand();
        //        command.CommandText = "INSERT OR REPLACE INTO operaciones (id, accion, confirmar, created_at, updated_at, producto_id, usuario_id) " +
        //        "VALUES(@setId, @setAccion, @setConfirmar, @setCreated_at, @setUpdated_at, @setProducto_id, @setUsuario_id) ";
        //        command.Parameters.AddWithValue("setId", operacion.id);
        //        command.Parameters.AddWithValue("setAccion", operacion.accion);
        //        command.Parameters.AddWithValue("setConfirmar", operacion.confirmar);
        //        command.Parameters.AddWithValue("setCreated_at", operacion.created_at);
        //        command.Parameters.AddWithValue("setUpdated_at", operacion.updated_at);
        //        command.Parameters.AddWithValue("setProducto_id", operacion.producto_id);
        //        command.Parameters.AddWithValue("setUsuario_id", operacion.usuario_id);
        //        command.ExecuteScalar();
        //    }
        //}
        public void saveSucursales(List<Sucursal> sucursales)
        {
            foreach (Sucursal sucursal in sucursales)
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT OR REPLACE INTO sucursales (id, puerta_enlace1, puerta_enlace2, puerta_enlace3, puerta_enlace4, razon_social, direccion, correo, activo, created_at, updated_at) " +
                "VALUES(@setId, @setEnlace1, @setEnlace2, @setEnlace3, @setEnlace4, @setRazon, @setDireccion, @setCorreo, @setActivo, @setCreated_at, @setUpdated_at) ";
                command.Parameters.AddWithValue("setId", sucursal.id);
                command.Parameters.AddWithValue("setEnlace1", sucursal.puerta_enlace1);
                command.Parameters.AddWithValue("setEnlace2", sucursal.puerta_enlace2);
                command.Parameters.AddWithValue("setEnlace3", sucursal.puerta_enlace3);
                command.Parameters.AddWithValue("setEnlace4", sucursal.puerta_enlace4);
                command.Parameters.AddWithValue("setRazon", sucursal.razon_social);
                command.Parameters.AddWithValue("setDireccion", sucursal.direccion);
                command.Parameters.AddWithValue("setCorreo", sucursal.correo);
                command.Parameters.AddWithValue("setActivo", sucursal.activo);
                command.Parameters.AddWithValue("setCreated_at", sucursal.created_at);
                command.Parameters.AddWithValue("setUpdated_at", sucursal.updated_at);
                command.ExecuteScalar();
            }
        }
        public int CrearVenta(Dictionary<string, object>venta, List<ProductoVenta> productos)
        {
            int id = 0;
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO ventas (total, fecha_venta, metodo_pago, tipo, sucursal_id, cancelacion, estado, detalles) " +
            "VALUES(@setTotal, @setFecha, @setMetodo, @setTipo, @setSucursal_id, @setCancelacion, @setEstado, @setDetalles) ";
            command.Parameters.AddWithValue("setTotal", venta["total"].ToString());
            command.Parameters.AddWithValue("setFecha", venta["fecha_venta"].ToString());
            command.Parameters.AddWithValue("setMetodo",  JsonConvert.SerializeObject( venta["metodo_pago"]));
            command.Parameters.AddWithValue("setTipo", venta["tipo"].ToString());
            command.Parameters.AddWithValue("setSucursal_id", venta["sucursal_id"].ToString());
            command.Parameters.AddWithValue("setCancelacion", 0);
            command.Parameters.AddWithValue("setEstado", 1);
            command.Parameters.AddWithValue("setDetalles", "Pendiente de envío");
            command.ExecuteScalar();
            command.CommandText = "select last_insert_rowid()";
            Int64 LastRowID64 = (Int64)command.ExecuteScalar();
           id= (int)LastRowID64;
            command.CommandText = "UPDATE ventas SET folio=@setFolio WHERE id = @setId";
            command.Parameters.AddWithValue("setFolio", venta["folio"].ToString() + (id%10000).ToString().PadLeft(4,'0'));
            command.Parameters.AddWithValue("setId", id);
            command.ExecuteScalar();
            foreach (ProductoVenta p in productos)
            {
                command.CommandText = "INSERT INTO producto_venta (venta_id, producto_id, codigo, cantidad, precio_venta, estado, detalles)" +
                    "VALUES (@setVenta, @setProducto, @setCodigo, @setCantidad, @setPrecio, @setEstado, @setDetalles)";
                command.Parameters.AddWithValue("setVenta", id);
                command.Parameters.AddWithValue("setProducto", p.id);
                command.Parameters.AddWithValue("setCodigo", p.codigo);
                command.Parameters.AddWithValue("setCantidad", p.cantidad);
                command.Parameters.AddWithValue("setPrecio", p.precio_venta);
                command.Parameters.AddWithValue("setEstado", 1);
                command.Parameters.AddWithValue("setDetalles", "Pendiente de envío");
                command.ExecuteScalar();
            }
            return id;
        }
        public Producto GetProductByIdOrCode(string identifier)
        {
            Producto producto = null;
            SQLiteCommand command = connection.CreateCommand();

            // Check if the identifier is numeric (assuming it's the ID).
            int productId;
            if (int.TryParse(identifier, out productId))
            {
                command.CommandText = "SELECT * FROM productos WHERE activo = 1 AND id = @setId";
                command.Parameters.AddWithValue("setId", productId);
            }
            else
            {
                // If it's not numeric, assume it's the code.
                command.CommandText = "SELECT * FROM productos WHERE activo = 1 AND codigo = @setCode";
                command.Parameters.AddWithValue("setCode", identifier);
            }

            SQLiteDataReader result = command.ExecuteReader();

            if (result.Read())
            {
                producto = new Producto();

                producto.id = result.GetInt32(0);
                producto.codigo = result.GetString(1);
                producto.nombre = result.GetString(2);
                producto.presentacion = result.IsDBNull(3) ? "" : result.GetString(3);
                producto.iva = result.GetFloat(4);
                producto.menudeo = result.GetFloat(5);
                producto.mayoreo = result.GetFloat(6);
                producto.cantidad_mayoreo = result.GetInt32(7);
                producto.especial = result.GetFloat(8);
                producto.vendedor = result.GetFloat(9);
                producto.imagen = result.IsDBNull(10) ? "" : result.GetString(10);
                producto.activo = result.GetInt32(11);
                producto.created_at = result.GetString(12);
                producto.updated_at = result.GetString(13);
                producto.medida_id = result.GetInt32(14);
                producto.categoria_id = result.GetInt32(15);
            }
            else
            {
                // Reset command and perform the query again.
                command.Reset();

                // Check if the identifier is numeric (assuming it's the ID).
                if (int.TryParse(identifier, out productId))
                {
                    command.CommandText = "SELECT * FROM productos WHERE activo = 1 AND id = @setId";
                    command.Parameters.AddWithValue("setId", productId);
                }
                else
                {
                    // If it's not numeric, assume it's the code.
                    command.CommandText = "SELECT * FROM productos WHERE activo = 1 AND codigo = @setCode";
                    command.Parameters.AddWithValue("setCode", identifier);
                }

                result = command.ExecuteReader();

                if (result.Read())
                {
                    producto = new Producto();

                    producto.id = 0;
                    producto.codigo = result.GetString(1);
                    producto.nombre = result.GetString(2);
                    producto.presentacion = result.IsDBNull(3) ? "" : result.GetString(3);
                    producto.menudeo = result.GetFloat(4);
                    producto.mayoreo = result.GetFloat(5);
                    producto.cantidad_mayoreo = result.GetInt32(6);
                    producto.especial = result.GetFloat(7);
                    producto.vendedor = result.GetFloat(8);
                    producto.imagen = "";
                    producto.medida_id = result.GetInt32(9);
                    producto.categoria_id = result.GetInt32(10);
                }
            }

            return producto;
        }

        public Producto GetProductByCode(string code)
        {
            Producto producto= null;
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM productos WHERE activo=1 AND codigo = @setCode";
            command.Parameters.AddWithValue("setCode", code);
            SQLiteDataReader result = command.ExecuteReader();
            if (result.Read())
            {
                producto = new Producto();

                producto.id = result.GetInt32(0);
                producto.codigo = result.GetString(1);
                producto.nombre = result.GetString(2);
                producto.presentacion = result.IsDBNull(3) ? "" : result.GetString(3);
                producto.iva = result.GetFloat(4);
                producto.menudeo = result.GetFloat(5);
                producto.mayoreo = result.GetFloat(6);
                producto.cantidad_mayoreo = result.GetInt32(7);
                producto.especial = result.GetFloat(8);
                producto.vendedor = result.GetFloat(9);
                producto.imagen = result.IsDBNull(10)?"":result.GetString(10);
                producto.activo = result.GetInt32(11);
                producto.created_at = result.GetString(12);
                producto.updated_at = result.GetString(13);
                producto.medida_id = result.GetInt32(14);
                producto.categoria_id = result.GetInt32(15);

            }
            else
            {
                command.Reset();
                command.CommandText = "SELECT * FROM productos WHERE activo=1 AND codigo = @setCode";
                command.Parameters.AddWithValue("setCode", code);
                result = command.ExecuteReader();
                if (result.Read())
                {
                    producto = new Producto();

                    producto.id = 0;
                    producto.codigo = result.GetString(1);
                    producto.nombre = result.GetString(2);
                    producto.presentacion = result.IsDBNull(3) ? "" : result.GetString(3);
                    producto.menudeo = result.GetFloat(4);
                    producto.mayoreo = result.GetFloat(5);
                    producto.cantidad_mayoreo = result.GetInt32(6);
                    producto.especial = result.GetFloat(7);
                    producto.vendedor = result.GetFloat(8);
                    producto.imagen =  "";
                    producto.medida_id = result.GetInt32(9);
                    producto.categoria_id = result.GetInt32(10);

                }
            }
            return producto;
        }
        public void altaTemporal(List<NuevoProducto> productos)
        {
            foreach (NuevoProducto producto in productos)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT OR REPLACE INTO alta_temporal (codigo, nombre, presentacion, menudeo, mayoreo, cantidad_mayoreo, especial, vendedor, medida_id, categoria_id, estado, detalles) " +
                "VALUES(@setCodigo, @setNombre, @setPresentacion, @setMenudeo, @setMayoreo, @setCantidad_mayoreo, @setEspecial, @setVendedor, @setMedida_id, @setCategoria_id, @setEstado, @setDetalles) ";
                command.Parameters.AddWithValue("setCodigo", producto.codigo);
                command.Parameters.AddWithValue("setNombre", producto.nombre);
                command.Parameters.AddWithValue("setPresentacion", producto.presentacion);
                command.Parameters.AddWithValue("setMenudeo", producto.menudeo);
                command.Parameters.AddWithValue("setMayoreo", producto.mayoreo);
                command.Parameters.AddWithValue("setCantidad_mayoreo", producto.cantidad_mayoreo);
                command.Parameters.AddWithValue("setEspecial", producto.especial);
                command.Parameters.AddWithValue("setVendedor", producto.vendedor);
                command.Parameters.AddWithValue("setMedida_id", producto.medida_id);
                command.Parameters.AddWithValue("setCategoria_id", producto.categoria_id);
                command.Parameters.AddWithValue("setEstado", 1);
                command.Parameters.AddWithValue("setDetalles", "Pendiente de envío");
                command.ExecuteScalar();
            }
        }
        public void changeEstadoVenta(int id, int estado, string detalles)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE ventas SET estado = @setEstado, detalles = @setDetalles WHERE id = @setId";
            command.Parameters.AddWithValue("setEstado", estado);
            command.Parameters.AddWithValue("setDetalles", detalles);
            command.Parameters.AddWithValue("@setId", id);
            command.ExecuteScalar();
            command.CommandText = "UPDATE producto_venta SET estado = @setEstado, detalles = @setDetalles WHERE venta_id = @setId";
            command.Parameters.AddWithValue("setEstado", estado);
            command.Parameters.AddWithValue("setDetalles", detalles);
            command.Parameters.AddWithValue("@setId", id);
            command.ExecuteScalar();
        }
        public void changeEstadoCorte(int id, int estado, string detalles)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE cortes SET estado = @setEstado, detalles = @setDetalles WHERE id = @setId";
            command.Parameters.AddWithValue("setEstado", estado);
            command.Parameters.AddWithValue("setDetalles", detalles);
            command.Parameters.AddWithValue("@setId", id);
            command.ExecuteScalar();
            
        }
        public void changeEstadoEntrada(int id, int estado, string detalles)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE entradas SET estado = @setEstado, detalles = @setDetalles WHERE id = @setId";
            command.Parameters.AddWithValue("setEstado", estado);
            command.Parameters.AddWithValue("setDetalles", detalles);
            command.Parameters.AddWithValue("@setId", id);
            command.ExecuteScalar();
            command.CommandText = "UPDATE producto_entrada SET estado = @setEstado, detalles = @setDetalles WHERE entrada_id = @setId";
            command.Parameters.AddWithValue("setEstado", estado);
            command.Parameters.AddWithValue("setDetalles", detalles);
            command.Parameters.AddWithValue("@setId", id);
            command.ExecuteScalar();
        }
        public List<NuevoProducto> GetProductosTemporales()
        {
            List<NuevoProducto> productos = new List<NuevoProducto>();

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM alta_temporal";
            SQLiteDataReader result = command.ExecuteReader();
            while (result.Read())
            {
                productos.Add(new NuevoProducto
                {
                    codigo = result.GetString(1),
                    nombre = result.GetString(2),
                    presentacion = result.IsDBNull(3)?"": result.GetString(3),
                    menudeo = result.IsDBNull(4) ? 0 : result.GetDouble(4),
                    mayoreo = result.IsDBNull(5) ? 0 : result.GetDouble(5),
                    cantidad_mayoreo = result.IsDBNull(6) ? 0 : result.GetInt32(6),
                    especial = result.IsDBNull(7) ? 0 : result.GetDouble(7),
                    vendedor = result.IsDBNull(8) ? 0 : result.GetDouble(8),
                    imagen = "...",
                    medida_id = result.IsDBNull(9) ? 0 : result.GetInt32(9),
                    categoria_id = result.IsDBNull(10) ? 0 : result.GetInt32(10),

                });
                ;
            }
            return productos;
        }
        public void reconnectProductosVenta(List<NuevoProducto>productos)
        {
            foreach( NuevoProducto p in productos)
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT id from productos WHERE codigo = @setCodigo";
                command.Parameters.AddWithValue("setCodigo", p.codigo);
                SQLiteDataReader result = command.ExecuteReader();
                if (result.Read())
                {
                    int id = result.GetInt32(0);
                    SQLiteCommand command2 = connection.CreateCommand();
                    command2.CommandText = "UPDATE producto_venta SET producto_id = @setId WHERE codigo = @setCodigo";
                    command2.Parameters.AddWithValue("setId", id);
                    command2.Parameters.AddWithValue("setCodigo", p.codigo);
                    command2.ExecuteScalar();
                }
            }
        }
        public void reconnectProductosEntrada(List<NuevoProducto> productos)
        {
            foreach (NuevoProducto p in productos)
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT id from productos WHERE codigo = @setCodigo";
                command.Parameters.AddWithValue("setCodigo", p.codigo);
                SQLiteDataReader result = command.ExecuteReader();
                if (result.Read())
                {
                    int id = result.GetInt32(0);
                    SQLiteCommand command2 = connection.CreateCommand();
                    command2.CommandText = "UPDATE producto_entrada SET producto_id = @setId WHERE codigo = @setCodigo";
                    command2.Parameters.AddWithValue("setId", id);
                    command2.Parameters.AddWithValue("setCodigo", p.codigo);
                    command2.ExecuteScalar();
                }
            }
        }
        public bool hasTemporal(int id, string tabla)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM @setTabla WHERE venta_id = @setVenta AND producto_id = @setProducto";
            command.Parameters.AddWithValue("setTabla", tabla);
            command.Parameters.AddWithValue("setVenta", id);
            command.Parameters.AddWithValue("setProducto", 0);
            SQLiteDataReader result = command.ExecuteReader();
            if (result.Read())
            {
                return true;
            }
            return false;
        }
        public List<ProductoVenta> getCarrito(int idVenta)
        {
            List<ProductoVenta> productos = new List<ProductoVenta>();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM producto_venta WHERE venta_id = @setVenta";
            command.Parameters.AddWithValue("setVenta", idVenta);
            SQLiteDataReader result = command.ExecuteReader();
            while (result.Read())
            {
                productos.Add(new ProductoVenta
                {
                    id=result.GetInt32(2),
                    codigo= result.GetString(3),
                    cantidad = result.GetInt32(4),
                    precio_venta = result.GetDouble(5)
                });
            }
            return productos;
        }
        public List<ProductoEntrada> getListaEntrada(int idEntrada)
        {
            List<ProductoEntrada> productos = new List<ProductoEntrada>();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM producto_entrada WHERE entrada_id = @setEntrada";
            command.Parameters.AddWithValue("setEntrada", idEntrada);
            SQLiteDataReader result = command.ExecuteReader();
            while (result.Read())
            {
                productos.Add(new ProductoEntrada
                {
                    id = result.GetInt32(2),
                    codigo = result.GetString(3),
                    cantidad = result.GetInt32(4),
                    costo = result.GetDouble(5)
                });
            }
            return productos;
        }
        public void clearAltaTemporal()
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM alta_temporal";
            command.ExecuteScalar();
        }
        public int registrarEntrada(Dictionary<string, object> entrada, List<ProductoEntrada> productos)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO entradas (fecha_factura, total_factura, folio_factura, usuario_id, sucursal_id, proveedor_id, cancelacion, estado, detalles)" +
                "VALUES (@setFecha, @setTotal, @setFolio, @setUsuario, @setSucursal, @setProveedor, @setCancelacion, @setEstado, @setDetalles)";
            command.Parameters.AddWithValue("setFecha", entrada["fecha_factura"].ToString());
            command.Parameters.AddWithValue("setTotal", entrada["total_factura"].ToString());
            command.Parameters.AddWithValue("setFolio", entrada["folio_factura"].ToString());
            command.Parameters.AddWithValue("setUsuario", entrada["usuario_id"].ToString());
            command.Parameters.AddWithValue("setSucursal", entrada["sucursal_id"].ToString());
            command.Parameters.AddWithValue("setProveedor", entrada["proveedor_id"].ToString());
            command.Parameters.AddWithValue("setCancelacion", 0);
            command.Parameters.AddWithValue("setEstado", 1);
            command.Parameters.AddWithValue("setDetalles", "Pendiente de envío");
            command.ExecuteScalar();
            command.CommandText = "select last_insert_rowid()";
            Int64 LastRowID64 = (Int64)command.ExecuteScalar();
            int id = (int)LastRowID64;
            foreach (ProductoEntrada p in productos)
            {
                command.CommandText = "INSERT INTO producto_entrada (entrada_id, producto_id, codigo, cantidad, costo, estado, detalles)" +
                    "VALUES (@setEntrada, @setProducto, @setCodigo, @setCantidad, @setCosto, @setEstado, @setDetalles)";
                command.Parameters.AddWithValue("setEntrada", id);
                command.Parameters.AddWithValue("setProducto", p.id);
                command.Parameters.AddWithValue("setCodigo", p.codigo);
                command.Parameters.AddWithValue("setCosto", p.costo);
                command.Parameters.AddWithValue("setCantidad", p.cantidad);
                command.Parameters.AddWithValue("setEstado", 1);
                command.Parameters.AddWithValue("setDetalles", "Pendiente de envío");
                command.ExecuteScalar();
            }
            return id;
        }
        public int nuevaSalida(int idsucursal)
        {
            int response =0;
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id FROM salidas_temporal WHERE estado = 0 ORDER BY id DESC LIMIT 1";
            SQLiteDataReader result = command.ExecuteReader();
            if (result.Read())
            {
                response = result.GetInt32(0);
            }
            else
            {
                command.Reset();
                command.CommandText = "INSERT INTO salidas_temporal (id_sucursal_destino, estado)" +
                    "VALUES(@setSucursal, @setEstado)";
                command.Parameters.AddWithValue("setSucursal", idsucursal);
                command.Parameters.AddWithValue("setEstado", 0);
                command.ExecuteNonQuery();
                command.CommandText = "select last_insert_rowid()";
                Int64 LastRowID64 = (Int64)command.ExecuteScalar();
                response = (int)LastRowID64;
            }
            return response;
        }
        public int getCajaAbierta()
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM cortes ORDER BY id DESC LIMIT 1";
            SQLiteDataReader result = command.ExecuteReader();
            if (result.Read())
            {
                int estado = result.GetInt32(15);
                int id = result.GetInt32(0);
                if (estado == 0)
                {
                    return id;
                }
            }
            return -1;
        }
        public int crearCorte(double apertura)
        {
            DateTime localDate = DateTime.Now;
            int id;
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO cortes (fondo_apertura, total_efectivo, total_debito, total_credito, total_cheques, total_transferencias, gastos, ingresos, sobrante, fecha_apertura, estado, detalles)" +
                "values(@setApertura, 0, 0, 0, 0, 0, '{}', '{}', 0, @setFecha, 0, 'Abierta')";
            command.Parameters.AddWithValue("setApertura", apertura);
            command.Parameters.AddWithValue("setFecha", localDate.Year + "-" + localDate.Month.ToString().PadLeft(2, '0') + "-" + localDate.Day.ToString().PadLeft(2, '0'));
            command.ExecuteScalar();
            command.CommandText = "select last_insert_rowid()";
            Int64 LastRowID64 = (Int64)command.ExecuteScalar();
            id = (int)LastRowID64;
            return id;
        }
        public void completarCorte(int idcorte, int idsucursal, int idusuario)
        {            
            DateTime localDate = DateTime.Now;
            string folio = idsucursal.ToString().PadLeft(2, '0') + localDate.Day.ToString().PadLeft(2, '0') + localDate.Month.ToString().PadLeft(2, '0') + localDate.Year + idcorte.ToString().PadLeft(4, '0');
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE cortes SET folio = @setFolio, fecha_corte = @setFecha, sucursal_id = @setSucursal, usuario_id = @setUsuario, estado = 1, detalles = @setDetalles WHERE id = @setId";
            command.Parameters.AddWithValue("setFolio", folio);
            command.Parameters.AddWithValue("setId", idcorte);
            command.Parameters.AddWithValue("setFecha", localDate.Year + "-" + localDate.Month.ToString().PadLeft(2, '0') + "-" + localDate.Day.ToString().PadLeft(2, '0'));
            command.Parameters.AddWithValue("setSucursal", idsucursal);
            command.Parameters.AddWithValue("setUsuario", idusuario);
            command.Parameters.AddWithValue("setDetalles", "Pendiente de envío");
            command.ExecuteNonQuery();
        }
        public Dictionary<string, string> getCorte(int id)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM cortes WHERE id = @setId";
            command.Parameters.AddWithValue("setId", id);
            SQLiteDataReader result = command.ExecuteReader();
            if (result.Read())
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data["fondo_apertura"] = result.GetDouble(2).ToString("0.00");
                data["total_efectivo"] = result.GetDouble(3).ToString("0.00");
                data["folio_corte"] = result.IsDBNull(1)? "-" : result.GetString(1);
                data["total_tarjetas_debito"] = result.GetDouble(4).ToString("0.00");
                data["total_tarjetas_credito"] = result.GetDouble(5).ToString("0.00");
                data["total_cheques"] = result.GetDouble(6).ToString("0.00");
                data["total_transferencias"] = result.GetDouble(7).ToString("0.00");
                data["gastos"] = result.GetString(8);
                data["ingresos"] = result.GetString(9);
                data["sobrante"] = result.GetDouble(10).ToString("0.00");
                data["fecha_apertura_caja"] = result.GetString(11);
                data["fecha_corte_caja"] = result.IsDBNull(12) ? "-" : result.GetString(12);
                data["sucursal_id"] = result.IsDBNull(13) ? "-" : result.GetInt32(13).ToString();
                data["usuario_id"] = result.IsDBNull(14) ? "-" : result.GetInt32(14).ToString();
                return data;
            }
            return null;
        }
        public void acumularPagos (Dictionary<string, double> pagos, int idcorte)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE cortes SET total_efectivo = total_efectivo + @setEfectivo, total_debito = total_debito + @setDebito, total_credito = total_credito + @setCredito," +
                "total_cheques = total_cheques + @setCheques, total_transferencias = total_transferencias + @setTransferencias WHERE id = @setId";
            command.Parameters.AddWithValue("setId", idcorte);
            command.Parameters.AddWithValue("setEfectivo", pagos.ContainsKey("efectivo") ? pagos["efectivo"] : 0);
            command.Parameters.AddWithValue("setDebito", pagos.ContainsKey("debito") ? pagos["debito"] : 0);
            command.Parameters.AddWithValue("setCredito", pagos.ContainsKey("credito") ? pagos["credito"] : 0);
            command.Parameters.AddWithValue("setCheques", pagos.ContainsKey("cheque") ? pagos["cheque"] : 0);
            command.Parameters.AddWithValue("setTransferencias", pagos.ContainsKey("transferencia") ? pagos["transferencia"] : 0);
            command.ExecuteNonQuery();
        }
        public void registrarIngreso(string concepto, double monto, int idcorte)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT ingresos FROM cortes WHERE id = @setId";
            command.Parameters.AddWithValue("setId", idcorte);
            SQLiteDataReader result = command.ExecuteReader();
            if (result.Read())
            {
                var temp = JsonConvert.DeserializeObject<Dictionary<string, double>>(result.GetString(0));
                if (temp.ContainsKey(concepto))
                    temp[concepto] += monto;
                else
                    temp.Add(concepto, monto);
                command.Reset();
                command.CommandText = "UPDATE cortes SET total_efectivo = total_efectivo + @setMonto, ingresos = @setIngresos WHERE id = @setId";
                command.Parameters.AddWithValue("setMonto", monto);
                command.Parameters.AddWithValue("setId", idcorte);
                command.Parameters.AddWithValue("setIngresos", JsonConvert.SerializeObject(temp));
                command.ExecuteNonQuery();
            }
        }
        public void registrarGasto(string concepto, double monto, int idcorte)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT gastos FROM cortes WHERE id = @setId";
            command.Parameters.AddWithValue("setId", idcorte);
            SQLiteDataReader result = command.ExecuteReader();
            if (result.Read())
            {
                var temp = JsonConvert.DeserializeObject<Dictionary<string, double>>(result.GetString(0));
                if (temp.ContainsKey(concepto))
                    temp[concepto] += monto;
                else
                    temp.Add(concepto, monto);
                command.Reset();
                command.CommandText = "UPDATE cortes SET total_efectivo = total_efectivo - @setMonto, gastos = @setGastos WHERE id = @setId";
                command.Parameters.AddWithValue("setMonto", monto);
                command.Parameters.AddWithValue("setId", idcorte);
                command.Parameters.AddWithValue("setGastos", JsonConvert.SerializeObject(temp));
                command.ExecuteNonQuery();
            }
        }
        public bool reimprimirTicket(string folio)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT ventas.id, ventas.total, ventas.fecha_venta, producto_venta.cantidad, producto_venta.precio_venta, productos.nombre FROM ventas INNER JOIN producto_venta ON ventas.id = producto_venta.venta_id INNER JOIN productos ON producto_venta.producto_id = productos.id WHERE ventas.folio = @setFolio";
            command.Parameters.AddWithValue("setFolio", folio);
            SQLiteDataReader result = command.ExecuteReader();
            string c = "Impresora: "+impresora+"\n";
            bool printh = true;
            if (!impresora.Equals(""))
            {                
                CreaTicket Ticket1 = new CreaTicket();
                Ticket1.impresora = impresora;
                double total = 0;
                while (result.Read())
                {                    
                    if (printh)
                    {
                        Ticket1.TextoCentro("Esto si afecta");
                        Ticket1.TextoDerecha("Caja 1");
                        Ticket1.TextoExtremos("Fecha: " + result.GetString(2), "");
                        Ticket1.TextoCentro("Venta mostrador"); // imprime en el centro "Venta mostrador"
                        Ticket1.LineasGuion(); // imprime una linea de guiones
                        Ticket1.EncabezadoVenta();
                        printh = false;
                        total = result.GetDouble(1);
                    }
                    int cantidad = result.GetInt32(3);
                    double precio = result.GetDouble(4);
                    Ticket1.AgregaArticulo(result.GetString(5), cantidad, precio, cantidad * precio);
                    c += "idventa: " + result.GetInt32(0) + " total: " + result.GetDouble(1) +" Fecha: "+ result.GetString(2) + " cantidad: " + result.GetInt32(3) + " precio: " + result.GetDouble(4) + " nombre: " + result.GetString(5) + "\n";
                }
                if (printh == false)
                {
                    Ticket1.LineasTotales(); // imprime linea
                    Ticket1.AgregaTotales("Total", total); // imprime linea con total
                    Ticket1.CortaTicket(); // corta el ticket
                }
            }
            return !printh;
        }
        public void endDatabase()
        {
            connection.Close();
            connection.Dispose();
        }
        public DataTable getEntradasPorSucursal(int sucursalId, int offset, int rowsPerPage)
        {
            DataTable dtEntradas = new DataTable();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = @"
        SELECT entradas.id AS ID,
               entradas.folio_factura AS 'FOLIO FACTURA',
               entradas.total_factura AS 'TOTAL FACTURA',
               usuarios.nombre AS 'USUARIO',
               sucursales.razon_social AS 'SUCURSAL',
               proveedores.nombre AS 'PROVEEDOR',
               entradas.fecha_factura AS 'FECHA FACTURA' 
        FROM entradas
        JOIN usuarios ON entradas.usuario_id = usuarios.id
        JOIN sucursales ON entradas.sucursal_id = sucursales.id
        JOIN proveedores ON entradas.proveedor_id = proveedores.id
        WHERE sucursales.id = @sucursalId
        ORDER BY entradas.id DESC
        LIMIT @rowsPerPage OFFSET @offset";

            command.Parameters.AddWithValue("@sucursalId", sucursalId);
            command.Parameters.AddWithValue("@rowsPerPage", rowsPerPage);
            command.Parameters.AddWithValue("@offset", offset);

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dtEntradas);

            return dtEntradas;
        }

        public int getEntradasCountPorSucursal(int sucursalId)
        {
            int count = 0;
            string query = "SELECT COUNT(*) FROM entradas WHERE sucursal_id = @sucursalId";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@sucursalId", sucursalId);
                count = Convert.ToInt32(command.ExecuteScalar());
            }
            return count;
        }

        public int getSalidasCountPorSucursal(int sucursalId)
        {
            int count = 0;
            string query = "SELECT COUNT(*) FROM salidas_temporal WHERE id_sucursal_origen = @id_sucursal_origen";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id_sucursal_origen", sucursalId);
                count = Convert.ToInt32(command.ExecuteScalar());
            }
            return count;
        }

    }
}
