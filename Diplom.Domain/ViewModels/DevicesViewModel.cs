using Diplom.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Domain.ViewModels
{
    public class DevicesViewModel
    {
        public int Id { get; set; }

        public string Model { get; set; } = null!;

        public string Manufacturer { get; set; } = null!;

        public string Type { get; set; } = null!;

        public short Year_of_release { get; set; }

        public int? Serial_number { get; set; }

        public int ClientId { get; set; }

        public string? Photo { get; set; }

        public virtual Client Client { get; set; } = null!;
    }
}
