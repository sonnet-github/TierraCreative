namespace TierraCreative.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Setting")]
    public partial class Setting
    {
        public int SettingId { get; set; }

        [StringLength(250)]
        public string Value { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
    }
}
