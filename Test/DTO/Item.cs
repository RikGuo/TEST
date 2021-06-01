using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Test.EFORM
{
    public partial class Item
    {
        public Item()
        {
            Bom = new HashSet<Bom>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string ItemName { get; set; }
        public int CurrentItem { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifyDate { get; set; }

        [InverseProperty("IdItemNavigation")]
        public virtual ICollection<Bom> Bom { get; set; }
    }
}
