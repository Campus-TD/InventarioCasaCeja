
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioCasaCeja
{

    public class Entrada
    {
        public int id { get; set; }
        public string folio_factura { get; set; }
        public double total_factura { get; set; }
        public string fecha_factura { get; set; }
        public int usuario_id { get; set; }
        public int sucursal_id { get; set; }
        public int proveedor_id { get; set; }
        public string cancelacion { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}
