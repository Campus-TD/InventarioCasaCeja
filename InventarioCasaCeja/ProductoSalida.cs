using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioCasaCeja
{
    public class ProductoSalida
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string unidad { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
        public int idproducto { get; set; }       
    }
}