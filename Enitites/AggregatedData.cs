using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enitites
{
    public class AggregatedData
    {
        [Key]
        public int Id { get; set; }
        public string Tinklas { get; set; }
        public float? PPlusSum { get; set; }
        public float? PMinusSum { get; set; }
        public DateTime Date { get; set; }
    }
}

