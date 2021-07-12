using System;
using JetBrains.Annotations;

namespace Dice.Internal
{
    /// <inheritdoc />
    public class RandomGenerator : IRandomGenerator
    {
        private readonly INext _next;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="next"></param>
        public RandomGenerator([NotNull] INext next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }


        /// <inheritdoc />
        public int ValueFor(int min, int max)
        {
            var result = _next.Value;
            return result % max + min;
        }
    }
}