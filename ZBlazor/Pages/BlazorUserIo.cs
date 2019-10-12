using System;
using ZMachineLib;
using ZMachineLib.Content;

namespace ZBlazor.Pages
{
    public class BlazorUserIo : IUserIo
    {
        public const string Prompt = "\n>";
        private readonly IZMachineModel _model;

        public BlazorUserIo(IZMachineModel model)
        {
            _model = model;
        }

        public void Print(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            _model.Output += text;

            if (_model.Output.EndsWith(Prompt))
            {
//                _model.Output = _model.Output[..^Prompt.Length]; // Blazor wasm is still in preview and doesn't target .NET Standard 2.1 yet
                _model.Output = _model.Output.Substring(0,_model.Output.Length - Prompt.Length);
            }

            Console.WriteLine(text);
        }

        public string Read(int max, IZMemory memory)
        {
            throw new System.NotImplementedException();
        }

        public char ReadChar()
        {
            throw new System.NotImplementedException();
        }

        public void SetCursor(ushort line, ushort column, ushort window)
        {
            throw new System.NotImplementedException();
        }

        public void SetWindow(ushort window)
        {
            throw new System.NotImplementedException();
        }

        public void EraseWindow(ushort window)
        {
            throw new System.NotImplementedException();
        }

        public void BufferMode(bool buffer)
        {
            throw new System.NotImplementedException();
        }

        public void SplitWindow(ushort lines)
        {
            throw new System.NotImplementedException();
        }

        public void ShowStatus(IZMemory memory)
        {
            var currentRoomObjNumber = (byte)memory.Globals.Get(0);
            var currentRoom = memory.ObjectTree.GetOrDefault(currentRoomObjNumber);
            var score = memory.Globals.Get(1);
            var turn = memory.Globals.Get(2);

            _model.CurrentRoom = currentRoom.Name;
            _model.Score = score.ToString();
            _model.Turns = turn.ToString();
        }

        public void SetTextStyle(TextStyle textStyle)
        {
            throw new System.NotImplementedException();
        }

        public void SetColor(ZColor foreground, ZColor background)
        {
            throw new System.NotImplementedException();
        }

        public void SoundEffect(ushort number)
        {
            throw new System.NotImplementedException();
        }

        public void Quit()
        {
            throw new System.NotImplementedException();
        }

        public byte ScreenHeight { get; }
        public byte ScreenWidth { get; }
    }
}