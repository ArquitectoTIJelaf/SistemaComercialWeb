﻿using SisComWeb.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class PuntoVentaRepository
    {
        #region Métodos No Transaccionales

        public static Response<List<PuntoVentaEntity>> ListarTodos(Int16 Codi_Sucursal)
        {
            var response = new Response<List<PuntoVentaEntity>>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarPuntosVenta";
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, Codi_Sucursal);
                var Lista = new List<PuntoVentaEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new PuntoVentaEntity
                        {
                            CodiSucursal = Reader.GetSmallIntValue(drlector, "Codi_Sucursal"),
                            CodiPuntoVenta = Reader.GetSmallIntValue(drlector, "Codi_puntoVenta"),
                            Descripcion = Reader.GetStringValue(drlector, "Descripcion")
                        };
                        Lista.Add(entidad);
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Se encontró correctamente los puntos de venta. ";
                    response.Estado = true;
                }
            }
            return response;
        }

        #endregion
    }
}
