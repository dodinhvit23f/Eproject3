//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Eproject3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FeedBack
    {
        public int id { get; set; }
        public Nullable<int> Use_id { get; set; }
        public Nullable<int> Recipes_id { get; set; }
        public string Content { get; set; }
        public Nullable<int> Tip_id { get; set; }
    
        public virtual Recipes Recipes { get; set; }
        public virtual Tips Tips { get; set; }
        public virtual Users Users { get; set; }
    }
}