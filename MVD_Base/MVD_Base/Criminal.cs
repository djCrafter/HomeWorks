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
    
    public partial class Criminal
    {
        public int CaseNumber { get; set; }
        public string FullName { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Gender { get; set; }
        public string Adress { get; set; }
        public int Crime_ID { get; set; }
        public int Victim_ID { get; set; }
        public string Condition { get; set; }
        public int Employee_ID { get; set; }
    
        public virtual Crime Crime { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Victim Victim { get; set; }
    }
}
