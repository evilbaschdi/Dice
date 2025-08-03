using Dice.Core.Settings;

namespace Dice.Core.Tests;

public class RollTheDiceTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(RollTheDice).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(RollTheDice sut)
    {
        sut.Should().BeAssignableTo<IRollTheDice>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(RollTheDice).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task Value_ForProvidedInformation_ReturnsDicedPathInformation(
        [Frozen] IDicePath dicePath,
        [Frozen] IRollTheDiceResultPath rollTheDiceResultPath,
        [Frozen] IInitialDirectoryFromSettings initialDirectoryFromSettings,
        RollTheDice sut,
        string dummyInitialDirectoryFromSettings,
        string dummyResult)
    {
        // Arrange
        initialDirectoryFromSettings.Value.Returns(dummyInitialDirectoryFromSettings);
        dicePath.ValueForAsync(dummyInitialDirectoryFromSettings).Returns(Task.FromResult(dummyResult));

        // Act
        var result = await sut.ValueAsync();

        // Assert
        rollTheDiceResultPath.Value.Should().Be(dummyResult);
        result.Should().StartWith($"'{dummyResult}'");
        result.Should().EndWith("[roll the dice again]");
    }
}