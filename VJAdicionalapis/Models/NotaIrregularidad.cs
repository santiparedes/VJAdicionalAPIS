using System;

namespace VJAdicionalapis.Models
{
    public class NotaIrregularidad
    {
        public int IdNotas { get; set; }
        public string Tienda { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
    }
} 