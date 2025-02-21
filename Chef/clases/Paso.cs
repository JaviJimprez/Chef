using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chef.clases
{
    public class Paso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double Cantidad { get; private set; }

        public Paso(string nombre, double cantidad)
        {
            Nombre = nombre;
            Cantidad = cantidad;
        }
    }
}
