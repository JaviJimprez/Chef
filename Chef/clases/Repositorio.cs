using Chef.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;

namespace Chef.clases
{
    internal class Repositorio
    {
        public class IngredienteRecetaService
        {
            private readonly string connectionString = "tu_conexion_sql_aqui";

            public int Guardar(IngredienteReceta ingredienteReceta)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO IngredientesReceta (IngredienteId, RecetaId, Cantidad) OUTPUT INSERTED.Id VALUES (@IngredienteId, @RecetaId, @Cantidad)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IngredienteId", ingredienteReceta.IngredienteId);
                        cmd.Parameters.AddWithValue("@RecetaId", ingredienteReceta.RecetaId);
                        cmd.Parameters.AddWithValue("@Cantidad", ingredienteReceta.Cantidad);

                        int newId = (int)cmd.ExecuteScalar();
                        return newId;
                    }
                }
            }
        }

    }

}
}
