using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BluesoftBankAPI.Models;
using BluesoftBankAPI.Response;
using BluesoftBankAPI.Models.DTO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cors;

namespace BluesoftBankAPI.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {

        public readonly BluesoftBankContext _context;
        Respuesta respuesta = new Respuesta();
        public Movimiento movimientos = new Movimiento();

        public CuentaController(BluesoftBankContext context)
        {
            _context = context;
        }

        [HttpGet("ConsultarClienteYcuentas")]
        public Respuesta GetCuentas()
        {
            List<Cliente> ListaCliente = new List<Cliente>();
            List<ClienteDetalle> listaClienteDetalle = new List<ClienteDetalle>();
            try
            {
                ListaCliente = _context.Clientes.ToList();

                foreach (var cliente in ListaCliente)
                {
                    List<Cuenta> cuentaLista = new List<Cuenta>();
                    List<Movimiento> movimientoLista = new List<Movimiento>();

                    ClienteDetalle clienteDetalle = new ClienteDetalle();

                    clienteDetalle.Id = cliente.Id;
                    clienteDetalle.Pnombre = cliente.Pnombre.Trim();
                    clienteDetalle.Snombre = cliente.Snombre.Trim();
                    clienteDetalle.Papellido = cliente.Papellido.Trim();
                    clienteDetalle.Sapellido = cliente.Sapellido.Trim();
                    clienteDetalle.Sapellido = cliente.Sapellido.Trim();
                    clienteDetalle.FechaNacimiento = cliente.FechaNacimiento;
                    clienteDetalle.IdTipoCliente = cliente.IdTipoCliente;
                    clienteDetalle.NumeroIdentificador = cliente.NumeroIdentificador.Trim();

                    List<ClienteCuenta> cuentas = _context.ClienteCuenta.Where(pi => pi.IdCliente == clienteDetalle.Id).ToList();
                    foreach (var cuenta in cuentas)
                    {
                        Cuenta cuentaCliente = _context.Cuenta.FirstOrDefault(pi => pi.Id == cuenta.IdCuenta);
                        if (cuentaCliente != null)
                        {
                            cuentaCliente.NumeroCuenta = cuentaCliente.NumeroCuenta.Trim();
                            cuentaCliente.SaldoCuenta = cuentaCliente.SaldoCuenta.Trim();
                            cuentaLista.Add(cuentaCliente);
                        }
                    }
                    foreach (var movimiento in cuentas)
                    {
                        List<Movimiento> movimientos = _context.Movimientos.Where(pi => pi.IdCuenta == movimiento.IdCuenta).ToList();
                        foreach (var movimient in movimientos)
                        { 
                            if (movimient != null)
                            {
                                movimient.Monto = movimient.Monto.Trim();
                                movimientoLista.Add(movimient);
                            }
                        }
                    }
                    clienteDetalle.Cuenta = cuentaLista;
                    clienteDetalle.Movimiento = movimientoLista;
                    listaClienteDetalle.Add(clienteDetalle);
                }

                respuesta.code = 200;
                respuesta.Mensaje = "Consulta exitosa";
                respuesta.Obsjeto = listaClienteDetalle;
            }
            catch (Exception ex)
            {
                respuesta.code = 200;
                respuesta.Mensaje = ex.Message;
                respuesta.Obsjeto = null;

            }


            return respuesta;
        }
        [HttpPost("ConsigarYretirar")]
        public Respuesta PostCuenta(string NumeroCuenta, string Monto, int TipoTranscacion, int IdCiudad)
        {
            try
            {
                if (NumeroCuenta.Length > 0 && Monto.Length > 0)
                {
                    Cuenta cuenta = _context.Cuenta.FirstOrDefault(pi => pi.NumeroCuenta == NumeroCuenta);

                    if (cuenta != null)
                    {
                        decimal montoTransaccion = decimal.Parse(Monto);

                        if (TipoTranscacion == 2)
                        {
                            decimal saldoActual = decimal.Parse(cuenta.SaldoCuenta);
                            decimal nuevoSaldo = saldoActual + montoTransaccion;
                            cuenta.SaldoCuenta = nuevoSaldo.ToString().Trim();
                        }
                        else if (TipoTranscacion == 1)
                        {
                            decimal saldoActual = decimal.Parse(cuenta.SaldoCuenta);
                            if (montoTransaccion <= saldoActual)
                            {
                                decimal nuevoSaldo = saldoActual - montoTransaccion;
                                cuenta.SaldoCuenta = nuevoSaldo.ToString().Trim();
                            }
                            else
                            {
                                respuesta.code = 200;
                                respuesta.Mensaje = "Fondos insuficientes para realizar el retiro.";
                                respuesta.Obsjeto = null;
                                return respuesta;
                            }
                        }
                        else
                        {
                            respuesta.code = 200;
                            respuesta.Mensaje = "El tipo de transacción no se reconoce, vuelve a intentar.";
                            respuesta.Obsjeto = null;
                            return respuesta;
                        }
                        movimientos.Id = 0;
                        movimientos.IdMovimiento = TipoTranscacion;
                        movimientos.IdCuenta = cuenta.Id;
                        movimientos.Monto = Monto;
                        movimientos.IdCiudad = IdCiudad;
                        movimientos.FechaMovimiento = DateTime.Now;
                        bool registroM = RegistrarMovimiento(movimientos);
                        if (registroM)
                        {
                        _context.SaveChanges();
                        }
                        else
                        {
                            respuesta.code = 200;
                            respuesta.Mensaje = "Error al registrar el movimiento";
                            respuesta.Obsjeto = cuenta;
                        }

                        respuesta.code = 200;
                        respuesta.Mensaje = "El monto fue actualizado exitosamente";
                        respuesta.Obsjeto = cuenta;
                    }
                    else
                    {
                        respuesta.code = 200;
                        respuesta.Mensaje = "El numero de cuenta no se encontro";
                        respuesta.Obsjeto = null;
                    }
                }
                else
                {
                    respuesta.code = 200;
                    respuesta.Mensaje = "El monto ingresado es invalido.";
                    respuesta.Obsjeto = null;
                }
            }
            catch (Exception ex)
            {
                respuesta.code = 200;
                respuesta.Mensaje = ex.Message;
                respuesta.Obsjeto = null;
            }
            return respuesta;
        }

