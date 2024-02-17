using System;
using System.Collections.Generic;

namespace BluesoftBankAPI.Models
{
    public partial class TipoMovimiento
    {
        //public TipoMovimiento()
        //{
        //    Movimientos = new HashSet<Movimiento>();
        //}

        public int Id { get; set; }
        public string? NombreMovimiento { get; set; }

        //public virtual ICollection<Movimiento> Movimientos { get; set; }
    }
}
