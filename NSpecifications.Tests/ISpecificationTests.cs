using System.Linq;
using FluentAssertions;
using NSpecifications.Tests.Entities;
using NUnit.Framework;

namespace NSpecifications.Tests
{
    [TestFixture]
    public class ISpecificationTests
    {
        [Test]
        public void WhiskeyAndCold()
        {
            // Arrange
            var coldWhiskey = Drink.ColdWhiskey();
            var appleJuice = Drink.AppleJuice();
            ISpecification<Drink> whiskeySpec = new Spec<Drink>(d => d.Name.ToLower() == "whiskey");
            ISpecification<Drink> coldSpec = new Spec<Drink>(d => d.With.Any(w => w.ToLower() == "ice"));

            // Act
            var coldWhiskeySpec = whiskeySpec.And(coldSpec);

            // Assert
            coldWhiskeySpec.IsSatisfiedBy(coldWhiskey).Should().BeTrue();
            coldWhiskeySpec.IsSatisfiedBy(appleJuice).Should().BeFalse();
        }

        [Test]
        public void AppleOrOrangeJuice()
        {
            // Arrange
            var blackberryJuice = Drink.BlackberryJuice();
            var appleJuice = Drink.AppleJuice();
            var orangeJuice = Drink.OrangeJuice();
            ISpecification<Drink> juiceSpec = new Spec<Drink>(d => d.Name.ToLower().Contains("juice"));
            ISpecification<Drink> appleSpec = new Spec<Drink>(d => d.Name.ToLower().Contains("apple"));
            ISpecification<Drink> orangeSpec = new Spec<Drink>(d => d.Name.ToLower().Contains("orange"));

            // Act
            var appleOrOrangeJuiceSpec = juiceSpec.And(appleSpec.Or(orangeSpec));

            // Assert
            appleOrOrangeJuiceSpec.IsSatisfiedBy(appleJuice).Should().BeTrue();
            appleOrOrangeJuiceSpec.IsSatisfiedBy(orangeJuice).Should().BeTrue();
            appleOrOrangeJuiceSpec.IsSatisfiedBy(blackberryJuice).Should().BeFalse();
        }

        [Test]
        public void And()
        {
            // Arrange
            var coldWhiskey = Drink.ColdWhiskey();
            var appleJuice = Drink.AppleJuice();
            ISpecification<Drink> whiskeySpec = new Spec<Drink>(d => d.Name.ToLower() == "whiskey");
            ISpecification<Drink> coldSpec = new Spec<Drink>(d => d.With.Any(a => a.ToLower() == "ice"));

            // Act
            var coldWhiskeySpec = whiskeySpec.And(coldSpec);

            // Assert
            coldWhiskeySpec.IsSatisfiedBy(coldWhiskey).Should().BeTrue();
            coldWhiskeySpec.IsSatisfiedBy(appleJuice).Should().BeFalse();
        }

        
        [Test]
        public void Or()
        {
            // Arrange
            var coldWhiskey = Drink.ColdWhiskey();
            var appleJuice = Drink.AppleJuice();
            var blackberryJuice = Drink.BlackberryJuice();
            ASpec<Drink> whiskeySpec = new Spec<Drink>(d => d.Name.ToLower() == "whiskey");
            ASpec<Drink> appleJuiceSpec = new Spec<Drink>(d => d.Name.ToLower() == "apple juice");

            // Act
            var whiskeyOrAppleJuiceSpec = whiskeySpec.Or(appleJuiceSpec);

            // Assert
            whiskeyOrAppleJuiceSpec.IsSatisfiedBy(coldWhiskey).Should().BeTrue();
            whiskeyOrAppleJuiceSpec.IsSatisfiedBy(appleJuice).Should().BeTrue();
            whiskeyOrAppleJuiceSpec.IsSatisfiedBy(blackberryJuice).Should().BeFalse();
            // And
            coldWhiskey.Is(whiskeyOrAppleJuiceSpec).Should().BeTrue();
            appleJuice.Is(whiskeyOrAppleJuiceSpec).Should().BeTrue();
            blackberryJuice.Is(whiskeyOrAppleJuiceSpec).Should().BeFalse();
        }
    }
}
