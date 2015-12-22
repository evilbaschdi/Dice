using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dice.Internal
{
    public class FilePath : IFilePath
    {
        public List<string> GetSubdirectoriesContainingOnlyFiles(string path)
        {
            return Directory.GetDirectories(path, "*", SearchOption.AllDirectories).ToList();
        }

        public List<string> GetFileList(string path)
        {
            var fileList = new List<string>();

            var initialDirectoryFileList = Directory.GetFiles(path).ToList();
            Parallel.ForEach(initialDirectoryFileList.Where(file => IsValidFileName(file, fileList)),
                file => fileList.Add(file));

            var initialDirectorySubdirectoriesFileList = GetSubdirectoriesContainingOnlyFiles(path).SelectMany(Directory.GetFiles);

            Parallel.ForEach(initialDirectorySubdirectoriesFileList.Where(file => IsValidFileName(file, fileList)),
                file => fileList.Add(file));

            return fileList;
        }

        private bool IsValidFileName(string file, ICollection<string> fileList)
        {
            var fileExtension = Path.GetExtension(file);
            return !fileList.Contains(file) && !string.IsNullOrWhiteSpace(fileExtension) && !fileExtension.ToLower().Equals(".ini") && !fileExtension.ToLower().Equals(".db");
        }
    }
}