//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVD_Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rank
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rank()
        {
            this.Employees = new HashSet<Employee>();
        }
    
        public int Rank_ID { get; set; }
        public string RankName { get; set; }
        public Nullable<double> Premium { get; set; }
        public string Duties { get; set; }
        public string Requirements { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees { get; set; }
    }
}