
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
    
public partial class Tips
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Tips()
    {

        this.FeedBack = new HashSet<FeedBack>();

    }


    public int id { get; set; }

    public Nullable<int> Use_id { get; set; }

    public string Content { get; set; }

    public string Img { get; set; }

    public string Title { get; set; }

    public string Levels { get; set; }

    public Nullable<int> Cate_id { get; set; }

    public Nullable<bool> isFree { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<FeedBack> FeedBack { get; set; }

    public virtual Users Users { get; set; }

    public virtual Categories Categories { get; set; }

}

}
