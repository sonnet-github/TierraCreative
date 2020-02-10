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
            Console.WriteLine("DB Extraction Started...");           
            List<Models.FilenameFields> fileNameFields = new List<Models.FilenameFields>();
            var data = FtpPurge.Process.Extractions.ExtractPurgeData(out fileNameFields);
            Console.WriteLine("DB Extraction Ended...");         

            Console.WriteLine("");          

            //create CSV file
            Console.WriteLine("Create CSV file Started...");          
            if (!Directory.Exists(directoryname))
                Directory.CreateDirectory(directoryname);

            File.WriteAllText(directoryname + filename, data);
            Console.WriteLine("Create CSV file Ended..");
            
            //purge data
            Console.WriteLine("Purge DB data Started...");
            var IsSuccess = FtpPurge.Process.Purges.PurgeData();
            Console.WriteLine("Purge DB data Ended...");

            ////ftp data
            //Console.WriteLine("FTP CSV file Started...");
            //if (IsSuccess)
            //    IsSuccess = FtpPurge.Process.Ftps.PostDatatoFTP(directoryname,filename);

            //Console.WriteLine("FTP CSV file Ended...");


        }
    }
}
