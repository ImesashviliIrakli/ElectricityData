using Microsoft.EntityFrameworkCore;

namespace ElectricityData.Models
{
    public class GroupedTinklasModel
    {
        public int Id { get; set; }
        public string Tinklas { get; set; }
        public float? PPlusSum { get; set; }
        public float? PMinusSum { get; set; }
        public DateTime Month { get; set; }
    }
}
