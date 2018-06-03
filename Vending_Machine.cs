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
    
    public partial class Vending_Machine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vending_Machine()
        {
            this.Change = new HashSet<Change>();
            this.Slots = new HashSet<Slots>();
            this.Solded = new HashSet<Solded>();
        }
    
        public int Id_Machine { get; set; }
        public string Machine_Model { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Poblation { get; set; }
        public Nullable<int> UserAdministrator { get; set; }
        public string TokenId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Change> Change { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Slots> Slots { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Solded> Solded { get; set; }
    }
}