        [HttpPost("RegistrarCuentaYcliente")]
        public Respuesta PostCuentaYcliente(RegistoCuentaCliente registroCuentaCliente)
        {
            Cliente cliente = new Cliente();
            Cuenta cuenta = new Cuenta();
            ClienteCuenta clienteCuenta = new ClienteCuenta();
            try
            {
                //Datos Cliente
                cliente.Pnombre = registroCuentaCliente.Pnombre.Trim();
                cliente.Snombre = registroCuentaCliente.Snombre.Trim();
                cliente.Papellido = registroCuentaCliente.Papellido.Trim();
                cliente.Sapellido = registroCuentaCliente.Sapellido.Trim();
                cliente.FechaNacimiento = registroCuentaCliente.FechaNacimiento;
                cliente.IdTipoCliente = registroCuentaCliente.IdTipoCliente;
                cliente.NumeroIdentificador = registroCuentaCliente.NumeroIdentificador.Trim();

                //Datos Cuenta
                cuenta.NumeroCuenta = registroCuentaCliente.NumeroCuenta.Trim();
                cuenta.SaldoCuenta = registroCuentaCliente.SaldoCuenta.Trim();
                cuenta.IdCiudad = registroCuentaCliente.IdCiudad;
                cuenta.FechaCreacion = DateTime.Now;
                cuenta.IdTipoCuenta = registroCuentaCliente.IdTipoCuenta;

                _context.Clientes.Add(cliente);
                _context.Cuenta.Add(cuenta);
                _context.SaveChanges();

                int IdCliente = cliente.Id;
                int IdCuenta = cuenta.Id;

                clienteCuenta.IdCliente = IdCliente;
                clienteCuenta.IdCuenta = IdCuenta;

                _context.ClienteCuenta.Add(clienteCuenta);
                _context.SaveChanges();


                respuesta.code = 200;
                respuesta.Mensaje = "Registro exitoso.";
                respuesta.Obsjeto = "";
            }
            catch (Exception ex)
            {
                respuesta.code = 200;
                respuesta.Mensaje = ex.Message;
                respuesta.Obsjeto = null;

            }


            return respuesta;
        }

        [Route("ConsultarClienteMenoresEdad")]
        [HttpGet]
        public Respuesta GetYears()
        {
            List<Cliente> ListCliente = new List<Cliente>();
            try
            {
                ListCliente = _context.Clientes.ToList();
                var ListaEdad = ListCliente.Where(x => x.FechaNacimiento < DateTime.Now.AddYears(- 18));

                respuesta.code = 200;
                respuesta.Mensaje = "Consulta exitosa";
                respuesta.Obsjeto = ListaEdad;
            }
            catch (Exception ex)
            {
                respuesta.code=400;
                respuesta.Mensaje=ex.Message;
                respuesta.Obsjeto = null;
            }
            return respuesta;
        }


        private bool RegistrarMovimiento(Movimiento mov)
        {
            Movimiento movimiento = new Movimiento();
            try
            {
                movimiento.Id = mov.Id;
                movimiento.IdMovimiento = mov.IdMovimiento;
                movimiento.IdCuenta = mov.IdCuenta;
                movimiento.Monto = mov.Monto;
                movimiento.IdCiudad = mov.IdCiudad;
                movimiento.FechaMovimiento = mov.FechaMovimiento;


                _context.Movimientos.Add(movimiento);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
