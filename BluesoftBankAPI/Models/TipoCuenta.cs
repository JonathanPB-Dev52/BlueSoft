using System;
using System.Collections.Generic;

namespace BluesoftBankAPI.Models
{
    public partial class TipoCuenta
    {
        //public TipoCuentum()
        //{
        //    Cuenta = new HashSet<Cuentum>();
        //}

        public int IdTc { get; set; }
        public string? TipoCuentaN { get; set; }

        //public virtual ICollection<Cuentum> Cuenta { get; set; }
    }
}
