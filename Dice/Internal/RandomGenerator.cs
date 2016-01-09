using System;
using System.Security.Cryptography;

namespace Dice.Internal
{
    public static class RandomGenerator
    {
        private static readonly RNGCryptoServiceProvider Global = new RNGCryptoServiceProvider();
        [ThreadStatic] private static Random _local;

        public static int Next()
        {
            var inst = _local;
            if(inst == null)
            {
                var buffer = new byte[4];
                Global.GetBytes(buffer);
                _local = inst = new Random(BitConverter.ToInt32(buffer, 0));
            }
            return inst.Next();
        }
    }
}