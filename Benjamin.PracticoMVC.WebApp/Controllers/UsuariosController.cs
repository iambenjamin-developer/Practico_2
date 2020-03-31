using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Benjamin.PracticoMVC.WebApp.Controllers
{
    public class UsuariosController : Controller
    {

        public ActionResult Login()
        {
            return View();

        }

        public ActionResult ListarUsuarios()
        {

            AccesoDatos.Usuarios objUsuario = new AccesoDatos.Usuarios();

            List<Entidades.Usuarios> _listaUsuarios = objUsuario.ListarObjetos();

            ViewBag.listaUsuarios = _listaUsuarios;


            return View();
        }


        public ActionResult ObtenerUsuarioLogin()
        {
            string mensaje = string.Empty;

            AccesoDatos.Usuarios usuarioDatos = new AccesoDatos.Usuarios();

            Entidades.Usuarios _usuario = usuarioDatos.ObtenerUsuario("drodriguez");

            if (_usuario == null)
            {
                mensaje = "Usuario Inexistente";

            }
            else
            {
                //validar contraseña
                if (usuarioDatos.ValidarPassword("policia", _usuario.PasswordSalt, _usuario.Password) == true)
                {
                    mensaje = "Contraseña Correcta";

                }
                else
                {
                    mensaje = "Contraseña Incorrecta";
                }
            }
            return View();

        }


        public ActionResult CrearUsuario()
        {
            Entidades.Usuarios objUsuario = new Entidades.Usuarios();

            objUsuario.IdRol = "CLI";
            objUsuario.Nombre = "Carlos";
            objUsuario.Apellido = "Herrera";
            objUsuario.Activo = true;
            objUsuario.Usuario = "cherrera";
            objUsuario.Password = "Soygarca666";

            AccesoDatos.Usuarios usuarioData = new AccesoDatos.Usuarios();

            usuarioData.CrearObjeto(objUsuario);

            return View();
        }

        public ActionResult ActualizarPassword()
        {
            Entidades.Usuarios objUsuario = new Entidades.Usuarios();

            objUsuario.Usuario = "drodriguez";
            objUsuario.Password = "policia";

            AccesoDatos.Usuarios usuarioData = new AccesoDatos.Usuarios();

            usuarioData.ActualizarPassword(objUsuario);

            return View();
        }

    }
}