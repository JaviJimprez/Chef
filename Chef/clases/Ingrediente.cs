using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chef.clases
{
    internal class Ingrediente
    {
        public string Nombre { get; set; }
       
        public Ingrediente(string nombre)
        {
            Nombre = nombre;
            
        }
    }
}
