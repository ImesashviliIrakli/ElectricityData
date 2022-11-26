using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enitites.ViewModels
{
    public class AggregatedDataViewModel
    {
        public string Tinklas { get; set; }
        public float? PPlusSum { get; set; }
        public float? PMinusSum { get; set; }
        public string Month { get; set; }
    }
}
