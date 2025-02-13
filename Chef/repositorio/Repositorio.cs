using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using Chef.clases; // Asegúrate de que IngredienteReceta se encuentre aquí

namespace Chef.Data
{
    public class Repositorio
    {
        private readonly string _connectionString;

        public Repositorio()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MySQLPersonaje"].ConnectionString;
        }

        /// <summary>
        /// Verifica si existe un usuario con el nombre especificado.
        /// </summary>
        public bool UsuarioExiste(string nombre)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM usuario WHERE nombre = @nombre";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Obtiene la contraseña almacenada para el usuario indicado.
        /// </summary>
        public string ObtenerContrasenia(string nombre)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT contrasenia FROM usuario WHERE nombre = @nombre";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    object result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }

        /// <summary>
        /// Registra un nuevo usuario con el nombre y la contraseña indicados.
        /// </summary>
        public void RegistrarUsuario(string nombre, string contrasenia)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO usuario (nombre, contrasenia) VALUES (@nombre, @contrasenia)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@contrasenia", contrasenia);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Servicio para gestionar la relación Ingrediente-Receta.
        /// </summary>
        public class IngredienteRecetaService
        {
            private readonly string _connectionString;

            /// <summary>
            /// Se espera recibir la misma cadena de conexión que usa el Repositorio.
            /// </summary>
            public IngredienteRecetaService(string connectionString)
            {
                _connectionString = connectionString;
            }

            /// <summary>
            /// Guarda un registro en la tabla IngredientesReceta y devuelve el ID generado.
            /// </summary>
            public int Guardar(IngredienteReceta ingredienteReceta)
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO ingredientesrecetas (id_ingredientes_intermedia, id_recetas_intermedia, cantidad) " +
                                   "VALUES (@id_ingredientes_intermedia, @id_recetas_intermedia, @cantidad); SELECT LAST_INSERT_ID();";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_ingredientes_intermedia", ingredienteReceta.IngredienteId);
                        cmd.Parameters.AddWithValue("@id_recetas_intermedia", ingredienteReceta.RecetaId);
                        cmd.Parameters.AddWithValue("@cantidad", ingredienteReceta.Cantidad);

                        int newId = Convert.ToInt32(cmd.ExecuteScalar());
                        return newId;
                    }
                }
            }
        }
    }
}
