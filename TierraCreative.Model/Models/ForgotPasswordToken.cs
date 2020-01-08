namespace TierraCreative.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ForgotPasswordToken")]
    public partial class ForgotPasswordToken
    {
        [Key]
        public int ForgotPasswordId { get; set; }

        [StringLength(150)]
        public string Unique_Guid { get; set; }

        [StringLength(150)]
        public string Email { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
