using System;
using System.Collections.Generic;

namespace BluesoftBankAPI.Models
{
    public partial class ClienteCuenta
    {
        public int Id { get; set; }
        public int? IdCliente { get; set; }
        public int? IdCuenta { get; set; }

        //public virtual Cliente? IdClienteNavigation { get; set; }
        //public virtual Cuentum? IdCuentaNavigation { get; set; }
    }
}
