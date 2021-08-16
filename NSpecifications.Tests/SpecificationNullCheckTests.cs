using NSpecifications.Tests.Entities;
using NUnit.Framework;
using System;

namespace NSpecifications.Tests
{
    [TestFixture]
    public class SpecificationNullCheckTests
    {
        [TestCase(true, false, true)]
        [TestCase(false, true, true)]
        [TestCase(false, false, false)]
        [TestCase(true, true, true)]
        public void OrSpecificationNullCheck(bool spec1Null, bool spec2Null, bool expectException)
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
    }
}
