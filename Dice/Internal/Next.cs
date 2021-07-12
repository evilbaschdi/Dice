using System;
using System.Security.Cryptography;

namespace Dice.Internal
{
    /// <inheritdoc />
    public class Next : INext
    {
        [ThreadStatic] private static Random _local;
        private readonly RNGCryptoServiceProvider _rngCryptoServiceProvider;

        /// <summary>
        ///     Constructor of the class
        /// </summary>
        /// <param name="rngCryptoServiceProvider"></param>
        public Next(RNGCryptoServiceProvider rngCryptoServiceProvider)
        {
            _rngCryptoServiceProvider = rngCryptoServiceProvider ?? throw new ArgumentNullException(nameof(rngCryptoServiceProvider));
        }

        /// <inheritdoc />
        public int Value
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
                _local = inst = new(BitConverter.ToInt32(buffer, 0));
                return inst.Next();
            }
        }
    }
}