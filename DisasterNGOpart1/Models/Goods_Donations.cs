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
    
    public partial class Goods_Donations
    {
        public int Goods_ID { get; set; }
        public string Good_Name { get; set; }
        public Nullable<System.DateTime> Goods_Date { get; set; }
        public int No_of_items { get; set; }
        public string Category { get; set; }
        public string Item_description { get; set; }
        public string Username { get; set; }

        public string Annonymous { get; set; }
        public virtual Register Register { get; set; }
    }
}