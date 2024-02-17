using System;
using System.Collections.Generic;

namespace BluesoftBankAPI.Models
{
    public partial class TipoCliente
    {
        //public TipoCliente()
        //{
        //    Clientes = new HashSet<Cliente>();
        //}

        public int Id { get; set; }
        public string? TipoCliente1 { get; set; }

        //public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
