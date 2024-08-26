using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioCasaCeja
{
    public class EntradaProducto
    {
        public int entrada_id { get; set; }
        public int producto_id { get; set; }
        public string codigo { get; set; }
        public int cantidad { get; set; }
        public double costo { get; set; }
        public int estado { get; set; }
        public string detalles { get; set; }
    }
}