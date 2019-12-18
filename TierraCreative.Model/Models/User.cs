namespace TierraCreative.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [Key]
        public int UserId { get; set; }

        [StringLength(150)]
        public string UserName { get; set; }

        [StringLength(150)]
        public string Email { get; set; }

        [StringLength(150)]
        public string Password { get; set; }

        [Display(Name = "Role")]       
        public int? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [Display(Name = "Enabled")]
        public bool IsEnabled { get; set; }

        public int? CreatedById { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? UpdatedById { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? DeletedById { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}
