using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TierraCreative.FtpPurge.Process
{
    public static class Extractions
    {
        public static string ExtractPurgeData(out List<Models.FilenameFields> fileNameFields) {
            TierraCreativeContext _context = new TierraCreativeContext();

            fileNameFields = new List<Models.FilenameFields>();

            var ctr = 1;
            string data = string.Empty;

            var Main_Account = _context.CSNLookUps.SingleOrDefault(x => x.CSNName == "Main").CSNAccount;
            var AIL_Account = _context.CSNLookUps.SingleOrDefault(x => x.CSNName == "AIL").CSNAccount;
            var SP_Account = _context.CSNLookUps.SingleOrDefault(x => x.CSNName == "Supplementary Dividend").CSNAccount;
            
            try {
                //var drps = _context.DRPs.ToList();
                //foreach (var drp in drps) {
                //    var fileNameField = new Models.FilenameFields
                //    {
                //        Sequence = ctr.ToString(),
                //        Timestamp = drp.CreatedDate.ToString(),
                //        MainCSN = Main_Account,
                //        CSN = "",
                //        ISIN = drp.ISIN,
                //        InstructionID = "DERI",
                //        TIN = "",
                //        TransferDate = System.DateTime.Now.ToString(),
                //        TransferQty = drp.DRPAmount.ToString()
                //    };
                //    fileNameFields.Add(fileNameField);

                //    ctr += 1;
                //}
                
                var ails = _context.AILs.Where(x=>x.ReviewedDate!=null).ToList();
                foreach (var ail in ails) {
                    var fileNameField = new Models.FilenameFields
                    {
                        Sequence = ctr.ToString(),
                        Timestamp = ail.CreatedDate.ToString(),
                        MainCSN = Main_Account,
                        CSN = AIL_Account,
                        ISIN = ail.ISIN,
                        InstructionID = "RERE",
                        TIN ="",
                        TransferDate = System.DateTime.Now.ToString(),
                        TransferQty = ail.TransferAmount.ToString()
                    };
                    fileNameFields.Add(fileNameField);

                    ctr += 1;
                }

                var sps = _context.SupplementaryDividends.Where(x => x.ReviewedDate != null).ToList();
                foreach (var sp in sps) {
                    var fileNameField = new Models.FilenameFields
                    {
                        Sequence = ctr.ToString(),
                        Timestamp = sp.CreatedDate.ToString(),
                        MainCSN = Main_Account,
                        CSN = SP_Account,
                        ISIN = sp.ISIN,
                        InstructionID = "RERE",
                        TIN = "",
                        TransferDate = System.DateTime.Now.ToString(),
                        TransferQty = sp.TransferAmount.ToString()
                    };
                    fileNameFields.Add(fileNameField);

                    ctr += 1;
                }

                //concat data
                var setting = _context.Settings.SingleOrDefault(x => x.Description == "lastpurgeid");
                var lastpurgeid = double.Parse(setting.Value);

                foreach (var fileNameField in fileNameFields)
                {

                    //1,2019-12-09-16-35-21,220001091,123456789,NZF01DT123C1,DERI,2019-12-09,10000.50,
                    lastpurgeid += 1;

                    data += string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}{9}",
                                        lastpurgeid,
                                        Convert.ToDateTime(fileNameField.Timestamp).ToString("yyyy-dd-MM-HH-mm-ss"),
                                        fileNameField.MainCSN,
                                        fileNameField.CSN,
                                        fileNameField.ISIN,
                                        fileNameField.InstructionID,                                     
                                        Convert.ToDateTime(fileNameField.TransferDate).ToString("yyyy-dd-MM"),
                                        Convert.ToDouble(fileNameField.TransferQty).ToString("##.00"),
                                        fileNameField.TIN,
                                        Environment.NewLine);
                }

                setting.Value = lastpurgeid.ToString();

                _context.Entry(setting).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();

            }
            catch { }

            return data;
        }
    }
}
