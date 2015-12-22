using System.Collections.Generic;

namespace Dice.Internal
{
    public interface IFilePath
    {
        List<string> GetSubdirectoriesContainingOnlyFiles(string path);

        List<string> GetFileList(string path);
    }
}