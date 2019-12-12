using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TierraCreative.ViewModels
{
    public class ReviewModel
    {
        public int Id { get; set; }

        public string Source { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string ISIN { get; set; }

        public double Amount { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedBy { get; set; }

        [Display(Name = "Submitted Date")]
        public DateTime SubmittedDate { get; set; }

        [Display(Name = "Approved By")]
        public string ApprovedBy { get; set; }

        [Display(Name = "Approved Date")]
        public DateTime? ApprovedDate { get; set; }
    }
}