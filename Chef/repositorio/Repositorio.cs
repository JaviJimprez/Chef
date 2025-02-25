using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using Chef.clases;
using Chef.models;
using WpfApp2;
using Chef.Singleton;
using System.Windows; // Asegúrate de que IngredienteReceta se encuentre aquí

namespace Chef.Data
{
    public class Repositorio
    {
        private readonly string _connectionString;

        public Repositorio() 
        {
            _connectionString = ConfigurationManager.ConnectionStrings["mariadb"].ConnectionString;
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
        public int ObtenerDatosUsuario(string nombre)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT id, nombre, contrasenia FROM usuario WHERE nombre = @nombre";
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
        /// Obtiene las recetas del usuario indicado.
        /// </summary>
        public List<Receta> ObtenerRecetasPorUsuario()
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
                    cmd.Parameters.AddWithValue("@idUsuario", UsuarioIniciado.Id);
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
                                 VALUES (@nombre, @duracion, @descripcion, @dificultad, @id_usuario_recetas);
                                 SELECT LAST_INSERT_ID();";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", receta.Nombre);
                    cmd.Parameters.AddWithValue("@duracion", receta.Tiempo);
                    cmd.Parameters.AddWithValue("@descripcion", receta.Descripcion);
                    cmd.Parameters.AddWithValue("@dificultad", receta.Dificultad);
                    cmd.Parameters.AddWithValue("@id_usuario_recetas", UsuarioIniciado.Id);

                    //cmd.ExecuteNonQuery();
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
       public int InsertarIngrediente(Ingrediente ingrediente)
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

