using SlimFitGym.Models.Models;
using SlimFitGym.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData.Interfaces
{
    public interface IEntriesRepository
    {
        Entry NewEntry(string token, int accountId);
        List<Entry> GetEntriesByAccountId(string token, int accountId, string fromDate = "2025.01.01 00:00:00", int limit = 10, int offset = 0, string orderDirection="desc");
        int GetEntriesCountByUserId(string token, int accountId);
        List<EntryResponse> GetAllEntries(string fromDate = "2025.01.01 00:00:00", int limit = 10, int offset = 0, string orderField = "date", string orderDirection = "desc");
        int GetAllEntriesCount();
    }
}
