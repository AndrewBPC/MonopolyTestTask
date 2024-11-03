using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Domain.Models.Warehouse
{
    public class Box : StorageItem
    {
        public DateOnly productionDate { get; set; } // Дата производства

        public int PalleteId { get; set; }
        public Pallet Pallet { get; set; }

        private int exprirationLife { get => 100; } // Срок годности

        public override int Weight { get => base.Weight; set => base.Weight = value; }

        public override DateOnly? expirationDate { get => productionDate.AddDays(exprirationLife); }
        public Box(int length, int width,int height, int weight, DateOnly productionDate) : base(length, height, width)
        {
            this.productionDate = productionDate;
            this.Weight = weight;
        }


    }
}
