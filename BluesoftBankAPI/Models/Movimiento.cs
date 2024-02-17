using System;
using System.Collections.Generic;

namespace BluesoftBankAPI.Models
{
    public partial class Movimiento
    {
        public int Id { get; set; }
        public int? IdMovimiento { get; set; }
        public int? IdCuenta { get; set; }
        public string? Monto { get; set; }
        public int? IdCiudad { get; set; }
        public DateTime? FechaMovimiento { get; set; }

        //public virtual Ciudad? IdCiudadNavigation { get; set; }
        //public virtual Cuentum? IdCuentaNavigation { get; set; }
        //public virtual TipoMovimiento? IdMovimientoNavigation { get; set; }
    }
}
