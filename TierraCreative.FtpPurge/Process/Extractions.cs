﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TierraCreative.FtpPurge.Process
{
    public static class Extractions
    {
        public static string ExtractPurgeData(string filename) {
            TierraCreativeContext _context = new TierraCreativeContext();

            var ctr = 1;
            string data = string.Empty;

            var Main_Account = _context.CSNLookUps.SingleOrDefault(x => x.CSNName == "Main").CSNAccount;
            var AIL_Account = _context.CSNLookUps.SingleOrDefault(x => x.CSNName == "AIL").CSNAccount;
            var SP_Account = _context.CSNLookUps.SingleOrDefault(x => x.CSNName == "Supplementary Dividend").CSNAccount;

            List<Models.FilenameFields> fileNameFields = new List<Models.FilenameFields>();

            try {
                var drps = _context.DRPs.ToList();
                foreach (var drp in drps) {
                    var fileNameField = new Models.FilenameFields
                    {
                        Sequence = ctr.ToString(),
                        Timestamp = drp.CreatedDate.ToString(),
                        MainCSN = Main_Account,
                        CSN = "",
                        ISIN = drp.ISIN,
                        InstructionID = "DERI",
                        TIN = "",
                        TransferDate = System.DateTime.Now.ToString(),
                        TransferQty = drp.DRPAmount.ToString()
                    };
                    fileNameFields.Add(fileNameField);

                    ctr += 1;
                }


                var ails = _context.AILs.ToList();
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

                var sps = _context.SupplementaryDividends.ToList();
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

                //create file and purge
                foreach (var fileNameField in fileNameFields)
                {

                }

            }
            catch { }

            return data;
        }
    }
}
