using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TierraCreative.FtpPurge.Models
{
    public class FilenameFields
    {
        public string Sequence { get; set; }
        public string Timestamp { get; set; }
        public string MainCSN { get; set; }
        public string CSN { get; set; }
        public string ISIN { get; set; }
        public string InstructionID { get; set; }
        public string TransferDate { get; set; }
        public string TransferQty { get; set; }
        public string TIN { get; set; }        
    }
}
