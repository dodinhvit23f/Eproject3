
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
    
public partial class Exams
{

    public int id { get; set; }

    public Nullable<int> Contest_id { get; set; }

    public Nullable<int> Mark { get; set; }

    public Nullable<int> Contester_id { get; set; }

    public Nullable<int> Recipes_id { get; set; }

    public Nullable<int> E_Status { get; set; }



    public virtual Contest Contest { get; set; }

    public virtual Contester Contester { get; set; }

    public virtual Recipes Recipes { get; set; }

}

}
