using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benjamin.PracticoMVC.AccesoDatos
{
   public  interface IRepositorio<Entidad> where Entidad : class
    {

        //Lista generica de cada entidad
        List<Entidad> ListarObjetos();

        //Detalles del Objeto
        Entidad ObtenerDetallesDelObjeto(object id);

        //Crear Objeto - Insertar en tabla
        void CrearObjeto(Entidad objEntidad);

        //Editar Objeto - Actualizar tabla
        void EditarObjeto(Entidad objEntidad);

        //Eliminar Objeto - Eliminar Registro Tabla
        void EliminarObjeto(object id);

        //Eliminacion confirmada
        bool ConfirmarEliminacionObjeto(object id);

        //Guardar cambios - confirmar cambios
        void Guardar();

        // Dispose()
        void Desechar();


    }
}
