using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SW.ExcelImport.UnitTests.TypeBased
{
       public class Weight
    {
        [Required]
        public string Unit { get; set; }
        public decimal Value { get; set; }
    }
    public class Order
    {
        [Required]
        public string Number { get; set; }
        public string UserNumber { get; set; }
        [StringLength(10)]
        public string Customer { get; set; }

        public string[] Emails {get; set;}
        public string[] People {get; set;}
        public int[] Phones { get; set; }
        [Required]
        public ICollection<Item> Items { get; set; }
        public ICollection<Other> OtherData { get; set; }
        public Weight Weight { get; set; }

    }

    public class Item
    {
        [Required]
        [StringLength(10)]
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public int Quantity { get; set; }
    }

    public class Other
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
    }
}