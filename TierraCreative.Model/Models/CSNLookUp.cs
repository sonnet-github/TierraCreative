namespace TierraCreative.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CSNLookUp")]
    public partial class CSNLookUp
    {
        [Key]
        public int CSNId { get; set; }

        [StringLength(500)]
        public string CSNAccount { get; set; }

        [StringLength(1500)]
        public string CSNName { get; set; }
    }
}
