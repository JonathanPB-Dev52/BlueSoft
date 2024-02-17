using System;
using System.Collections.Generic;

namespace BluesoftBankAPI.Models
{
    public partial class Cuenta
    {
        //public Cuentum()
        //{
        //    ClienteCuenta = new HashSet<ClienteCuentum>();
        //    Movimientos = new HashSet<Movimiento>();
        //}

        public int Id { get; set; }
        public string? NumeroCuenta { get; set; }
        public string? SaldoCuenta { get; set; }
        public int? IdCiudad { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int? IdTipoCuenta { get; set; }

        //public virtual Ciudad? IdCiudadNavigation { get; set; }
        //public virtual TipoCuentum? IdTipoCuentaNavigation { get; set; }
        //public virtual ICollection<ClienteCuentum> ClienteCuenta { get; set; }
        //public virtual ICollection<Movimiento> Movimientos { get; set; }
    }
}
