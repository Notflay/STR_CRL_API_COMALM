using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.BL
{
    public class Sq_Query
    {
        public static readonly string get_tokenPass = "ObtenerContraseniaUsuario";
        public static readonly string get_infoUser = "ObtenerInformacionUsuario";
        public static readonly string get_infoUserId = "ObtenerInformacionUsuarioId";
        public static readonly string get_estados = "ObtenerEstados";
        public static readonly string get_proveedores = "ObtenerProveedores";
        public static readonly string get_filiales = "ObtieneFiliales";
        public static readonly string get_filial = "ObtieneFilial";
        public static readonly string get_roles = "ObtieneRoles";
        public static readonly string get_rol = "ObtieneRol";
        public static readonly string get_condicionPago = "ObtieneCondiciones";
        public static readonly string get_direccion = "ObtieneDireccion";
        public static readonly string get_items_art = "ObtieneItemsArticulo";
        public static readonly string get_items_serv = "ObtieneItemsServicio";
        public static readonly string get_dimensiones = "ObtieneDimensiones";
        public static readonly string get_dimension = "ObtieneDimension";
        public static readonly string get_proyectos = "ObtieneProyectos";
        public static readonly string get_proyecto = "ObtieneProyecto";

        // Tipo de Viaticos
        public static readonly string get_viaticos = "ObtieneTipoViaticos";
        public static readonly string get_viatico = "ObtieneTipoViatico";
        public static readonly string get_tpViaticos = "ObtieneViaticos";
        public static readonly string get_tpViatico = "ObtieneViatico";

        // Configuración
        public static readonly string get_cf = "ObtenerConfGeneral";

        // ObtenerIDSolicitudTrsl
        public static readonly string get_idSolicitudTrsl = "ObtieneIDSolicitudTraslado";

        // Solicitud CRUD
        public static readonly string post_solicitudTrsl = "CreacionSolicitudTraslado";
        public static readonly string post_solicitudTrslDt = "CreacionSolicitudTrasladoDet";
        public static readonly string upd_solicitudTrsl = "ActualizaSolicitudTraslado";
        public static readonly string upd_solicitudTrslDt = "ActualizacionSolicitudTrasladoDet";
        public static readonly string get_solicitudTrsl = "ObtenerSolicitudTraslado";
        public static readonly string get_solicitudTrslDt = "ObtenerSolicitudTrasladoDet";
        public static readonly string dlt_solicitudTrslDt = "BorrarSolicitudTrasladoDet";

        // Solicitud Rendición
        public static readonly string post_solicitudEar = "InsertaSoliRendicion";
        public static readonly string upd_solicitudEar = "ActualizarSoliRendicion";
        public static readonly string get_solicitudEar = "ObtenerSolicitudRendicion";
        public static readonly string get_solicitudEarId = "ObtieneIdSolicitudRendicion";
        public static readonly string get_lstSolicitudEar = "ListarSoliRendicion";
        public static readonly string upd_cambiarMigradaSR = "CambioMigrada";
        public static readonly string dlt_aprobadoresSr = "EliminaAprobadoresDeSolicitud";

        // Rendiciones
        public static readonly string get_rendiciones = "ListarRendiciones";

        // ObtenerAprobadores
        public static readonly string get_infoAprobadores = "ObtieneInfoAprobadores";
        public static readonly string get_aprobadores = "ObtieneAprobadores";
        public static readonly string post_insertAprobadores = "InsertaTablaAprobadoresSR";
        public static readonly string upd_cambiarEstadoSR = "CambiaEstadoSR";
        public static readonly string get_listaAprobadoresCab = "ListarAprobadoresCabecera";
        public static readonly string upd_aprobadores = "ActualizaablaAprobadoresSR";
        public static readonly string get_SerieOPRQ = "ObtenerSerieOPRQ";

        public static readonly string get_numeroRendicion = "ObtieneNumeroRendicion";

        public static readonly string get_listaAprobadoresDet = "ListarAprobadoresDetalle";
    }   
}
