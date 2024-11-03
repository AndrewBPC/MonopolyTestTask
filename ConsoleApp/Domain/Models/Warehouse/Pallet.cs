using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Domain.Models.Warehouse
{
    public class Pallet : StorageItem
    {
        private readonly List<Box> _boxes = new List<Box>();  // изменяемая коллекция только внутри класса

        public IReadOnlyList<Box> Boxes => _boxes;  // доступ только для чтения извне

        public override int Weight { get => Boxes.Sum(x => x.Weight); }

        public override DateOnly? expirationDate { get => Boxes.Count > 0 ? Boxes.Min(x => x.expirationDate) : null; }
        public Pallet(int length, int width, int height) : base(length, height, width)
        {

        }

        public void AddBox(Box box)
        {
            if (box.Length > Length || box.Width > Width)
            {
                
                throw new Exception("Размер коробки привышает размер палеты");
            }
            _boxes.Add(box);
            
        }


        public override double CalculateVolume()
        {
            return Boxes.Sum(box => box.CalculateVolume());

            //return base.CalculateVolume();
        }

    }
}
