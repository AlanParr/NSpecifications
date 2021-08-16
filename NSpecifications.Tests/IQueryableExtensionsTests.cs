using System.Linq;
using FluentAssertions;
using NSpecifications.Tests.Entities;
using NUnit.Framework;

namespace NSpecifications.Tests
{
    [TestFixture(Description = "Tests IEnumerable extensions against a plain lambda to ensure the results are the same.")]
    public class IQueryableExtensionsTests
    {
        private ISpecification<Drink> _juiceSpec;
        private ISpecification<Drink> _whiskeySpec;
        private ISpecification<Drink> _appleSpec;
        private DrinksContext _drinksContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            _juiceSpec = new Spec<Drink>(d => d.Name.ToLower().Contains("juice"));
            _whiskeySpec = new Spec<Drink>(d => d.Name.ToLower().Contains("whiskey"));
            _appleSpec = new Spec<Drink>(d => d.Name.ToLower().Contains("apple"));
            _drinksContext = new DrinksContext();
        }

        [Test]
        public void WhereWithOrSpecification()
        {
            var lambdaResult = _drinksContext.Drinks.Where(x => x.Name.ToLower().Contains("juice") || x.Name.ToLower().Contains("whiskey"));
            var specResult = _drinksContext.Drinks.Where(_juiceSpec.Or(_whiskeySpec));

            lambdaResult.Should().BeEquivalentTo(specResult, o => o.Excluding(c => c.ManufacturedOn));
        }

        [Test]
        public void WhereWithAndSpecification()
        {
            var lambdaResult = _drinksContext.Drinks.Where(x => x.Name.ToLower().Contains("juice") && x.Name.ToLower().Contains("apple"));
            var specResult = _drinksContext.Drinks.Where(_juiceSpec.And(_appleSpec));

            lambdaResult.Should().BeEquivalentTo(specResult, o => o.Excluding(c => c.ManufacturedOn));
        }

        [Test]
        public void WhereWithNotSpecification()
        {
            var lambdaResult = _drinksContext.Drinks.Where(x => !x.Name.ToLower().Contains("juice"));
            var specResult = _drinksContext.Drinks.Where(_juiceSpec.Not());

            lambdaResult.Should().BeEquivalentTo(specResult, o => o.Excluding(c => c.ManufacturedOn));
        }

        [Test]
        public void OrderByWithOrSpecification()
        {
            var lambdaResult = _drinksContext.Drinks.OrderBy(x =>
                x.Name.ToLower().Contains("juice") || x.Name.ToLower().Contains("whiskey"));

            var specResult = _drinksContext.Drinks.OrderBy(_juiceSpec.Or(_whiskeySpec));

            lambdaResult.Should().BeEquivalentTo(specResult, o=>o.Excluding(c=>c.ManufacturedOn));
        }
    }
}
