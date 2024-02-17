namespace BluesoftBankAPI.Models.DTO
{
    public class ClienteDetalle
    {
        public int Id { get; set; }
        public string? Pnombre { get; set; }
        public string? Snombre { get; set; }
        public string? Papellido { get; set; }
        public string? Sapellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? IdTipoCliente { get; set; }
        public string? NumeroIdentificador { get; set; }
        public List<Cuenta>? Cuenta { get; set; }
        public List<Movimiento>? Movimiento { get; set; }
    }
}
