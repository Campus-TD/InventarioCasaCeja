using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioCasaCeja
{
    public class Salida
    {
        public int id_sucursal_origen { get; set; }
        public int id_sucursal_destino { get; set; }
        public string productos { get; set; }
        public string folio { get; set; }
        public string fecha_salida { get; set; }
        public int usuario_id { get; set; }
        public double total_importe { get; set; }
    }
}
