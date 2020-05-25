using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Dice.Core;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.Testing;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Dice.Tests.Core
{
    public class AppSettingsTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AppSettings).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(AppSettings sut)
        {
            sut.Should().BeAssignableTo<IAppSettings>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void InitialDirectory_ByValueFromAppSettingsBase_ReturnsValue(
            [Frozen] IAppSettingsBase appSettingsBase,
            AppSettings sut,
            string dummyInitialDirectory)
        {
            // Arrange
            appSettingsBase.Get("InitialDirectory", "").Returns(dummyInitialDirectory);

            // Act
            var result = sut.InitialDirectory;

            // Assert
            result.Should().Be(dummyInitialDirectory);
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void InitialDirectory_ValueFromAppSettingsBaseEmpty_ReturnsFallBack(
            [Frozen] IAppSettingsBase appSettingsBase,
            AppSettings sut)
        {
            // Arrange
            appSettingsBase.Get("InitialDirectory", "").ReturnsNull();

            // Act
            var result = sut.InitialDirectory;

            // Assert
            result.Should().BeNull();
        }
    }
}