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
            var data = FtpPurge.Process.Extractions.ExtractPurgeData(filename);
            
            //ftp data
            var IsSuccess = FtpPurge.Process.Ftps.PostDatatoFTP(filename);

        }
    }
}
