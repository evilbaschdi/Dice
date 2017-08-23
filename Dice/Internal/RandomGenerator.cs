using System;
using System.Security.Cryptography;

namespace Dice.Internal
{
    /// <inheritdoc />
    public class RandomGenerator : IRandomGenerator
    {
        [ThreadStatic] private static Random _local;
        private readonly RNGCryptoServiceProvider _rngCryptoServiceProvider;

        /// <summary>
        ///     Constructor of the class
        /// </summary>
        /// <param name="rngCryptoServiceProvider"></param>
        public RandomGenerator(RNGCryptoServiceProvider rngCryptoServiceProvider)
        {
            _rngCryptoServiceProvider = rngCryptoServiceProvider ?? throw new ArgumentNullException(nameof(rngCryptoServiceProvider));
        }

        private int Next
        {
            get
            {
                var inst = _local;
                if (inst != null)
                {
                    return inst.Next();
                }
                var buffer = new byte[4];
                _rngCryptoServiceProvider.GetBytes(buffer);
                _local = inst = new Random(BitConverter.ToInt32(buffer, 0));
                return inst.Next();
            }
        }


        /// <inheritdoc />
        public int ValueFor(int min, int max)
        {
            var result = Next;
            return result % max + min;
        }
    }
}