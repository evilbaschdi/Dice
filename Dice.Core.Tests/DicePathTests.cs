using EvilBaschdi.Core.Internal;

namespace Dice.Core.Tests;

public class DicePathTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DicePath).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(DicePath sut)
    {
        sut.Should().BeAssignableTo<IDicePath>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task ValueFor_ForProvidedInitialDirectory_ListNull_ReturnsString(
        [Frozen] IFileListFromPath fileListFromPath,
        DicePath sut,
        string dummyInitialDirectory)
    {
        // Arrange
        fileListFromPath.GetSubdirectoriesContainingOnlyFiles(dummyInitialDirectory).ReturnsNull();

        // Act
        var result = await sut.ValueForAsync(dummyInitialDirectory);

        // Assert
        result.Should().Be("directory is empty");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task ValueFor_ForProvidedInitialDirectory_ListEmpty_ReturnsString(
        [Frozen] IFileListFromPath fileListFromPath,
        DicePath sut,
        string dummyInitialDirectory)
    {
        // Arrange
        fileListFromPath.GetSubdirectoriesContainingOnlyFiles(dummyInitialDirectory).Returns(new List<string>());

        // Act
        var result = await sut.ValueForAsync(dummyInitialDirectory);

        // Assert
        result.Should().Be("directory is empty");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task ValueFor_ForProvidedInitialDirectory_ReturnsRandomPath(
        [Frozen] IFileListFromPath fileListFromPath,
        DicePath sut,
        string dummyInitialDirectory)
    {
        // Arrange
        var dummyFolderList = new List<string>
                              {
                                  "zero", "one", "two", "three", "four", "five"
                              };

        fileListFromPath.GetSubdirectoriesContainingOnlyFiles(dummyInitialDirectory).Returns(dummyFolderList);

        // Act
        var result = await sut.ValueForAsync(dummyInitialDirectory);

        // Assert
        dummyFolderList.Should().Contain(result);
    }
}