namespace TierraCreative.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DRP")]
    public partial class DRP
    {
        public int DRPId { get; set; }

        [Display(Name = "UserName")]
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [StringLength(500)]
        public string CSN { get; set; }

        [StringLength(500)]
        public string ISIN { get; set; }

        [Display(Name = "DRP Amount")]
        public double? DRPAmount { get; set; }

        public int? CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedById { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? ReviewedById { get; set; }
        [ForeignKey("ReviewedById")]
        public User ReviewedUser { get; set; }

        public DateTime? ReviewedDate { get; set; }
    }
}
