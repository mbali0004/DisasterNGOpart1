//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DisasterNGOpart1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mon_Donation
    {
        public int Mon_ID { get; set; }
        public string Mon_Name { get; set; }
        public string Username { get; set; }
        public Nullable<System.DateTime> Mon_Date { get; set; }
        public Nullable<decimal> Mon_Amt { get; set; }
    
        public string Annonymous {  get; set; }
        public virtual Register Register { get; set; }
    }
}