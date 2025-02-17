using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using Chef.clases;
using Chef.models;
using WpfApp2; // Asegúrate de que IngredienteReceta se encuentre aquí

namespace Chef.Data
{
    public class Repositorio
    {
        private readonly string _connectionString;

        public Repositorio()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MySQLPersonaje"].ConnectionString;
        }

        public int ObtenerIdUsuario(string nombre)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT id FROM usuario WHERE nombre = @nombre";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
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
        /// Obtiene las recetas del usuario indicado.
        /// </summary>
        public List<Receta> ObtenerRecetasPorUsuario(int idUsuario)
        {
            List<Receta> recetas = new List<Receta>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"SELECT id, nombre, duracion, descripcion, dificultad, id_usuario_recetas
                                 FROM recetas 
                                 WHERE id_usuario_recetas = @idUsuario";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Receta rec = new Receta
                            {
                                Id = reader.GetInt32("id"),
                                Nombre = reader.GetString("nombre"),
                                Tiempo = reader.GetInt32("duracion"),
                                Descripcion = reader.GetString("descripcion"),
                                Dificultad = reader.GetInt32("dificultad"),
                                IdUsuarioReceta = reader.GetInt32("id_usuario_recetas"),
                            };
                            recetas.Add(rec);
                        }
                    }
                }
            }

            return recetas;
        }

        /// <summary>
        /// Inserta una nueva receta en la base de datos y retorna el id autogenerado.
        /// </summary>
        /// <param name="receta">Objeto Receta con los datos a insertar</param>
        /// <returns>Id generado por la base de datos</returns>
        public int InsertarReceta(Receta receta)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO recetas (nombre, duracion, descripcion, dificultad, id_usuario_recetas)
                                 VALUES (@nombre, @tiempo, @descripcion, @dificultad, @id_usuario_recetas);
                                 SELECT LAST_INSERT_ID();";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", receta.Nombre);
                    cmd.Parameters.AddWithValue("@duracion", receta.Tiempo);
                    cmd.Parameters.AddWithValue("@descripcion", receta.Descripcion);
                    cmd.Parameters.AddWithValue("@dificultad", receta.Dificultad);
                    cmd.Parameters.AddWithValue("@id_usuario_recetas", receta.IdUsuarioReceta);

                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    return id;
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
            /// Constructor que requiere la cadena de conexión.
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

        // Método para obtener el id de un ingrediente según su nombre
        public int ObtenerIdIngrediente(string nombre)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT id FROM ingredientes WHERE nombre = @nombre";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        // Método para insertar un nuevo ingrediente y retornar su id
        public int InsertarIngrediente(Ingredientes ingrediente)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO ingredientes (nombre) VALUES (@nombre); SELECT LAST_INSERT_ID();";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", ingrediente.Nombre);
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    return id;
                }
            }
        }

        internal int InsertarIngrediente(Ingrediente ing)
        {
            throw new NotImplementedException();
        }
    }
}
