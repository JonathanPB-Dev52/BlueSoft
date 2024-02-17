using System;
using System.Collections.Generic;

namespace BluesoftBankAPI.Models
{
    public partial class Ciudad
    {
        //public Ciudad()
        //{
        //    Cuenta = new HashSet<Cuentum>();
        //    Movimientos = new HashSet<Movimiento>();
        //}

        public int Id { get; set; }
        public string? NombreCiudad { get; set; }

        //public virtual ICollection<Cuentum> Cuenta { get; set; }
        //public virtual ICollection<Movimiento> Movimientos { get; set; }
    }
}
