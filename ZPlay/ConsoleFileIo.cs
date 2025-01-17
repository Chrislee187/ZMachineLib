﻿using System;
using System.IO;
using ZMachineLib;

namespace ZPlay
{
    public class ConsoleFileIo : IFileIo
    {
        private readonly string _saveFileId;

        private string _lastSavePath;
        public ConsoleFileIo(string saveFileId)
        {
            _saveFileId = saveFileId;
        }
        public bool Save(Stream s)
        {
            var saveFilePath = GetUserSavePath();
            
            _lastSavePath = SaveGame(s, saveFilePath);

            return true;
        }

        public Stream Restore()
        {
            var saveFilePath = GetUserSavePath(false);
                
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

        private string GetUserSavePath(bool checkForOverwrite = true)
        {
            var saveFilePath = !string.IsNullOrEmpty(_lastSavePath) 
                ? _lastSavePath 
                : BuildSavePath();
            bool confirmed;
            do
            {
                var display = Path.GetFileNameWithoutExtension(saveFilePath);
                Console.Write($"Enter save filename (press RETURN To use '{display}'): ");
                var readLine = Console.ReadLine();

                if (!string.IsNullOrEmpty(readLine))
                {
                    saveFilePath = Path.Combine(
                        Path.GetDirectoryName(saveFilePath),
                        $"{readLine}.zSave"
                    );
                }
                if (checkForOverwrite && File.Exists(saveFilePath))
                {
                    Console.Write("File already exists, overwrite Y/n?: ");

                    var line = Console.ReadLine();
                    confirmed = string.IsNullOrEmpty(line) || line.ToLower().StartsWith("y");
                }
                else
                {
                    confirmed = true;
                }

            } while (!confirmed);



            return saveFilePath;
        }

        private static string SaveGame(Stream gameState, string saveFilePath)
        {
            try
            {
                gameState.Position = 0;
                var fs = File.Create(saveFilePath);
                gameState.CopyTo(fs);
                fs.Close();

                Console.WriteLine(($"Game saved to: {saveFilePath}"));
            }
            catch (Exception e)
            {
                Console.WriteLine(($"Error writing save to: {saveFilePath}"));
                Console.WriteLine(e.Message);
                return string.Empty;
            }

            return saveFilePath;
        }

        private string BuildSavePath()
        {
            var saveFilename = $"{_saveFileId}.zSave";
            var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var saveDirname = "ZFileSaves";
            var savePath = Path.Combine(docs, saveDirname);

            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

            var saveFilePath = Path.Combine(savePath, saveFilename);
            return saveFilePath;
        }
    }
}