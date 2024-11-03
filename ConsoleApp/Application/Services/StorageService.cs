using ConsoleApp.Application.Common.Helpers;
using ConsoleApp.Domain.Interfaces;
using ConsoleApp.Domain.Models.Warehouse;
using ConsoleApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Application.Services
{
    public class StorageService : IStorageService
    {
        private readonly ApplicationDbContext _context;
        public StorageService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddBoxAsync(Box box)
        {
            _context.Boxes.Add(box);
            await _context.SaveChangesAsync();
            
        }
        public async Task AddPalletAsync(Pallet pallet)
        {
            _context.Pallets.Add(pallet);
            await _context.SaveChangesAsync();
            
        }
        public async Task AddBoxToPalleteAsync(int palleteId, Box box)
        {
            var pallet = await _context.Pallets.Include(x => x.Boxes).FirstOrDefaultAsync(x => x.Id == palleteId);
            if(pallet is null)
            {
                throw new Exception("Pallete not found");
            }

            pallet.AddBox(box);
            await _context.SaveChangesAsync();
        }


        public async Task<List<IGrouping<DateOnly?, Pallet>>> GetGroupedAndSortedPalletsAsync()
        {
            List<Pallet> pallets = await _context.Pallets
        .Include(x => x.Boxes)
        
        .ToListAsync();

            var groupedPallets = pallets
            .GroupBy(p => p.expirationDate)
            .OrderBy(g => g.Key) // Sort groups by expiration date
            .Select(g => new Grouping<DateOnly?, Pallet>(g.Key, g.OrderBy(p => p.Weight)))
            .Cast<IGrouping<DateOnly?, Pallet>>()
            .ToList();

            return groupedPallets;
        }

        public async Task<List<Pallet>> GetPalletsWithBiggestLifeSorted(int limit)
        {
            List<Pallet> pallets = await _context.Pallets.Include(x => x.Boxes)
                .OrderBy(x => x.expirationDate)
                .Take(limit)
               .ToListAsync();

            return pallets;
        }
    }
}
