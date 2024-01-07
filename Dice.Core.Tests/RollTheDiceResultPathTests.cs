namespace Dice.Core.Tests;

public class RollTheDiceResultPathTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(RollTheDiceResultPath).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(RollTheDiceResultPath sut)
    {
        sut.Should().BeAssignableTo<IRollTheDiceResultPath>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(RollTheDiceResultPath).GetMethods().Where(method => !method.IsAbstract & !method.Name.StartsWith("set")));
    }
}