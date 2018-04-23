using System;
using PCLStorage;
using System.Threading.Tasks;


namespace StockMvvm
{
    public static class StorageControl
    {
        //public StorageControl()
        //{
           
        //}

        public static async Task<string> PCLSaveCommand(string MyEntry)
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync("MySubFolder", CreationCollisionOption.OpenIfExists);
            IFile file = await folder.CreateFileAsync("answer.txt", CreationCollisionOption.ReplaceExisting);
            await file.WriteAllTextAsync(MyEntry);
            return MyEntry;
        }

        public static async Task<string> PCLLoadCommand()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync("MySubFolder", CreationCollisionOption.OpenIfExists);
            IFile file = await folder.GetFileAsync("answer.txt");
            var MyEntry = await file.ReadAllTextAsync();
            return MyEntry;
        }



    }
}

