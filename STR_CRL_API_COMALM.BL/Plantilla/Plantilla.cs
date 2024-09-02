using STR_CRL_API_COMALM.EL.Request;
using STR_CRL_API_COMALM.EL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace STR_CRL_API_COMALM.BL
{
    public class Plantilla
    {
        
        public static readonly string ruta = "D:\\Chamba Backend\\Electro Peru\\Plantillas\\PlantillaPortalEAR - Importacion 1.xlsx";
        public ExcelPackage package = null;

        public List<Documento> ObtienePlantilla(ExcelPackage excel, int id)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            package = new ExcelPackage();
            package = excel;


            ExcelWorksheet facturaSheet = package.Workbook.Worksheets["Cabecera"];

            int rowCount = facturaSheet.Dimension.Rows;
            int colCount = facturaSheet.Dimension.Columns;

            List<Documento> documentos = new List<Documento>();

            // Valida si hay un ID Rendicion diferente
            for (int row = 0; row < rowCount; row++)
            {
                if (row > 1)
                {
                    if (facturaSheet.Cells[row, 21].Text != "")
                        if (facturaSheet.Cells[row, 21].Text != id.ToString())
                            throw new Exception("Se está intentando agregar documentos a una rendición diferente");
                }
            }

            for (int row = 1; row <= rowCount; row++)
            {
                if (row > 1)
                {
                    ObtieneCuerpo(ref documentos, facturaSheet, row);
                }
            }

            return documentos;
        }
        public void ObtieneCuerpo(ref List<Documento> documentos, ExcelWorksheet cab, int row)
         {
            Documento documento = new Documento();
            try
            {
                ObtieneCabecera(ref documentos, ref documento, ref cab, ref row);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ObtieneCabecera(ref List<Documento> documentos, ref Documento documento, ref ExcelWorksheet cab, ref int row)
        {
            string idDocumento = cab.Cells[row, 1].Text; // Valor en la primera celda
            if (string.IsNullOrEmpty(idDocumento))
                return; // Si está vacío, salir del método

            string idRendicion = cab.Cells[row, 2].Text;
            if (string.IsNullOrEmpty(idRendicion))
            {
                return;
            }

            string id = cab.Cells[row, 1].Text;     // Id del documento
            string almacen = string.Empty;     // Almacen al que pertenece
            string tpoRendicion = string.Empty;      // Tipo de Rendicion
            string mtvRendicion = string.Empty;     // Motivo de Rendicion
            //int cantidad = 1;                 // Cantidad del detalle

            ValoresPorDefecto(ref mtvRendicion, idRendicion);
            documento.STR_RENDICION = Convert.ToInt32(cab.Cells[row, 2].Text); // ID DE LA RENDICION
            documento.STR_RD_ID = Convert.ToInt32(cab.Cells[row, 2].Text); // ID DE LA RENDICION
            DateTime fechaContabiliza = DateTime.Parse(cab.Cells[row, 3].Text);
            DateTime fechaDocumento = DateTime.Parse(cab.Cells[row, 4].Text);
            DateTime fechaVencimiento = DateTime.Parse(cab.Cells[row, 5].Text);
            //agregar mas campos del excel


            documento.STR_FECHA_CONTABILIZA = fechaContabiliza.ToString("yyyy-MM-dd");
            documento.STR_FECHA_DOC = fechaDocumento.ToString("yyyy-MM-dd");
            documento.STR_FECHA_VENCIMIENTO = fechaVencimiento.ToString("yyyy-MM-dd");
            //documento.STR_PROVEEDOR = cab.Cells[row, 6].Text == "" ? null : new Proveedor { CardCode = cab.Cells[row, 6].Text, CardName = cab.Cells[row, 7].Text, LicTradNum = cab.Cells[row, 8].Text };
            //documento.STR_PROVEEDOR = cab.Cells[row, 7].Text == "" ? null : new Proveedor {  };
            //documento.STR_PROVEEDOR = cab.Cells[row, 8].Text == "" ? null : new Proveedor { };

            string ruc = cab.Cells[row, 6].Text;
            if (!string.IsNullOrEmpty(ruc))
            {
                Sq_Proveedor sqProveedor = new Sq_Proveedor();
                var response = sqProveedor.ObtenerProveedorxRuc(ruc);

                if (response.CodRespuesta == "00" && response.Result != null && response.Result.Count > 0)
                {
                    documento.STR_PROVEEDOR = response.Result[0]; // Asigna el primer proveedor encontrado con el RUC
                }
                else
                {
                    documento.STR_PROVEEDOR = null; // No se encontró el proveedor
                }
            }
            else
            {
                documento.STR_PROVEEDOR = null;
            }
            
            documento.STR_MONEDA = cab.Cells[row, 7].Text == "" ? null : new Complemento { name = cab.Cells[row, 7].Text };

            documento.STR_COMENTARIOS = cab.Cells[row, 8].Text;
            documento.STR_TIPO_DOC = cab.Cells[row, 9].Text == "" ? null : new Complemento { id = cab.Cells[row, 9].Text.Split('-')[0].Trim() };
            documento.STR_SERIE_DOC = cab.Cells[row, 10].Text;
            documento.STR_CORR_DOC = cab.Cells[row, 11].Text;
            //documento.STR_VALIDA_SUNAT = Convert.ToBoolean(cab.Cells[row, 14].Value);
            //documento.STR_OPERACION = cab.Cells[row,15].Text;
            //if (!string.IsNullOrEmpty(cab.Cells[row, 16].Text))
            //{
            //    documento.STR_PARTIDAFLUJO = Convert.ToInt32(cab.Cells[row, 16].Text);
            //}
            documento.STR_TOTALDOC = Convert.ToDouble(cab.Cells[row, 12].Text);
            //documento.STR_MOTIVORENDICION = cab.Cells[row,18].Text==""? null:new Complemento{id=cab.Cells[row,18].Text};
            documento.STR_DIRECCION = cab.Cells[row, 13].Text; // OPCIONAL
            documento.STR_AFECTACION = cab.Cells[row, 14].Text == ""? null : new Complemento { id = cab.Cells[row, 14].Text.Split('-')[0].Trim() };

            // Asignar el valor por defecto a STR_MOTIVORENDICION
            if (documento.STR_MOTIVORENDICION == null)
            {
                documento.STR_MOTIVORENDICION = new Complemento(); // Inicializa si es null
            }

            documento.STR_MOTIVORENDICION.id = mtvRendicion;

            documento.detalles = ObtieneDetalle(almacen, id, tpoRendicion);

            documentos.Add(documento);
        }

        public List<DocumentoDet> ObtieneDetalle(string almacen, string id, string tpoOpe)
        {
            List<DocumentoDet> detalles = new List<DocumentoDet>();

            ExcelWorksheet det = package.Workbook.Worksheets["Detalle"];

            int rowCount = det.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                // Verificar si la primera celda de la fila está vacía
                if (string.IsNullOrEmpty(det.Cells[row, 1].Text))
                {
                    continue; // Saltar la fila si la primera celda está vacía
                }

                if (det.Cells[row, 1].Text == id)
                {
                    DocumentoDet documentoDet = new DocumentoDet()
                    {
                        ID = Convert.ToInt32(det.Cells[row, 1].Text),
                        STR_CODARTICULO = new EL.Response.Articulo { ItemCode = det.Cells[row, 2].Text, ItemName = det.Cells[row, 3].Text },
                        //STR_ALMACEN = det.Cells[row, 4].Text,
                        //STR_INDIC_IMPUESTO = new Complemento { id = det.Cells[row, 4].Text },
                        STR_INDIC_IMPUESTO = det.Cells[row, 4].Text == "" ? null : new Complemento { id = det.Cells[row, 4].Text.Split('-')[0].Trim() },
                        //STR_SUBTOTAL = Convert.ToDouble(det.Cells[row, 5].Text),
                        STR_DIM1 = new Complemento { id = det.Cells[row, 5].Text },
                        STR_DIM2 = new Complemento { id = det.Cells[row, 6].Text },
                        STR_DIM4 = new Complemento { id = det.Cells[row, 7].Text },
                        STR_DIM5 = new Complemento { id = det.Cells[row, 8].Text },


                        STR_ALMACEN = almacen,
                        STR_TPO_OPERACION = tpoOpe,
                        //STR_CANTIDAD = cantidad,
                        STR_PROYECTO = new Complemento { id = det.Cells[row, 9].Text },
                        STR_PRECIO = Convert.ToDecimal(det.Cells[row, 10].Text),
                        STR_CANTIDAD = Convert.ToInt32(det.Cells[row, 11].Text),
                        STR_IMPUESTO = Convert.ToDecimal(det.Cells[row, 12].Text),
                        STR_SUBTOTAL = Convert.ToDouble(det.Cells[row, 13].Text),
                    };
                    detalles.Add(documentoDet);
                }
            }
            return detalles;
        }

        public void ValoresPorDefecto(ref string mtvRendicion, string id) // Id de Rendicion
        {
            Sq_Rendicion sq = new Sq_Rendicion();

            var data = sq.ObtenerRendicion(id.ToString()).Result[0];
            //almacen = data.STR_EMPLEADO_ASIGNADO.fax;
            //tpoRendicion = "1";//data.SOLICITUDRD.STR_MOTIVORENDICION;
            mtvRendicion = "VIA"; // 
        }
    
    }
}
