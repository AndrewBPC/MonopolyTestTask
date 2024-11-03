using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Domain.Models.Warehouse
{
    public abstract class StorageItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int Length { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }
        public virtual int Weight { get; set; }

        public virtual DateOnly? expirationDate { get; set; }


        public StorageItem(int length, int width, int height)
        {
            this.Length = length;
            this.Height = height;
            this.Width = width;
            
        }
        public virtual double CalculateVolume()
        {
            return Width * Height * Length;
        }
    }
}
