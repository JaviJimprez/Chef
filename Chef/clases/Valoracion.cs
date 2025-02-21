using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chef
{
    public class Valoracion
    {

        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int Estrellas { get; set; }
        public string Comentario { get; set; }

        public Valoracion() { }

        public Valoracion(int usuarioId, int estrellas, string comentario)
        {
            if (estrellas < 1 || estrellas > 5)
                throw new ArgumentOutOfRangeException(nameof(estrellas), "El número de estrellas debe estar entre 1 y 5.");

            UsuarioId = usuarioId;
            Estrellas = estrellas;
            Comentario = comentario ?? string.Empty;
        }

        public override string ToString()
        {
            return $"Usuario {UsuarioId}, Estrellas: {Estrellas}, Comentario: {Comentario}";
        }
    }
}
