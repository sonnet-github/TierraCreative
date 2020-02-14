namespace TierraCreative.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AIL")]
    public partial class AIL
    {
        public int AILId { get; set; }

        [Display(Name = "UserName")]
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Display(Name = "From CSN")]
        [StringLength(500)]
        public string FromCSN { get; set; }

        [Display(Name = "To CSN")]
        [StringLength(500)]
        public string ToCSN { get; set; }

        [StringLength(12)]
        public string ISIN { get; set; }

        [Display(Name = "Transfer Amount")]
        public double? TransferAmount { get; set; }

        public int? CreatedById { get; set; }
        [ForeignKey("UserId")]
        public User CreatedByUser { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedById { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? ReviewedById { get; set; }
        [ForeignKey("ReviewedById")]
        public User ReviewedUser { get; set; }

        public DateTime? ReviewedDate { get; set; }

        public int? DeletedById { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}
