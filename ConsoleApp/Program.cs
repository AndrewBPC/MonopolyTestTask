
using ConsoleApp.Application.Services;
using ConsoleApp.Domain.Models.Warehouse;
using ConsoleApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            Console.WriteLine("Старт программы!");
            var serviceProvider = new ServiceCollection()
            .AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("TestDb"))
            .BuildServiceProvider();

            var context = serviceProvider.GetService<ApplicationDbContext>();
            var storageService = new StorageService(context);

            var now = DateOnly.FromDateTime(DateTime.UtcNow);
            
            
            var boxes = new List<Box>()
            {
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-24)),
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-22)),
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-20)),
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-24)),
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-20)),
            };

            var pallet = new Pallet(length: 2, width: 2, height: 3);
            await storageService.AddPalletAsync(pallet);
            foreach (var box in boxes)
            {
                try
                {
                    // Можно при желании сделать BulkAdd
                    await storageService.AddBoxToPalleteAsync(pallet.Id, box);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    
                }
                
            }


            var boxes2 = new List<Box>()
            {
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-24)),
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-22)),
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-20)),
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-24)),
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-85)),
            };

            var pallet2 = new Pallet(length: 2, width: 2, height: 3);
            await storageService.AddPalletAsync(pallet2);
            foreach (var box in boxes2)
            {
                try
                {
                    await storageService.AddBoxToPalleteAsync(pallet2.Id, box);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }



            var boxes3 = new List<Box>()
            {
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-24)),
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-22)),
                new Box(length: 1,width: 6,height: 3, weight: 23, productionDate: now.AddDays(-20)),
                new Box(length: 3,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-24)),
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-85)),
            };

            var pallet3 = new Pallet(length: 2, width: 2, height: 3);
            await storageService.AddPalletAsync(pallet3);
            foreach (var box in boxes3)
            {
                try
                {
                    await storageService.AddBoxToPalleteAsync(pallet3.Id, box);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }

            var boxes4 = new List<Box>()
            {
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-24)),
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-22)),
                new Box(length: 1,width: 6,height: 3, weight: 23, productionDate: now.AddDays(-20)),
                new Box(length: 3,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-24)),
                new Box(length: 1,width: 2,height: 3, weight: 23, productionDate: now.AddDays(-110)),
            };

            var pallet4 = new Pallet(length: 2, width: 2, height: 3);
            await storageService.AddPalletAsync(pallet4);
            foreach (var box in boxes4)
            {
                try
                {
                    await storageService.AddBoxToPalleteAsync(pallet4.Id, box);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }

            Console.WriteLine("\n\n- Сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу.");
            var res = await storageService.GetGroupedAndSortedPalletsAsync();
            foreach (var group in res)
            {
                Console.WriteLine($"\n\nexpirationDate: {group.Key}");
                foreach (var item in group)
                {
                    Console.WriteLine($"PalleteId: {item.Id}\tWeight: {item.Weight} l: {item.Length} w: {item.Width} h: {item.Height}");
                }
            }


            Console.WriteLine("\n\n- 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.");

            var newestPallets = await storageService.GetPalletsWithBiggestLifeSorted(limit: 3);
            foreach (var item in newestPallets)
            {
                Console.WriteLine($"\n\nPalleteId: {item.Id} expirationDate: {item.expirationDate}");
            }
        }
    }
}
