using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TierraCreative.FtpPurge
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = string.Empty;

            //extraction
            List<Models.FilenameFields> fileNameFields = new List<Models.FilenameFields>();
            var data = FtpPurge.Process.Extractions.ExtractPurgeData(filename, out fileNameFields);
            
            //ftp data
            var IsSuccess = FtpPurge.Process.Ftps.PostDatatoFTP(filename);

        }
    }
}
