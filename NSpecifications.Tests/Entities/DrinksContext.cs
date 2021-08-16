using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpecifications.Tests.Entities
{
    /// <summary>
    /// This is a very basic facsimile of a context used by ORMs such as Entity Framework.
    /// </summary>
    public class DrinksContext
    {
        public IQueryable<Drink> Drinks => new List<Drink>()
            { Drink.AppleJuice(), Drink.BlackberryJuice(), Drink.ColdWhiskey(), Drink.OrangeJuice() }.AsQueryable();
    }
}
