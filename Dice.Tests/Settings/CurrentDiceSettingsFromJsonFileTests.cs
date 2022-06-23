using System.Linq;
using AutoFixture.Idioms;
using Dice.Core.Settings;
using EvilBaschdi.Testing;
using FluentAssertions;
using Xunit;

namespace Dice.Core.Tests.Settings
{
    public class CurrentDiceSettingsFromJsonFileTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(CurrentDiceSettingsFromJsonFile).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(CurrentDiceSettingsFromJsonFile sut)
        {
            sut.Should().BeAssignableTo<ICurrentDiceSettingsFromJsonFile>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(CurrentDiceSettingsFromJsonFile).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}