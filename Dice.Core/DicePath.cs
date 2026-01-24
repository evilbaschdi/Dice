using System.Security.Cryptography;
using EvilBaschdi.Core.Internal;

namespace Dice.Core;

/// <inheritdoc />
public class DicePath : IDicePath
{
    private readonly IFileListFromPath _filePath;
    private readonly Dictionary<string, (List<string> Result, DateTime CacheTime)> _directoryCache = new();
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="filePath"></param>
    public DicePath(IFileListFromPath filePath)
    {
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }

    /// <inheritdoc />
    public async Task<string> ValueForAsync(string initialDirectory, CancellationToken cancellationToken = default)
    {
        return initialDirectory == null
            ? throw new ArgumentNullException(nameof(initialDirectory))
            : await Task.Run(() =>
                             {
                                 var folderList = GetCachedSubdirectories(initialDirectory);
                                 if (folderList == null || folderList.Count == 0)
                                 {
                                     return "directory is empty";
                                 }

                                 var index = RandomNumberGenerator.GetInt32(0, folderList.Count);

                                 return folderList[index];
                             }, cancellationToken);
    }

    private List<string> GetCachedSubdirectories(string path)
    {
        lock (_directoryCache)
        {
            if (_directoryCache.TryGetValue(path, out var cached))
            {
                if (DateTime.UtcNow - cached.CacheTime < _cacheDuration)
                {
                    return cached.Result;
                }

                // Cache expired, remove it
                _directoryCache.Remove(path);
            }

            var result = _filePath.GetSubdirectoriesContainingOnlyFiles(path)?.ToList();
            if (result != null && result.Count > 0)
            {
                _directoryCache[path] = (result, DateTime.UtcNow);
            }

            return result;
        }
    }

    /// <summary>
    /// Clears the cache entry for a specific directory
    /// </summary>
    public void InvalidateCache(string path)
    {
        lock (_directoryCache)
        {
            _directoryCache.Remove(path);
        }
    }

    /// <summary>
    /// Clears all cached directory listings
    /// </summary>
    public void ClearCache()
    {
        lock (_directoryCache)
        {
            _directoryCache.Clear();
        }
    }
}
