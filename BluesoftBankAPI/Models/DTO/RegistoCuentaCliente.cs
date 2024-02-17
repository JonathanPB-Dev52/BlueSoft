namespace BluesoftBankAPI.Models.DTO
{
    public class RegistoCuentaCliente
    {
        public string? Pnombre { get; set; }
        public string? Snombre { get; set; }
        public string? Papellido { get; set; }
        public string? Sapellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? IdTipoCliente { get; set; }
        public string? NumeroIdentificador { get; set; }
        //public int Id { get; set; }
        public string? NumeroCuenta { get; set; }
        public string? SaldoCuenta { get; set; }
        public int? IdCiudad { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int? IdTipoCuenta { get; set; }
    }
}
