using BluesoftBankAPI.Models;
using BluesoftBankAPI.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BluesoftBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LlavesForaneasController : ControllerBase
    {
        public readonly BluesoftBankContext _context;
        Respuesta respuesta = new Respuesta();

        public LlavesForaneasController(BluesoftBankContext context)
        {
            _context = context;
        }

        [HttpGet("ConsultarTipoCLiente")]
        public Respuesta Get()
        {
            List<TipoCliente> tp = new List<TipoCliente>();
            try
            {
                tp = _context.TipoClientes.ToList();

                // Crear una nueva lista con los elementos modificados
                List<TipoCliente> nuevaLista = new List<TipoCliente>();
                foreach (var item in tp)
                {
                    TipoCliente tipo = new TipoCliente();
                    tipo.Id = item.Id;
                    tipo.TipoCliente1 = item.TipoCliente1.Trim();
                    nuevaLista.Add(tipo);
                }

                respuesta.code = 200;
                respuesta.Mensaje = "Consulta Exitosa";
                respuesta.Obsjeto = nuevaLista;
            }
            catch (Exception ex)
            {
                respuesta.code = 500;
                respuesta.Mensaje = "Error en el método consultar Tipo Cliente";
                respuesta.Obsjeto = null;
            }
            return respuesta;
        }
        [HttpGet("ConsultarTipoCuenta")]
        public Respuesta GetI()
        {
            try
            {
                List<TipoCuenta> tc = new List<TipoCuenta>();
                tc = _context.TipoCuenta.ToList();

                List<TipoCuenta> nuevaLista = new List<TipoCuenta>();
                foreach (var item in tc)
                {
                    TipoCuenta tipo = new TipoCuenta();
                    tipo.IdTc = item.IdTc;
                    tipo.TipoCuentaN = item.TipoCuentaN.Trim();
                    nuevaLista.Add(tipo);
                }

                respuesta.code = 200;
                respuesta.Mensaje = "Consulta Exitosa";
                respuesta.Obsjeto = tc;
            }
            catch (Exception ex)
            {
                respuesta.code = 500;
                respuesta.Mensaje = "Error en el método consultar Tipo cuenta: " + ex.Message;
                respuesta.Obsjeto = null;
            }
            return respuesta;
        }

        [HttpGet("ConsultarCiudad")]
        public Respuesta GetII()
        {
            try
            {
                List<Ciudad> tc = new List<Ciudad>();
                tc = _context.Ciudads.ToList();

                List<Ciudad> nuevaLista = new List<Ciudad>();
                foreach (var item in tc)
                {
                    Ciudad tipo = new Ciudad();
                    tipo.Id = item.Id;
                    tipo.NombreCiudad = item.NombreCiudad.Trim();
                    nuevaLista.Add(tipo);
                }

                respuesta.code = 200;
                respuesta.Mensaje = "Consulta Exitosa";
                respuesta.Obsjeto = tc;
            }
            catch (Exception ex)
            {
                respuesta.code = 500;
                respuesta.Mensaje = "Error en el método consultar Ciudades: " + ex.Message;
                respuesta.Obsjeto = null;
            }
            return respuesta;
        }

        [HttpGet("ConsultarTipoMovimiento")]
        public Respuesta GetIII()
        {
            try
            {
                List<TipoMovimiento> tc = new List<TipoMovimiento>();
                tc = _context.TipoMovimientos.ToList();

                List<TipoMovimiento> nuevaLista = new List<TipoMovimiento>();
                foreach (var item in tc)
                {
                    TipoMovimiento tipo = new TipoMovimiento();
                    tipo.Id = item.Id;
                    tipo.NombreMovimiento = item.NombreMovimiento.Trim();
                    nuevaLista.Add(tipo);
                }

                respuesta.code = 200;
                respuesta.Mensaje = "Consulta Exitosa";
                respuesta.Obsjeto = tc;
            }
            catch (Exception ex)
            {
                respuesta.code = 500;
                respuesta.Mensaje = "Error en el método consultar tipo movimiento: " + ex.Message;
                respuesta.Obsjeto = null;
            }
            return respuesta;
        }

    }
}
