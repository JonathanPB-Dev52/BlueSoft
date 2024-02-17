using System;
using System.Collections.Generic;

namespace BluesoftBankAPI.Models
{
    public partial class Cliente
    {
        //public Cliente()
        //{
        //    ClienteCuenta = new HashSet<ClienteCuentum>();
        //}

        public int Id { get; set; }
        public string? Pnombre { get; set; }
        public string? Snombre { get; set; }
        public string? Papellido { get; set; }
        public string? Sapellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? IdTipoCliente { get; set; }
        public string? NumeroIdentificador { get; set; }

        //public virtual TipoCliente? IdTipoClienteNavigation { get; set; }
        //public virtual ICollection<ClienteCuentum> ClienteCuenta { get; set; }
    }
}
