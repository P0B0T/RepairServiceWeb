using Diplom.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Domain.ViewModels
{
    public class RepairsViewModel
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }

        public int StaffId { get; set; }

        public DateOnly Date_of_admission { get; set; }

        public DateOnly End_date { get; set; }

        public decimal Cost { get; set; }

        public string Description_of_problem { get; set; } = null!;

        public string Descriprion_of_work_done { get; set; } = null!;

        public virtual Staff? Staff { get; set; }

        public virtual Device? Device { get; set; }
    }
}
