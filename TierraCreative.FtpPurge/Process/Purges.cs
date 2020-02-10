using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TierraCreative.FtpPurge.Process
{
    public static class Purges
    {
        public static bool PurgeData() {
            TierraCreativeContext _context = new TierraCreativeContext();

            var sCmd = @"INSERT INTO Purge_AIL(
                            [AILId],
	                        [UserId] ,
	                        [FromCSN],
	                        [ToCSN],
	                        [ISIN] ,
	                        [TransferAmount] ,
	                        [CreatedById],
	                        [CreatedDate] ,
	                        [UpdatedById] ,
	                        [UpdatedDate] ,
	                        [ReviewedById] ,
	                        [ReviewedDate] ,
	                        [DeletedById] ,
	                        [DeletedDate]
                        ) 
                        SELECT * FROM AIL;

                        INSERT INTO Purge_DRP(   
	                        [DRPId],
	                        [UserId],
	                        [CSN],
	                        [ISIN],
	                        [DRPAmount],
	                        [CreatedById],
	                        [CreatedDate] ,
	                        [UpdatedById] ,
	                        [UpdatedDate],
	                        [ReviewedById],
	                        [ReviewedDate],
	                        [DeletedById] ,
	                        [DeletedDate] 
                        ) 
                        SELECT * FROM DRP;

                        INSERT INTO Purge_SupplementaryDividend(   
	                        [SDId],
	                        [UserId],
	                        [FromCSN] ,
	                        [ToCSN] ,
	                        [ISIN],
	                        [TransferAmount],
	                        [CreatedById] ,
	                        [CreatedDate],
	                        [UpdatedById] ,
	                        [UpdatedDate] ,
	                        [ReviewedById] ,
	                        [ReviewedDate],
	                        [DeletedById] ,
	                        [DeletedDate] 
                        ) 
                        SELECT * FROM SupplementaryDividend;

                        DELETE FROM DRP;
                        DELETE FROM SupplementaryDividend;
                        DELETE FROM AIL;
                        ";

            try {

                _context.Database.ExecuteSqlCommand(sCmd);
                _context.SaveChanges();

                return true;
            }
            catch(Exception ex) { }

            return false;
        }
    }
}
