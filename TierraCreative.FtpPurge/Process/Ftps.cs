﻿using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace TierraCreative.FtpPurge.Process
{
   public static class Ftps
    {
        public static bool PostDatatoFTP(string directoryname, string filename)
        {
            string source = directoryname + filename;
            string destination = ConfigurationManager.AppSettings["FtpFolder"];
            string host = ConfigurationManager.AppSettings["FtpURL"]; 
            string username = ConfigurationManager.AppSettings["FtpUser"];
            string password = ConfigurationManager.AppSettings["FtpPass"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["FtpPort"]);

            try
            {
                #region -- sftp codes --
                using (SftpClient client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    client.ChangeDirectory(destination);
                    using (FileStream fs = new FileStream(source, FileMode.Open))
                    {
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(fs, Path.GetFileName(source));
                    }
                }
                #endregion

                #region -- ftp codes --
                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ConfigurationManager.AppSettings["FtpURL"] + @"\" + filename);
                //request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheIfAvailable);
                //request.Method = WebRequestMethods.Ftp.UploadFile;
                //request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FtpUser"], ConfigurationManager.AppSettings["FtpPass"]);

                //// Copy the contents of the file to the request stream.  
                ////StreamReader sourceStream = new StreamReader(@"E:\yourlocation\" + filename);
                //StreamReader sourceStream = new StreamReader(directoryname + filename);
                //byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                //sourceStream.Close();
                //request.ContentLength = fileContents.Length;
                //Stream requestStream = request.GetRequestStream();
                //requestStream.Write(fileContents, 0, fileContents.Length);
                //requestStream.Close();

                //FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                //Console.WriteLine("Upload FTP File Complete, status {0}", response.StatusDescription);

                //response.Close();

                #endregion

                return true;
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message.ToString());
                String status = ((FtpWebResponse)e.Response).StatusDescription;
                Console.WriteLine(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            return false;
        }
    }
}
