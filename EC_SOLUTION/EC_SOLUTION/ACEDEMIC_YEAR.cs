//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EC_SOLUTION
{
    using System;
    using System.Collections.Generic;
    
    public partial class ACEDEMIC_YEAR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ACEDEMIC_YEAR()
        {
            this.EC_CLAIMS = new HashSet<EC_CLAIMS>();
        }
    
        public int ACEDEMICYEARID { get; set; }
        public string YEAR { get; set; }
        public Nullable<bool> STATUS { get; set; }
        public Nullable<System.DateTime> closuredate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EC_CLAIMS> EC_CLAIMS { get; set; }
    }
}