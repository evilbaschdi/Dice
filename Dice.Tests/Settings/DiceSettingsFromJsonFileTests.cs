using Dice.Core.Settings;

namespace Dice.Core.Tests.Settings
{
    public class DiceSettingsFromJsonFileTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DiceSettingsFromJsonFile).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(DiceSettingsFromJsonFile sut)
        {
            sut.Should().BeAssignableTo<IDiceSettingsFromJsonFile>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DiceSettingsFromJsonFile).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}