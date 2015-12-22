using System.Windows.Forms;

namespace Dice.Core
{
    public class AppBasics : IAppBasics
    {
        public void BrowseFolder()
        {
            var folderDialog = new FolderBrowserDialog
            {
                SelectedPath = GetInitialDirectory()
            };

            var result = folderDialog.ShowDialog();
            if(result.ToString() != "OK")
            {
                return;
            }

            Properties.Settings.Default.InitialDirectory = folderDialog.SelectedPath;
            Properties.Settings.Default.Save();
        }

        public string GetInitialDirectory()
        {
            return string.IsNullOrWhiteSpace(Properties.Settings.Default.InitialDirectory)
                ? ""
                : Properties.Settings.Default.InitialDirectory;
        }
    }
}