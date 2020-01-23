using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TierraCreative.FtpPurge
{
    class Program
    {
        static void Main(string[] args)
        {
            var directoryname = @"C:\CSV\";
            var filename = string.Format("ELE_TIE_CPU_{0}.csv", System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

            //data db extraction
            List<Models.FilenameFields> fileNameFields = new List<Models.FilenameFields>();
            var data = FtpPurge.Process.Extractions.ExtractPurgeData(out fileNameFields);

            //create CSV file
            if (!Directory.Exists(directoryname))
                Directory.CreateDirectory(directoryname);

            File.WriteAllText(directoryname + filename, data);

            //purge data
            var IsSuccess = FtpPurge.Process.Purges.PurgeData();

            //ftp data
            //if (IsSuccess)
            //    IsSuccess = FtpPurge.Process.Ftps.PostDatatoFTP(directoryname,filename);
        }
    }
}
