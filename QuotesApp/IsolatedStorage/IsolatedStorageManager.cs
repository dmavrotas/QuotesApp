using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace QuotesApp.IsolatedStorage
{
    public static class IsolatedStorageManager
    {
        #region Save

        public async static void SaveToIsolatedStorage(string userData)
        {
            StorageFolder folder = null;

            try
            {
                var folders = await ApplicationData.Current.LocalFolder.GetFoldersAsync();
                foreach(var f in folders)
                {
                    if (f.Name == "LocalUserData") folder = f;
                }
            }
            catch(Exception ex)
            {
                //This exception must be silent, as the first time is going to be caught.
            }

            if (folder != null)
            {
                SaveToFile(userData);
            }
            else
            {
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("LocalUserData", CreationCollisionOption.OpenIfExists);
                SaveToFile(userData);
            }
        }

        private async static void SaveToFile(string userData)
        {
            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("LocalUserData");
            var file = await folder.CreateFileAsync("UserData.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, userData);
        }

        #endregion

        #region Load

        public async static Task<string> LoadFromIsolatedStorage()
        {
            StorageFolder folder = null;

            try
            {
                var folders = await ApplicationData.Current.LocalFolder.GetFoldersAsync();
                foreach (var f in folders)
                {
                    if (f.Name == "LocalUserData") folder = f;
                }
            }
            catch (Exception ex)
            {
                //This exception must be silent, as the first time is going to be caught.
            }

            if (folder != null)
            {
               return await GetFromFile();
            }

            return null;
        }

        private async static Task<string> GetFromFile()
        {
            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("LocalUserData");
            var file = await folder.GetFileAsync("UserData.txt");

            if(file != null)
            {
                return await FileIO.ReadTextAsync(file);
            }

            return string.Empty;
        }

        #endregion
    }
}
