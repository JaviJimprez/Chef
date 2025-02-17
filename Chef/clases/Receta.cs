namespace Chef.models
{
    public class Receta
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Tiempo { get; set; }          // Tiempo en minutos
        public string Descripcion { get; set; }
        public int Dificultad { get; set; }        // Por ejemplo, de 1 a 10
        public int IdUsuarioReceta { get; set; }   // Id del usuario que creó la receta
        public string Imagen { get; set; }
    }
}
