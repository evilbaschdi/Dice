using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using EvilBaschdi.Core.DirectoryExtensions;

namespace Dice.Internal
{
    /// <inheritdoc />
    public class DicePath : IDicePath
    {
        private readonly IFilePath _filePath;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="filePath"></param>
        public DicePath(IFilePath filePath)
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