using System.Security.Cryptography;
using EvilBaschdi.Core.Internal;

namespace Dice.Core;

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
    public async Task<string> ValueForAsync(string initialDirectory)
    {
        return initialDirectory == null
            ? throw new ArgumentNullException(nameof(initialDirectory))
            : await Task.Run(() =>
                             {
                                 var folderList = _filePath.GetSubdirectoriesContainingOnlyFiles(initialDirectory)?.ToList();
                                 if (folderList == null || folderList.Count == 0)
                                 {
                                     return "directory is empty";
                                 }

                                 var index = RandomNumberGenerator.GetInt32(0, folderList.Count);

                                 return folderList[index];
                             });
    }
}