using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Chef
{
    class Usuario
    {
        public string Nombre { get; set; }
        public string Contraseña { get; private set; }

        public Usuario() { }

        public Usuario(string nombre, string contraseña)
        {
            Nombre = nombre;
            Contraseña = GenerarHash(contraseña);
        }

        public static string GenerarHash(string contraseña)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
                StringBuilder resultado = new StringBuilder();
                foreach (byte b in bytes)
                {
                    resultado.Append(b.ToString("x2"));
                }
                return resultado.ToString();
            }
        }

        public bool ValidarContraseña(string contraseñaIngresada)
        {
            string hashIngresado = GenerarHash(contraseñaIngresada);
            return Contraseña == hashIngresado;
        }
    }
}
