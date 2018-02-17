using System;
using EvilBaschdi.Core;

namespace Dice.Internal
{
    /// <inheritdoc />
    public interface IDicePath : IValueFor<string, Func<string>>
    {
    }
}