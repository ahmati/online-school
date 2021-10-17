using Dapper;
using OnlineSchool.Domain.OnlineSchoolDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Contacts_.Repository
{
   //public class ContactRepository : OnlineSchoolBaseRepository, IContactRepository
   // {
   //     public async Task<string> GetMailAsync(int Id)
   //     {
   //         var prms = new DynamicParameters();
   //         prms.Add("@AgencyId ", Id);
   //         try
   //         {

   //             var sQuery = new StringBuilder();
   //             sQuery.AppendLine("Select Email from Agency a ");
   //             sQuery.AppendLine($"Where a.Id = @AgencyId");
   //             var entities = base.Connection.QueryFirstOrDefaultAsync<string>(sQuery.ToString(), prms);
   //             return await entities;


   //         }
   //         catch (Exception e)
   //         {
   //             base.Logger.Error(e, "Errore di imprevisto");
   //             return default(string);
   //         }
   //     }
 //   }
}
