using System;

namespace Chef
{
    public class Valoracion
    {
        public int Id { get; set; }
        public int RecetaId { get; set; }
        public int UsuarioId { get; set; }
        public double Estrellas { get; set; }
        public string Comentario { get; set; }

        public Valoracion() { }

        public Valoracion(int recetaId, int usuarioId, double estrellas, string comentario)
        {
            if (estrellas < 1 || estrellas > 5)
                throw new ArgumentOutOfRangeException(nameof(estrellas), "El número de estrellas debe estar entre 1 y 5.");

            RecetaId = recetaId;
            UsuarioId = usuarioId;
            Estrellas = estrellas;
            Comentario = comentario ?? string.Empty;
        }

        public override string ToString()
        {
            return $"Receta {RecetaId}, Usuario {UsuarioId}, Estrellas: {Estrellas}, Comentario: {Comentario}";
        }
    }
}
