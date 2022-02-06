using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Dice.Internal;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.Testing;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dice.Tests.Internal;

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
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DicePath).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_ForProvidedInitialDirectory_ReturnsRandomPath(
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
        var task = Task<string>.Factory.StartNew(sut.ValueFor(dummyInitialDirectory));
        task.ConfigureAwait(false);
        var result = task.Result;

        // Assert
        dummyFolderList.Contains(result).Should().BeTrue();
    }
}