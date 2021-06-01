using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Test.EFORM
{
    public partial class Product
    {
        public Product()
        {
            Bom = new HashSet<Bom>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string ProductName { get; set; }
        public int? ProductPrice { get; set; }
        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime ModifyDate { get; set; }

        [InverseProperty("IdProductNavigation")]
        public virtual ICollection<Bom> Bom { get; set; }
    }
}
