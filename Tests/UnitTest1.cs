using ConsoleApp.Infrastructure.Data;
using Moq;
using ConsoleApp.Domain.Interfaces;
using ConsoleApp.Application.Services;
using ConsoleApp.Domain.Models.Warehouse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace Tests
{
    public class StorageServiceTests
    {
        private readonly ApplicationDbContext _mockContext;
        private readonly IStorageService _storageService;

        public StorageServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            var serviceProvider = new ServiceCollection()
            .AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("TestDb"))
            .BuildServiceProvider();

            var context = serviceProvider.GetService<ApplicationDbContext>();


            _mockContext = new ApplicationDbContext(options);
            _storageService = new StorageService(_mockContext);
        }

        [Fact]
        [Description("Проверяет как обрабатываюся коробки в палетах")]
        public async Task ShouldGetExceptionOnBiggerSize()
        {
            var pallet = new Pallet(1, 2, 3);
            await _storageService.AddPalletAsync(pallet);

            var normalBox = new Box(1, 1, 1, 23, DateOnly.FromDateTime(DateTime.UtcNow));
            
            await _storageService.AddBoxToPalleteAsync(pallet.Id, normalBox);
            

            var biggerBox = new Box(2, 1, 1, 23, DateOnly.FromDateTime(DateTime.UtcNow));
            // Bigger box should throw an exception
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _storageService.AddBoxToPalleteAsync(pallet.Id, biggerBox);
            });

            // Optionally assert the exception message if needed
            Assert.Equal("Размер коробки привышает размер палеты", exception.Message);
        }





        [Fact]
        [Description("Проверяет группировку и сортировку")]
        public async Task GetGroupedAndSortedPalletsAsync_ShouldGroupAndSortPallets()
        {
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
            await _storageService.AddPalletAsync(pallet);
            foreach (var box in boxes)
            {
                try
                {
                    // Можно при желании сделать BulkAdd
                    await _storageService.AddBoxToPalleteAsync(pallet.Id, box);
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
            await _storageService.AddPalletAsync(pallet2);
            foreach (var box in boxes2)
            {
                try
                {
                    await _storageService.AddBoxToPalleteAsync(pallet2.Id, box);
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
            await _storageService.AddPalletAsync(pallet3);
            foreach (var box in boxes3)
            {
                try
                {
                    await _storageService.AddBoxToPalleteAsync(pallet3.Id, box);
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
            await _storageService.AddPalletAsync(pallet4);
            foreach (var box in boxes4)
            {
                try
                {
                    await _storageService.AddBoxToPalleteAsync(pallet4.Id, box);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }

            Console.WriteLine("\n\n- Сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу.");
            var groups = await _storageService.GetGroupedAndSortedPalletsAsync();
            foreach (var group in groups)
            {
                Console.WriteLine($"\n\nexpirationDate: {group.Key}");
                foreach (var item in group)
                {
                    Console.WriteLine($"PalleteId: {item.Id}\tWeight: {item.Weight} l: {item.Length} w: {item.Width} h: {item.Height}");
                }
            }
            Assert.Equal(3, groups.Count); // 3 группы
            Assert.Equal(DateOnly.FromDateTime(new DateTime(2024, 10, 23)), groups[0].Key); // Первая группа
            Assert.Equal(DateOnly.FromDateTime(new DateTime(2024, 11, 17)), groups[1].Key); // Вторая группа

            Assert.Equal(1, groups[0].Count()); // В первой группе 1 палета
            Assert.Equal(2, groups[1].Count()); // Во второй группе 2 палеты

            // Проверка сортировки по весу
            Assert.True(groups[1].ElementAt(0).Weight < groups[1].ElementAt(1).Weight);
        }
    }
}