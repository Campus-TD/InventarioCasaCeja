﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioCasaCeja
{
    public class Producto
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }        
        public string presentacion { get; set; }
        public double iva { get; set; }
        public double menudeo { get; set; }
        public double mayoreo { get; set; }
        public int cantidad_mayoreo { get; set; }
        public double especial { get; set; }
        public double vendedor { get; set; }
        public string imagen { get; set; }
        public int activo { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int medida_id { get; set; }
        public int categoria_id { get; set; }

    }
}
