using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chef.clases;
using System.Collections.ObjectModel;

namespace CocinaRecetas.clases
{
    

    public class Receta
    {
        public string Nombre { get; set; }
        public int Tiempo { get; set; }
        public string Imagen { get; set; }
        public string Descripcion { get; set; }
        public ObservableCollection<IngredienteReceta> Ingredientes { get; set; } = new();
    }
}
