namespace VJAdicionalapis.Model
{
    public class Notas
    {
        public int id_nota { get; set; }
        public string nombre_nota { get; set; }
        public string contenido { get; set; }
        public DateTime fecha_creacion { get; set; }
        public int id_usuario { get; set; }
        public int id_empleado { get; set; }
    }
}
