//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WEBAPI_VENDINGMACHINE
{
    using System;
    using System.Collections.Generic;
    
    public partial class Solded
    {
        public int IdSolded { get; set; }
        public System.DateTime Date { get; set; }
        public System.TimeSpan Time { get; set; }
        public int IdMachine { get; set; }
        public int IdProduct { get; set; }
    
        public virtual Items Items { get; set; }
        public virtual Vending_Machine Vending_Machine { get; set; }
    }
}
