using System;


namespace VJAdicionalapis.Models
{
    public class NotaIrregularidad
    {
        public int IdNotas { get; set; } // OK (auto-increment, ignorado en POST)
        public string Tienda { get; set; }
        public string Descripcion { get; set; }
        public string TipoIrregularidad { get; set; }
        public int IdUsuario { get; set; }
        public DateTime Fecha { get; set; } // OK (lo llena la base, no el cliente)
    }
} 