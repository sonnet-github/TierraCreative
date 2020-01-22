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
            var filename = "ELE_TIE_CPU_.csv";

            //data db extraction
            List<Models.FilenameFields> fileNameFields = new List<Models.FilenameFields>();
            var data = FtpPurge.Process.Extractions.ExtractPurgeData(out fileNameFields);

            //create CSV file
            if (!Directory.Exists(directoryname))
                Directory.CreateDirectory(directoryname);

            File.WriteAllText(directoryname + filename, data);

            //ftp data
            var IsSuccess = FtpPurge.Process.Ftps.PostDatatoFTP(filename);

        }
    }
}
