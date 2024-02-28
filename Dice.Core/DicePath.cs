using System.Runtime.InteropServices;
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
    public async Task<string> ValueFor(string initialDirectory)
    {
        if (initialDirectory == null)
        {
            throw new ArgumentNullException(nameof(initialDirectory));
        }

        return await Task.Run(() =>
                              {
                                  var folderList = _filePath.GetSubdirectoriesContainingOnlyFiles(initialDirectory)?.ToList();
                                  if (folderList == null || !folderList.Any())
                                  {
                                      return "directory is empty";
                                  }

                                  var span = CollectionsMarshal.AsSpan(folderList);
                                  Random.Shared.Shuffle(span);

                                  return span[0];
                              });
    }
}
