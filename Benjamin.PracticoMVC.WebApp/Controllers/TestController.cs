using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Benjamin.PracticoMVC.WebApp.Controllers
{
    public class TestController : Controller
    {

        public ActionResult ListarUsuarios()
        {

            AccesoDatos.Usuarios objUsuario = new AccesoDatos.Usuarios();

            List<Entidades.Usuarios> _listaUsuarios = objUsuario.ListarObjetos();

            ViewBag.listaUsuarios = _listaUsuarios;


            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TablaEjemplo()
        {
            return View();
        }


        public ActionResult Alertify()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();

        }
        

        public ActionResult FontAwesome()
        {
            return View();

        }

    }
}