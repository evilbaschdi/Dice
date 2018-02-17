using System;
using System.Linq;
using System.Security.Cryptography;
using EvilBaschdi.Core.Internal;

namespace Dice.Internal
{
    /// <inheritdoc />
    public class DicePath : IDicePath
    {
        private readonly IFileListFromPath _filePath;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="filePath"></param>
        public DicePath(IFileListFromPath filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        /// <inheritdoc />
        public Func<string> ValueFor(string initialDirectory)
        {
            if (initialDirectory == null)
            {
                throw new ArgumentNullException(nameof(initialDirectory));
            }

            string GetPath()
            {
                var folderList = _filePath.GetSubdirectoriesContainingOnlyFiles(initialDirectory).ToList();

                var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
                var randomGenerator = new RandomGenerator(rngCryptoServiceProvider);

                var index = randomGenerator.ValueFor(0, folderList.Count);

                return folderList[index];
            }

            return GetPath;
        }
    }
}