﻿using System.Linq;
using AutoFixture.Idioms;
using Dice.Core.Settings;
using EvilBaschdi.Testing;
using FluentAssertions;
using Xunit;

namespace Dice.Core.Tests.Settings
{
    public class InitialDirectoryFromSettingsTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(InitialDirectoryFromSettings).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(InitialDirectoryFromSettings sut)
        {
            sut.Should().BeAssignableTo<IInitialDirectoryFromSettings>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(InitialDirectoryFromSettings).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}