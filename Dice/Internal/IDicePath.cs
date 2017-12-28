using System;
using EvilBaschdi.Core.DotNetExtensions;

namespace Dice.Internal
{
    /// <inheritdoc />
    public interface IDicePath : IValueFor<string, Func<string>>
    {
    }
}