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
        public static readonly string get_proveedor = "ObtenerProveedor";
        public static readonly string get_proveedorxruc = "ObtenerProveedorPorRuc";
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

        public static readonly string get_cuentaCo = "ObtenerCuentaContable";
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
        //public static readonly string ins_rendicionSolicitud = "InsertarRendicionSolicitud";

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

        //
        public static readonly string post_insertDOC = "InsertRegistroDoc";
        public static readonly string delete_idDoc = "EliminarDocumento";
        public static readonly string delete_idDocDet = "EliminarDocumentoDet";
        public static readonly string post_insertDOCDt = "InsertRegistroDocDt";
        public static readonly string get_idDOC = "ObtieneIdDOC";
        public static readonly string upd_RDTotal = "ActualizarRDTotal";

        public static readonly string get_item = "ObtenerItem";
        public static readonly string get_items = "ListardItems";

        public static readonly string get_tpoDocumentos = "ObtenerTpoDocumentos";
        public static readonly string get_infoAprobadoresRD = "ObtieneInfoAprobadoresRD";
        public static readonly string upd_cambiarEstadoRD = "CambiaEstadoRD";
        public static readonly string upd_cambiarMigradaRD = "ActualizaRDMigrado";
        public static readonly string upd_migradaRdenSAP = "UpdateMigraRdSAP";
        public static readonly string upd_aprobadoresRD = "ActualizaablaAprobadoresRD";
        public static readonly string post_insertAprobadoresRD = "InsertaTablaAprobadoresRD";
        public static readonly string get_rendicion = "ObtenerRendicion";
        public static readonly string get_listaDocumentoDet = "ObtenerDocumentoDetalles";
        public static readonly string get_obtenerDocumento = "ObtenerDocumento";
        public static readonly string get_listaAprobadoresCabRd = "ListarAprobadoresCabeceraRd";
        public static readonly string get_estado = "ObtenerEstado";
        public static readonly string get_obtenerDocumentos = "ObtenerDocumentos";
        public static readonly string get_listaAprobadoresDetRd = "ListarAprobadoresDetalleRd";
        public static readonly string get_indicador = "ObtenerIndicador";
        public static readonly string get_tpoDocumento = "ObtenerTpoDocumento";
        public static readonly string upd_idDOC = "ActualizarDocumento";
        public static readonly string upd_idDOCDet = "ActualizarDocumentoDet";
        public static readonly string del_idDOCDet = "EliminarDocumentoDet";
        public static readonly string rev_estadoRD = "RevertirEstadoRD";
        public static readonly string get_stateEdit_RD = "ObtenerEstadoEditRD";
        public static readonly string upd_stateEdit_RD = "CambiarEstadoEditRD";
        public static readonly string upd_stateRDLoadDocs_RD = "CambiarEstadoRDCargaDocs"; 

        public static readonly string get_solicitudEar2 = "ObtenerSolicitudRendicionAcep";
        public static readonly string get_duplicated_docs = "ContarDocumentosDuplicados";
        public static readonly string upd_AnularModeEditRD = "anularModoEdicionRD";
        public static readonly string ins_RendicionPdf = "ActualiRendicionPdf";
        public static readonly string upd_montoDiferenciaRD = "actualizarMontoDiferenciaRD";


    }   
}
