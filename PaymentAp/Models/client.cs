//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PaymentAp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public client()
        {
            this.transactions = new HashSet<transaction>();
            this.transactions1 = new HashSet<transaction>();
        }
    
        public string id { get; set; }
        public string FullName { get; set; }
        public string accNumber { get; set; }
        public string IdentificationId { get; set; }
        public Nullable<int> BankId { get; set; }
        public Nullable<float> balance { get; set; }
        public string PhoneNumber { get; set; }
        public string tokenkey { get; set; }
        public Nullable<System.DateTime> expdate { get; set; }
    
        public virtual Bank Bank { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<transaction> transactions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<transaction> transactions1 { get; set; }
    }
}