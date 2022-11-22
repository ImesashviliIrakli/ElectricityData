using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace Enitites
{
    public class ElectricityModel
    {
        [Key]
        public int Id { get; set; }
        public string TINKLAS { get; set; }
        public string OBT_PAVADINIMAS { get; set; }
        public string OBJ_GV_TIPAS { get; set; }
        public int OBJ_NUMERIS { get; set; }
        [Name("P+")]
        public float? PPlus { get; set; }
        public DateTime PL_T { get; set; }
        [Name("P-")]
        public float? PMINUS { get; set; }
    }
}