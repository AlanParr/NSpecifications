using NSpecifications.Tests.Entities;
using NUnit.Framework;
using System;

namespace NSpecifications.Tests
{
    [TestFixture]
    public class NotSpecificationTests
    {
        [TestCase(false, false)]
        [TestCase(true, true)]
        public void NotSpecificationNullCheck(bool spec1Null, bool expectException)
        {
            ISpecification<Drink> spec1 = spec1Null ? null : new Spec<Drink>(d => d.Name.ToLower().Contains("juice"));

            if (expectException)
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    spec1.Not();
                });
            }
            else
            {
                var result = spec1.Not();
                Assert.IsNotNull(result);
            }
        }

        [TestCase("juice", true)]
        [TestCase("whiskey", false)]
        public void IsSatisfiedBy_WorksAsExpected(string spec1FilterValue, bool expectedResult)
        {
            ISpecification<Drink> spec1 = new Spec<Drink>(d => d.Name.ToLower().Contains(spec1FilterValue));

            var orSpec = spec1.Not();

            Assert.AreEqual(expectedResult, orSpec.IsSatisfiedBy(Drink.ColdWhiskey()));
        }

        [TestCase("juice", true)]
        [TestCase("whiskey", false)]
        public void Expression_WorksAsExpected(string spec1FilterValue, bool expectedResult)
        {
            ISpecification<Drink> spec1 = new Spec<Drink>(d => d.Name.ToLower().Contains(spec1FilterValue));

            var orSpec = spec1.Not();

            Assert.AreEqual(expectedResult, orSpec.Expression.Compile().Invoke(Drink.ColdWhiskey()));
        }
    }
}
