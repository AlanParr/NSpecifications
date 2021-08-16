using NSpecifications.Tests.Entities;
using NUnit.Framework;
using System;

namespace NSpecifications.Tests
{
    [TestFixture]
    public class OrSpecificationTests
    {
        [TestCase(true, false, true)]
        [TestCase(false, true, true)]
        [TestCase(false, false, false)]
        [TestCase(true, true, true)]
        public void NullChecks_ThrowWhenTheyShould(bool spec1Null, bool spec2Null, bool expectException)
        {
            ISpecification<Drink> spec1 = spec1Null ? null : new Spec<Drink>(d => d.Name.ToLower().Contains("juice"));
            ISpecification<Drink> spec2 = spec2Null ? null : new Spec<Drink>(d => d.Name.ToLower().Contains("juice"));

            if (expectException)
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    spec1.Or(spec2);
                });
            }
            else
            {
                var result = spec1.Or(spec2);
                Assert.IsNotNull(result);
            }
        }

        [TestCase("juice", "banana", false)]
        [TestCase("juice", "whiskey", true)]
        [TestCase("whiskey", "apple", true)]
        public void IsSatisfiedBy_WorksAsExpected(string spec1FilterValue, string spec2FilterValue, bool expectedResult)
        {
            ISpecification<Drink> spec1 = new Spec<Drink>(d => d.Name.ToLower().Contains(spec1FilterValue));
            ISpecification<Drink> spec2 = new Spec<Drink>(d => d.Name.ToLower().Contains(spec2FilterValue));

            var orSpec = spec1.Or(spec2);

            Assert.AreEqual(expectedResult, orSpec.IsSatisfiedBy(Drink.ColdWhiskey()));
        }

        [TestCase("juice", "banana", false)]
        [TestCase("juice", "whiskey", true)]
        [TestCase("whiskey", "apple", true)]
        public void Expression_WorksAsExpected(string spec1FilterValue, string spec2FilterValue, bool expectedResult)
        {
            ISpecification<Drink> spec1 = new Spec<Drink>(d => d.Name.ToLower().Contains(spec1FilterValue));
            ISpecification<Drink> spec2 = new Spec<Drink>(d => d.Name.ToLower().Contains(spec2FilterValue));

            var orSpec = spec1.Or(spec2);

            Assert.AreEqual(expectedResult, orSpec.Expression.Compile().Invoke(Drink.ColdWhiskey()));
        }
    }
}
