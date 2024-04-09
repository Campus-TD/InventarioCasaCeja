using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioCasaCeja
{
    public class CurrentData
    {
        public WebDataManager webDM { get; set; }
        public string folioCorte { get; set; }
        public Sucursal sucursal { get; set; }
        public List<ProductoVenta> carrito { get; set; }
        public double totalcarrito { get; set; }
        public int fontSize { get; set; }
        public int idCorte { get; set; }
        public int printerType { get; set; }
        public string fontName { get; set; }
        public string printerName { get; set; }
        public Dictionary<int, string> mapamedidasinv { get; set; }
        public Dictionary<string, int> mapasucursales { get; set; }

    }
}
