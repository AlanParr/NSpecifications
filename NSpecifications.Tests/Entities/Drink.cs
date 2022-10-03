using System;
using System.Collections.Generic;

namespace NSpecifications.Tests.Entities
{
    public class Drink
    {
        public Drink()
        {
            With = new List<string>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime ManufacturedOn { get; set; }

        public List<string> With { get; set; }

        internal static Drink ColdWhiskey(int id = default(int))
        {
            return new Drink
            {
                Id = id,
                Name = "Whiskey",
                With = { "Ice" },
                ManufacturedOn = DateTime.Now.AddYears(-11),
            };
        }

        internal static Drink AppleJuice(int id = default(int))
        {
            return new Drink
            {
                Id = id,
                Name = "Apple Juice",
                ManufacturedOn = DateTime.Now.AddMonths(-1),
            };
        }

        internal static Drink OrangeJuice(int id = default(int))
        {
            return new Drink
            {
                Id = id,
                Name = "Orange Juice",
                ManufacturedOn = DateTime.Now.AddMonths(-1),
            };
        }

        internal static Drink BlackberryJuice(int id = default(int))
        {
            return new Drink
            {
                Id = id,
                Name = "Blackberry Juice",
                ManufacturedOn = DateTime.Now.AddMonths(-1),
            };
        }
    }
}
