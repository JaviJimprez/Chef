public class Paso
{
    public int Numero { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }

    public Paso(int numero, string titulo, string descripcion)
    {
        Numero = numero;
        Titulo = titulo;
        Descripcion = descripcion;
    }
}
