using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Test.EFORM
{
    public partial class Bom
    {
        [Key]
        [Column("AUTOID")]
        public int Autoid { get; set; }
        [Column("Id_Product")]
        public int IdProduct { get; set; }
        [Column("Id_Item")]
        public int IdItem { get; set; }
        public int ItemNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime ModifyDate { get; set; }

        [ForeignKey(nameof(IdItem))]
        [InverseProperty(nameof(Item.Bom))]
        public virtual Item IdItemNavigation { get; set; }
        [ForeignKey(nameof(IdProduct))]
        [InverseProperty(nameof(Product.Bom))]
        public virtual Product IdProductNavigation { get; set; }
    }
}
