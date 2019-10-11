using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.JSInterop;
using ZMachineLib;
using ZMachineLib.Content;
using ZMachineLib.Operations.OP0;

namespace ZBlazor.Pages
{
    public class Zork1Model : ComponentBase
    {
        [Inject] public HttpClient HttpClient { get; set; }
        [Inject] public IJSRuntime JsRuntime { get; set; }

        private ZMachine2 _zMachine2;
        public string Output { get; set; }

        public string ProgramName { get; set; }
        public string CurrentRoom { get; set; }
        public string Score { get; set; }
        public string Turns { get; set; }

        private string _value;

        [Parameter]
        public string Value
        {
            get => _value;
            set
            {
                if (_enterPressed)
                {
                    _enterPressed = false;

                    if (!string.IsNullOrEmpty(value))
                    {
                        Output += $"{value}\n";
                        StateHasChanged();
                        ExecuteCommand(value);
                        _value = string.Empty;
                    }
                }
                else
                {
                    _value = value;
                }

                StateHasChanged();
            }
        }

        private void ExecuteCommand(string cmd)
        {
            _zMachine2.ContinueTillNextRead(cmd);
            ZmOutputElement.ScrollToBottom(JsRuntime);
            ZmUserInputElement.Focus(JsRuntime);
        }

        // ReSharper disable UnassignedField.Global
        protected ElementReference ZmUserInputElement;
        protected ElementReference ZmOutputElement;
        protected ElementReference ZmOutputAnchor;
        // ReSharper restore UnassignedField.Global

        public Zork1Model()
        {
            _zMachine2 = new ZMachine2(
                new BlazorUserIo(this),
                null,
                NullLogger.Instance
            );
        }
        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();

            ProgramName = "Zork I";


            var programStream = await HttpClient.GetStreamAsync(@"https://raw.githubusercontent.com/historicalsource/zork1/master/COMPILED/zork1.z3");

            _zMachine2.RunFile(programStream, false);
            ZmUserInputElement.Focus(JsRuntime);
            StateHasChanged();
        }

        private bool _enterPressed;
        public void OnKeyUp(KeyboardEventArgs e)
        {
            _enterPressed = e.Key.ToLower() == "enter";
        }
    }

    public class BlazorUserIo : IUserIo
    {
        private Zork1Model _model;

        public BlazorUserIo(Zork1Model model)
        {
            _model = model;
        }

        public void Print(string s)
        {
            _model.Output += s;
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
