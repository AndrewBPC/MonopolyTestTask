using ConsoleApp.Domain.Models.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Domain.Interfaces
{
    public interface IStorageService
    {
        public Task AddBoxAsync(Box box);

        public Task AddPalletAsync(Pallet pallet);

        public Task AddBoxToPalleteAsync(int palleteId, Box box);

        public Task<List<IGrouping<DateOnly?, Pallet>>> GetGroupedAndSortedPalletsAsync();
        public Task<List<Pallet>> GetPalletsWithBiggestLifeSorted(int limit);

    }
}
