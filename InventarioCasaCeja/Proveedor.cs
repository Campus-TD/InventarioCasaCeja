using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioCasaCeja
{
    public class Proveedor
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string descripcion { get; set; }
        public int activo { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}
