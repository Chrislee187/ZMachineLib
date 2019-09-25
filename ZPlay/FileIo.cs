using System;
using System.IO;
using ZMachineLib;

namespace ZPlay
{
    public class FileIo : IFileIo
    {
        private readonly string _saveFileId;

        public FileIo(string saveFileId)
        {
            _saveFileId = saveFileId;
        }
        public bool Save(Stream s)
        {
            var saveFilePath = BuildSavePath();
            s.Position = 0;
            var fs = File.Create(saveFilePath);
            s.CopyTo(fs);
            fs.Close();
            Console.WriteLine(($"Game saved to: {saveFilePath}"));
            return true;
        }

        public Stream Restore()
        {
            var saveFilePath = BuildSavePath();
            try
            {
                var fs = File.OpenRead(saveFilePath);
                return fs;
            }
            catch (Exception e)
            {
                Console.WriteLine(($"Error reading save from: {saveFilePath}"));
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private string BuildSavePath()
        {
            var saveFilename = $"{_saveFileId}.save";
            var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var saveDirname = "ZFileSaves";
            var savePath = Path.Combine(docs, saveDirname);

            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

            var saveFilePath = Path.Combine(savePath, saveFilename);
            return saveFilePath;
        }
    }
}