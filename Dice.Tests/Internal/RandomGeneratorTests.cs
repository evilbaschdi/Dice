using System.Linq;
using AutoFixture.Idioms;
using Dice.Internal;
using EvilBaschdi.Testing;
using FluentAssertions;
using Xunit;

namespace Dice.Tests.Internal
{
    public class RandomGeneratorTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(RandomGenerator).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(RandomGenerator sut)
        {
            sut.Should().BeAssignableTo<IRandomGenerator>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(RandomGenerator).GetMethods().Where(method => !method.IsAbstract));
        }

        [Theory]
        [NSubstituteOmitAutoPropertiesTrueInlineAutoData(1, 9)]
        [NSubstituteOmitAutoPropertiesTrueInlineAutoData(0, 275)]
        [NSubstituteOmitAutoPropertiesTrueInlineAutoData(4096, 8072)]
        public void ValueFor_ForProvidedMinAndMax_ReturnsValueBetweenMinAndMax(
            int min,
            int max,
            RandomGenerator sut)
        {
            // Arrange


            // Act
            var result = sut.ValueFor(min, max);

            // Assert
            result.Should().BeGreaterOrEqualTo(min);
            result.Should().BeLessOrEqualTo(max);
        }
    }
}