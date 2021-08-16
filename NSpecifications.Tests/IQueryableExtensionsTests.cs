using System.Linq;
using FluentAssertions;
using NSpecifications.Tests.Entities;
using NUnit.Framework;

namespace NSpecifications.Tests
{
    [TestFixture(Description = "Tests IQueryable extensions against a plain lambda and Specs with operators to ensure the results are the same.")]
    public class IQueryableExtensionsTests
    {
        private Spec<Drink> _juiceSpec;
        private Spec<Drink> _whiskeySpec;
        private Spec<Drink> _appleSpec;
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
            var specOperatorResult = _drinksContext.Drinks.Where(_juiceSpec | _whiskeySpec);
            var specExtensionsResult = _drinksContext.Drinks.Where(_juiceSpec.Or(_whiskeySpec));

            lambdaResult.Should().BeEquivalentTo(specOperatorResult, o => o.Excluding(c => c.ManufacturedOn));
            lambdaResult.Should().BeEquivalentTo(specExtensionsResult, o => o.Excluding(c => c.ManufacturedOn));
        }

        [Test]
        public void WhereWithAndSpecification()
        {
            var lambdaResult = _drinksContext.Drinks.Where(x => x.Name.ToLower().Contains("juice") && x.Name.ToLower().Contains("apple"));
            var specOperatorResult = _drinksContext.Drinks.Where(_juiceSpec & _appleSpec);
            var specExtensionsResult = _drinksContext.Drinks.Where(_juiceSpec.And(_appleSpec));

            lambdaResult.Should().BeEquivalentTo(specOperatorResult, o => o.Excluding(c => c.ManufacturedOn));
            lambdaResult.Should().BeEquivalentTo(specExtensionsResult, o => o.Excluding(c => c.ManufacturedOn));
        }

        [Test]
        public void WhereWithNotSpecification()
        {
            var lambdaResult = _drinksContext.Drinks.Where(x => !x.Name.ToLower().Contains("juice"));
            var specOperatorResult = _drinksContext.Drinks.Where(!_juiceSpec);
            var specExtensionsResult = _drinksContext.Drinks.Where(_juiceSpec.Not());

            lambdaResult.Should().BeEquivalentTo(specOperatorResult, o => o.Excluding(c => c.ManufacturedOn));
            lambdaResult.Should().BeEquivalentTo(specExtensionsResult, o => o.Excluding(c => c.ManufacturedOn));
        }

        [Test]
        public void OrderByWithOrSpecification()
        {
            var lambdaResult = _drinksContext.Drinks.OrderBy(x =>
                x.Name.ToLower().Contains("juice") || x.Name.ToLower().Contains("whiskey"));

            var specOperatorResult = _drinksContext.Drinks.OrderBy(_juiceSpec | _whiskeySpec);
            var specExtensionsResult = _drinksContext.Drinks.OrderBy(_juiceSpec.Or(_whiskeySpec));

            lambdaResult.Should().BeEquivalentTo(specOperatorResult, o=>o.Excluding(c=>c.ManufacturedOn));
            lambdaResult.Should().BeEquivalentTo(specExtensionsResult, o=>o.Excluding(c=>c.ManufacturedOn));
        }
    }
}
