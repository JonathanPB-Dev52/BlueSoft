using BluesoftBankAPI.Models;
using BluesoftBankAPI.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace BluesoftBankAPI.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class GenerarExcelController : ControllerBase
    {
        public readonly BluesoftBankContext _context;
        Respuesta respuesta = new Respuesta();

        public GenerarExcelController(BluesoftBankContext context)
        {
            _context = context;
        }
        [HttpGet("ClientesConTransaccionesEnMes")]
        public Respuesta GetClientesConTransaccionesEnMes(int mes, int anio)
        {
            try
            {
                var resultado = (
                    from cliente in _context.Clientes
                    join cc in _context.ClienteCuenta on cliente.Id equals cc.IdCliente into ccGroup
                    from cc in ccGroup.DefaultIfEmpty()
                    join cuenta in _context.Cuenta on cc.IdCuenta equals cuenta.Id into cuentaGroup
                    from cuenta in cuentaGroup.DefaultIfEmpty()
                    join movimiento in _context.Movimientos on cuenta.Id equals movimiento.IdCuenta into movimientoGroup
                    from movimiento in movimientoGroup.DefaultIfEmpty()
                    where movimiento == null ||
                          (movimiento.FechaMovimiento == null ||
                           (movimiento.FechaMovimiento.Value.Month == mes && movimiento.FechaMovimiento.Value.Year == anio))
                    orderby movimiento.Monto descending
                    select new
                    {
                        ClienteId = cliente.Id,
                        cliente.Pnombre,
                        cliente.Snombre,
                        cliente.Papellido,
                        cliente.Sapellido,
                        cliente.FechaNacimiento,
                        cliente.IdTipoCliente,
                        cliente.NumeroIdentificador,
                        CuentaId = cuenta.Id,
                        NumeroCuenta = cuenta.NumeroCuenta,
                        SaldoCuenta = cuenta.SaldoCuenta ?? "0",
                        cuenta.IdCiudad,
                        FechaCreacionCuenta = cuenta.FechaCreacion,
                        cuenta.IdTipoCuenta,
                        MovimientoId = movimiento.Id != null ? movimiento.Id : 0,
                        movimiento.IdMovimiento,
                        MovimientoCuentaId = movimiento.IdCuenta ?? 0,
                        Monto = movimiento.Monto ?? "0",
                        MovimientoCiudadId = movimiento.IdCiudad ?? 0,
                        movimiento.FechaMovimiento
                    }
                ).ToList();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Clientes");

                    // Encabezados
                    worksheet.Cells["A1"].Value = "ClienteId";
                    worksheet.Cells["B1"].Value = "Pnombre";
                    worksheet.Cells["C1"].Value = "Snombre";
                    worksheet.Cells["D1"].Value = "Papellido";
                    worksheet.Cells["E1"].Value = "Sapellido";
                    worksheet.Cells["F1"].Value = "FechaNacimiento";
                    worksheet.Cells["G1"].Value = "IdTipoCliente";
                    worksheet.Cells["H1"].Value = "NumeroIdentificador";
                    worksheet.Cells["I1"].Value = "CuentaId";
                    worksheet.Cells["J1"].Value = "NumeroCuenta";
                    worksheet.Cells["K1"].Value = "SaldoCuenta";
                    worksheet.Cells["L1"].Value = "IdCiudad";
                    worksheet.Cells["M1"].Value = "FechaCreacionCuenta";
                    worksheet.Cells["N1"].Value = "IdTipoCuenta";
                    worksheet.Cells["O1"].Value = "MovimientoId";
                    worksheet.Cells["P1"].Value = "IdMovimiento";
                    worksheet.Cells["Q1"].Value = "MovimientoCuentaId";
                    worksheet.Cells["R1"].Value = "Monto";
                    worksheet.Cells["S1"].Value = "MovimientoCiudadId";
                    worksheet.Cells["T1"].Value = "FechaMovimiento";

                    // Datos
                    int row = 2;
                    foreach (var item in resultado)
                    {
                        worksheet.Cells[$"A{row}"].Value = item.ClienteId;
                        worksheet.Cells[$"B{row}"].Value = item.Pnombre;
                        worksheet.Cells[$"C{row}"].Value = item.Snombre;
                        worksheet.Cells[$"D{row}"].Value = item.Papellido;
                        worksheet.Cells[$"E{row}"].Value = item.Sapellido;
                        worksheet.Cells[$"F{row}"].Value = item.FechaNacimiento;
                        worksheet.Cells[$"F{row}"].Style.Numberformat.Format = "yyyy-mm-dd";
                        worksheet.Cells[$"G{row}"].Value = item.IdTipoCliente;
                        worksheet.Cells[$"H{row}"].Value = item.NumeroIdentificador;
                        worksheet.Cells[$"I{row}"].Value = item.CuentaId;
                        worksheet.Cells[$"J{row}"].Value = item.NumeroCuenta;
                        worksheet.Cells[$"K{row}"].Value = item.SaldoCuenta;
                        worksheet.Cells[$"K{row}"].Style.Numberformat.Format = "0.00";
                        worksheet.Cells[$"L{row}"].Value = item.IdCiudad;
                        worksheet.Cells[$"M{row}"].Value = item.FechaCreacionCuenta;
                        worksheet.Cells[$"M{row}"].Style.Numberformat.Format = "yyyy-mm-dd";
                        worksheet.Cells[$"N{row}"].Value = item.IdTipoCuenta;
                        worksheet.Cells[$"O{row}"].Value = item.MovimientoId;
                        worksheet.Cells[$"P{row}"].Value = item.IdMovimiento;
                        worksheet.Cells[$"Q{row}"].Value = item.MovimientoCuentaId;
                        worksheet.Cells[$"R{row}"].Value = item.Monto;
                        worksheet.Cells[$"R{row}"].Style.Numberformat.Format = "0.00";
                        worksheet.Cells[$"S{row}"].Value = item.MovimientoCiudadId;
                        worksheet.Cells[$"T{row}"].Value = item.FechaMovimiento;
                        worksheet.Cells[$"T{row}"].Style.Numberformat.Format = "yyyy-mm-dd";

                        row++;
                    }
                    worksheet.Cells.AutoFitColumns();
                    worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    var stream = new System.IO.MemoryStream();
                    package.SaveAs(stream);

                    byte[] bytes = stream.ToArray();

                    string base64String = Convert.ToBase64String(bytes);

                    respuesta.code = 200;
                    respuesta.Mensaje = "Consulta Clientes exitosa";
                    respuesta.Obsjeto = base64String;

                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.code = 500;
                respuesta.Mensaje = ex.Message;
                respuesta.Obsjeto = null;
                return respuesta;
            }
        }

        [HttpGet("ClientesenCiudadDiferente")]
        public Respuesta ClientesenCiudadDiferente()
        {
            try
            {
                SqlConnection conexion = (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand command = conexion.CreateCommand();
                conexion.Open();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "ObtenerClientesConMovimientos";

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var resultado = new List<ClienteMovimientoResult>();

                    while (reader.Read())
                    {
                        ClienteMovimientoResult consult = new ClienteMovimientoResult
                        {
                            ClienteId = (int)reader["ClienteId"],
                            Pnombre = (string)reader["Pnombre"],
                            Snombre = (string)reader["Snombre"],
                            Papellido = (string)reader["Papellido"],
                            Sapellido = (string)reader["Sapellido"],
                            FechaNacimiento = (DateTime)reader["FechaNacimiento"],
                            IdTipoCliente = (int)reader["IdTipoCliente"],
                            NumeroIdentificador = (string)reader["NumeroIdentificador"],
                            CuentaId = (int)reader["CuentaId"],
                            NumeroCuenta = (string)reader["NumeroCuenta"],
                            SaldoCuenta = (string)reader["SaldoCuenta"],
                            IdCiudad = (int)reader["IdCiudad"],
                            FechaCreacionCuenta = (DateTime)reader["FechaCreacionCuenta"],
                            IdTipoCuenta = (int)reader["IdTipoCuenta"],
                            MovimientoId = (int)reader["MovimientoId"],
                            IdMovimiento = (int)reader["IdMovimiento"],
                            MovimientoCuentaId = (int)reader["MovimientoCuentaId"],
                            Monto = (string)reader["Monto"],
                            MovimientoCiudadId = (int)reader["MovimientoCiudadId"],
                            FechaMovimiento = (DateTime)reader["FechaMovimiento"]
                        };

                        resultado.Add(consult);
                    }

                    conexion.Close();

                    if (resultado == null || resultado.Count == 0)
                    {
                        respuesta.code = 404;
                        respuesta.Mensaje = "No se encontraron resultados";
                        respuesta.Obsjeto = null;
                        return respuesta;
                    }

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Clientes");

                        // Encabezados
                        worksheet.Cells["A1"].Value = "ClienteId";
                        worksheet.Cells["B1"].Value = "Pnombre";
                        worksheet.Cells["C1"].Value = "Snombre";
                        worksheet.Cells["D1"].Value = "Papellido";
                        worksheet.Cells["E1"].Value = "Sapellido";
                        worksheet.Cells["F1"].Value = "FechaNacimiento";
                        worksheet.Cells["G1"].Value = "IdTipoCliente";
                        worksheet.Cells["H1"].Value = "NumeroIdentificador";
                        worksheet.Cells["I1"].Value = "CuentaId";
                        worksheet.Cells["J1"].Value = "NumeroCuenta";
                        worksheet.Cells["K1"].Value = "SaldoCuenta";
                        worksheet.Cells["L1"].Value = "IdCiudad";
                        worksheet.Cells["M1"].Value = "FechaCreacionCuenta";
                        worksheet.Cells["N1"].Value = "IdTipoCuenta";
                        worksheet.Cells["O1"].Value = "MovimientoId";
                        worksheet.Cells["P1"].Value = "IdMovimiento";
                        worksheet.Cells["Q1"].Value = "MovimientoCuentaId";
                        worksheet.Cells["R1"].Value = "Monto";
                        worksheet.Cells["S1"].Value = "MovimientoCiudadId";
                        worksheet.Cells["T1"].Value = "FechaMovimiento";

                        // Datos
                        int row = 2;
                        foreach (var item in resultado)
                        {
                            worksheet.Cells[$"A{row}"].Value = item.ClienteId;
                            worksheet.Cells[$"B{row}"].Value = item.Pnombre;
                            worksheet.Cells[$"C{row}"].Value = item.Snombre;
                            worksheet.Cells[$"D{row}"].Value = item.Papellido;
                            worksheet.Cells[$"E{row}"].Value = item.Sapellido;
                            worksheet.Cells[$"F{row}"].Value = item.FechaNacimiento;
                            worksheet.Cells[$"F{row}"].Style.Numberformat.Format = "yyyy-mm-dd";
                            worksheet.Cells[$"G{row}"].Value = item.IdTipoCliente;
                            worksheet.Cells[$"H{row}"].Value = item.NumeroIdentificador;
                            worksheet.Cells[$"I{row}"].Value = item.CuentaId;
                            worksheet.Cells[$"J{row}"].Value = item.NumeroCuenta;
                            worksheet.Cells[$"K{row}"].Value = item.SaldoCuenta;
                            worksheet.Cells[$"K{row}"].Style.Numberformat.Format = "0.00";
                            worksheet.Cells[$"L{row}"].Value = item.IdCiudad;
                            worksheet.Cells[$"M{row}"].Value = item.FechaCreacionCuenta;
                            worksheet.Cells[$"M{row}"].Style.Numberformat.Format = "yyyy-mm-dd";
                            worksheet.Cells[$"N{row}"].Value = item.IdTipoCuenta;
                            worksheet.Cells[$"O{row}"].Value = item.MovimientoId;
                            worksheet.Cells[$"P{row}"].Value = item.IdMovimiento;
                            worksheet.Cells[$"Q{row}"].Value = item.MovimientoCuentaId;
                            worksheet.Cells[$"R{row}"].Value = item.Monto;
                            worksheet.Cells[$"R{row}"].Style.Numberformat.Format = "0.00";
                            worksheet.Cells[$"S{row}"].Value = item.MovimientoCiudadId;
                            worksheet.Cells[$"T{row}"].Value = item.FechaMovimiento;
                            worksheet.Cells[$"T{row}"].Style.Numberformat.Format = "yyyy-mm-dd";

                            row++;
                        }
                        worksheet.Cells.AutoFitColumns();
                        worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        var stream = new System.IO.MemoryStream();
                        package.SaveAs(stream);

                        byte[] bytes = stream.ToArray();

                        string base64String = Convert.ToBase64String(bytes);

                        respuesta.code = 200;
                        respuesta.Mensaje = "Consulta Clientes exitosa";
                        respuesta.Obsjeto = base64String;

                        return respuesta;
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.code = 500;
                respuesta.Mensaje = ex.Message;
                respuesta.Obsjeto = null;
                return respuesta;
            }
        }
    }

    public class ClienteMovimientoResult
    {
        public int ClienteId { get; set; }
        public string Pnombre { get; set; }
        public string Snombre { get; set; }
        public string Papellido { get; set; }
        public string Sapellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int IdTipoCliente { get; set; }
        public string NumeroIdentificador { get; set; }
        public int CuentaId { get; set; }
        public string NumeroCuenta { get; set; }
        public string? SaldoCuenta { get; set; }
        public int IdCiudad { get; set; }
        public DateTime? FechaCreacionCuenta { get; set; }
        public int IdTipoCuenta { get; set; }
        public int MovimientoId { get; set; }
        public int IdMovimiento { get; set; }
        public int MovimientoCuentaId { get; set; }
        public string? Monto { get; set; }
        public int MovimientoCiudadId { get; set; }
        public DateTime? FechaMovimiento { get; set; }
    }

}
