using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using Dapper;
using System.Security.Cryptography;


namespace Benjamin.PracticoMVC.AccesoDatos
{
    public class Usuarios : IRepositorio<Entidades.Usuarios>
    {

        string cadenaConexion = Conexiones.ObtenerCadenaConexion();


        public List<Entidades.Usuarios> ListarObjetos()
        {
            List<Entidades.Usuarios> listaUsuarios = new List<Entidades.Usuarios>();

            StringBuilder consultaSQL = new StringBuilder();

            consultaSQL.Append("SELECT ");
            consultaSQL.Append("Id, IdRol, Usuario, Nombre, Apellido, Password, PasswordSalt, FechaCreacion, Activo ");
            consultaSQL.Append("FROM Usuarios ");


            using (var connection = new SqlConnection(cadenaConexion))
            {
                listaUsuarios = connection.Query<Entidades.Usuarios>(consultaSQL.ToString()).ToList();
            }

            return listaUsuarios;
        }


        public Entidades.Usuarios ObtenerUsuario(string usuario)
        {
            StringBuilder consultaSQL = new StringBuilder();

            consultaSQL.Append("SELECT ");
            consultaSQL.Append("Id, IdRol, Usuario, Nombre, Apellido, Password, PasswordSalt, FechaCreacion, Activo ");
            consultaSQL.Append("FROM Usuarios ");
            consultaSQL.Append("WHERE Usuario = @usuarioParametro ");


            using (var connection = new SqlConnection(cadenaConexion))
            {
                var objUsuario = connection.QuerySingleOrDefault<Entidades.Usuarios>(consultaSQL.ToString(), new { usuarioParametro = usuario });


                return objUsuario;
            }

        }

        public bool ValidarPassword(string password, string passwordSaltBase, string passwordHashBase)
        {

            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(passwordSaltBase);
            var saltyPasswordBytes = new byte[saltBytes.Length + passwordBytes.Length];

            Buffer.BlockCopy(saltBytes, 0, saltyPasswordBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, saltyPasswordBytes, saltBytes.Length, passwordBytes.Length);

            string passwordHashCifrada = Convert.ToBase64String(new HMACSHA256(saltBytes).ComputeHash(saltyPasswordBytes));


            if (passwordHashCifrada == passwordHashBase)
                return true;
            else
                return false;

        }



        public void CrearObjeto(Entidades.Usuarios objEntidad)
        {
            //INSERT INTO Usuarios(IdRol, Usuario, Nombre, Apellido, Password, PasswordSalt, FechaCreacion, Activo)
            //VALUES('CLI', 'drodriguez', 'David', 'Rodriguez', 'password', 'passwordSalt', GETDATE(), 0);

            //generamos password salt para guardar en la base
            objEntidad.PasswordSalt = GenerarPasswordSalt(objEntidad.Password);

            //generamos Password hash ya encriptada, para que solo el usuario sepa la password
            objEntidad.Password = GenerarPasswordHash(objEntidad.Password, objEntidad.PasswordSalt);


            StringBuilder consultaSQL = new StringBuilder();

            consultaSQL.Append("INSERT INTO Usuarios(IdRol, Usuario, Nombre, Apellido, Password, PasswordSalt, FechaCreacion, Activo)  ");
            consultaSQL.Append("VALUES(@IdRol, @Usuario, @Nombre, @Apellido, @Password, @PasswordSalt, @FechaCreacion, @Activo); ");

            using (var connection = new SqlConnection(cadenaConexion))
            {
                var filasAfectadas = connection.Execute(consultaSQL.ToString(),
                    new
                    {
                        IdRol = objEntidad.IdRol,
                        Usuario = objEntidad.Usuario,
                        Nombre = objEntidad.Nombre,
                        Apellido = objEntidad.Apellido,
                        Password = objEntidad.Password,
                        PasswordSalt = objEntidad.PasswordSalt,
                        FechaCreacion = DateTime.Now,
                        Activo = objEntidad.Activo
                    });


            }
        }


        public static string GenerarPasswordSalt(string password)
        {
            string passwordSalt;

            // generar un salt de 128-bit usando PRNG seguro
            byte[] salt = new byte[128 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            //convertir en string base 64 bits
            passwordSalt = Convert.ToBase64String(salt);

            return passwordSalt;
        }


        public static string GenerarPasswordHash(string password, string salt, string hashingAlgorithm = "HMACSHA256")
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);
            var saltyPasswordBytes = new byte[saltBytes.Length + passwordBytes.Length];

            Buffer.BlockCopy(saltBytes, 0, saltyPasswordBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, saltyPasswordBytes, saltBytes.Length, passwordBytes.Length);

            switch (hashingAlgorithm)
            {
                case "HMACSHA256":
                    return Convert.ToBase64String(new HMACSHA256(saltBytes).ComputeHash(saltyPasswordBytes));
                default:
                    // Supported types include: SHA1, MD5, SHA256, SHA384, SHA512
                    HashAlgorithm algorithm = HashAlgorithm.Create(hashingAlgorithm);

                    if (algorithm != null)
                    {
                        return Convert.ToBase64String(algorithm.ComputeHash(saltyPasswordBytes));
                    }

                    throw new CryptographicException("Unknown hash algorithm");
            }
        }


        public void ActualizarPassword(Entidades.Usuarios objUsuario)
        {
            /*
               UPDATE Usuarios
               SET PasswordSalt = 'CLAVE SALADA', Password = 'CLAVE'
               WHERE Usuario LIKE 'mperez';
            */

            //generamos password salt para guardar en la base
            objUsuario.PasswordSalt = GenerarPasswordSalt(objUsuario.Password);

            //generamos Password hash ya encriptada, para que solo el usuario sepa la password
            objUsuario.Password = GenerarPasswordHash(objUsuario.Password, objUsuario.PasswordSalt);


            StringBuilder consultaSQL = new StringBuilder();

            consultaSQL.Append("UPDATE Usuarios ");
            consultaSQL.Append("SET PasswordSalt = @PasswordSalt, Password = @Password ");
            consultaSQL.Append("WHERE Usuario LIKE @Usuario ;");

            using (var connection = new SqlConnection(cadenaConexion))
            {
                var filasAfectadas = connection.Execute(consultaSQL.ToString(),
                    new
                    {
                        Usuario = objUsuario.Usuario,
                        Password = objUsuario.Password,
                        PasswordSalt = objUsuario.PasswordSalt
                    });


            }
        }














        public bool ConfirmarEliminacionObjeto(object id)
        {
            throw new NotImplementedException();
        }


        public void Desechar()
        {
            throw new NotImplementedException();
        }

        public void EditarObjeto(Entidades.Usuarios objEntidad)
        {
            throw new NotImplementedException();
        }

        public void EliminarObjeto(object id)
        {
            throw new NotImplementedException();
        }

        public void Guardar()
        {
            throw new NotImplementedException();
        }


        public Entidades.Usuarios ObtenerDetallesDelObjeto(object id)
        {
            throw new NotImplementedException();
        }


        private void CrearClaveHash(string password, out string passwordHash, out string passwordSalt)
        {

            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            passwordSalt = Convert.ToBase64String(salt);

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            //string passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            //    password: password,
            //    salt: salt,
            //    prf: KeyDerivationPrf.HMACSHA1,
            //    iterationCount: 10000,
            //    numBytesRequested: 256 / 8));





            //using (/*algoritmo*/)
            //{
            passwordSalt = ""; // salta
            passwordHash = ""; //clave hash en base 64
                               //}
            HMACSHA256 hmac = new HMACSHA256(salt);




        }




    }

}