        public void InicioSesion(String nombre, String contrasenia)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"SELECT id, nombre, contrasenia FROM usuario WHERE nombre = @nombre AND contrasenia = @contrasenia";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@contrasenia", contrasenia);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            UsuarioIniciado.Id = reader.GetInt32("id");
                            UsuarioIniciado.Nombre = reader.GetString("nombre");
                            UsuarioIniciado.Contrasenia = reader.GetString("contrasenia");
                            
                        }

                    }
                }
            }
        }

        public bool RegistrarUsuario(string nombre, string contrasenia)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO usuario (nombre, contrasenia) VALUES (@nombre, @contrasenia)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@contrasenia", contrasenia);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        public int InsertarValoracion(int recetaId, int usuarioId, double estrellas, string comentario)
        {
            if (recetaId <= 0 || usuarioId <= 0 || estrellas < 0)
                throw new ArgumentException("Los valores de recetaId, usuarioId y estrellas deben ser válidos.");

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO valoracion (id_recetas_valoracion, id_usuario_valoracion, estrellas, comentario) 
                 VALUES (@id_recetas_valoracion, @id_usuario_valoracion, @estrellas, @comentario); 
                 SELECT LAST_INSERT_ID();";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id_recetas_valoracion", recetaId);
                    cmd.Parameters.AddWithValue("@id_usuario_valoracion", usuarioId);
                    cmd.Parameters.AddWithValue("@estrellas", estrellas);

                    // Si comentario es null, pasamos DBNull.Value
                    if (string.IsNullOrEmpty(comentario))
                        cmd.Parameters.AddWithValue("@comentario", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@comentario", comentario);

                    try
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al insertar valoración: {ex.Message}");
                        return -1; // Indicar error
                    }
                }
            }
        }

        public List<Valoracion> ObtenerValoracionesPorReceta(int recetaId)
        {
            List<Valoracion> valoraciones = new List<Valoracion>();
            if (recetaId <= 0)
                throw new ArgumentException("El ID de la receta debe ser mayor a 0.");

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"SELECT id, id_recetas_valoracion, id_usuario_valoracion, estrellas, comentario 
                 FROM valoracion WHERE id_recetas_valoracion = @id_recetas_valoracion";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id_recetas_valoracion", recetaId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            valoraciones.Add(new Valoracion
                            {
                                Id = reader.GetInt32("id"),
                                RecetaId = reader.GetInt32("id_recetas_valoracion"),
                                UsuarioId = reader.GetInt32("id_usuario_valoracion"),
                                Estrellas = reader.GetDouble("estrellas"),
                                Comentario = reader.IsDBNull(reader.GetOrdinal("comentario")) ? null : reader.GetString("comentario"),
                            });
                        }
                    }
                }
            }
            return valoraciones;
        }


        public void GuardarImagenReceta(int recetaId, string imagenBase64)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE recetas SET imagen = @imagen WHERE id = @idReceta";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@imagen", imagenBase64);
                    cmd.Parameters.AddWithValue("@idReceta", recetaId);
                    cmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine($"Imagen guardada para receta {recetaId}: {imagenBase64.Substring(0, 100)}...");
        }

        public string ObtenerImagenReceta(int recetaId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT imagen FROM recetas WHERE id = @idReceta";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idReceta", recetaId);
                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                    {
                        Console.WriteLine($"⚠️ No se encontró imagen para la receta {recetaId}");
                        return null;
                    }

                    string base64String = result.ToString();

                    // Validar si la cadena tiene al menos 100 caracteres antes de usar Substring
                    if (base64String.Length > 100)
                    {
                        Console.WriteLine($"✅ Imagen recuperada para receta {recetaId}: {base64String.Substring(0, 100)}...");
                    }
                    else
                    {
                        Console.WriteLine($"✅ Imagen recuperada para receta {recetaId}: {base64String}");
                    }

                    return base64String;
                }
            }
        }



        public List<string> ObtenerIngredientesPorReceta(int recetaId)
        {
            List<string> ingredientes = new List<string>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"SELECT i.nombre 
                         FROM ingredientesrecetas ir
                         JOIN ingredientes i ON ir.id_ingredientes_intermedia = i.id
                         WHERE ir.id_recetas_intermedia = @recetaId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@recetaId", recetaId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ingredientes.Add(reader.GetString("nombre"));
                        }
                    }
                }
            }

            return ingredientes;
        }


        public List<Paso> ObtenerPasosDeLaReceta(int recetaId)
        {
            List<Paso> pasos = new List<Paso>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"SELECT id, nombre, num_pasos, descripcion 
                         FROM pasos 
                         WHERE id_recetas_pasos = @recetaId 
                         ORDER BY id ASC";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@recetaId", recetaId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pasos.Add(new Paso(
                                reader.GetInt32("id"),
                                reader.GetString("nombre"),
                                reader.GetInt32("num_pasos"),
                                reader.GetString("descripcion"),
                                recetaId
                            ));
                        }
                    }
                }
            }

            return pasos;
        }



        public bool BorrarReceta(int recetaId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Eliminar ingredientes relacionados con la receta
                        string deleteIngredientes = "DELETE FROM ingredientesrecetas WHERE id_recetas_intermedia = @recetaId";
                        using (MySqlCommand cmd = new MySqlCommand(deleteIngredientes, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@recetaId", recetaId);
                            cmd.ExecuteNonQuery();
                        }

                        // Eliminar pasos relacionados con la receta
                        string deletePasos = "DELETE FROM pasos WHERE id_recetas_pasos = @recetaId";
                        using (MySqlCommand cmd = new MySqlCommand(deletePasos, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@recetaId", recetaId);
                            cmd.ExecuteNonQuery();
                        }

                        // Eliminar valoraciones de la receta
                        string deleteValoraciones = "DELETE FROM valoracion WHERE id_recetas_valoracion = @recetaId";
                        using (MySqlCommand cmd = new MySqlCommand(deleteValoraciones, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@recetaId", recetaId);
                            cmd.ExecuteNonQuery();
                        }

                        // Finalmente, eliminar la receta
                        string deleteReceta = "DELETE FROM recetas WHERE id = @recetaId";
                        using (MySqlCommand cmd = new MySqlCommand(deleteReceta, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@recetaId", recetaId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            // Si no se eliminó ninguna fila, la receta no existía
                            if (rowsAffected == 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error al borrar receta: " + ex.Message);
                        return false;
                    }
                }
            }
        }


        public List<string> ObtenerTodosLosIngredientes()
        {
            List<string> ingredientes = new List<string>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT nombre FROM ingredientes";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ingredientes.Add(reader.GetString("nombre"));
                        }
                    }
                }
            }

            return ingredientes;
        }


        public bool InsertarIngrediente2(Ingrediente ingrediente)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO ingredientes (nombre) VALUES (@nombre)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", ingrediente.Nombre);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }








    }
}
