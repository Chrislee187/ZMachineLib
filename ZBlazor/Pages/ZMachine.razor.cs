using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.JSInterop;
using ZMachineLib;

namespace ZBlazor.Pages
{
    public class ZMachineModel : ComponentBase, IZMachineModel
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IJSRuntime JsRuntime { get; set; }

        // ReSharper disable UnassignedField.Global
        // ReSharper disable MemberCanBePrivate.Global
        protected ElementReference MachineInputElement;
        protected ElementReference MachineOutputElement;
        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore UnassignedField.Global

        private readonly ZMachine2 _zMachine2;
        public string Output { get; set; } = "Loading...";//= new string('#', 10000)  + Environment.NewLine;

        [Parameter] public string ProgramName { get; set; }
        [Parameter] public string ProgramFile { get; set; }

        public string CurrentRoom { get; set; }
        public string Score { get; set; }

        public string Turns { get; set; }

        private string _value;
        private bool _lastKeyPressedWasEnter;

        [Parameter]
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                if (_lastKeyPressedWasEnter && !string.IsNullOrEmpty(Value))
                {
                    _lastKeyPressedWasEnter = false;

                    RunCommand();
                    StateHasChanged();
                }
            }
        }

        public ZMachineModel()
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

            try
            {
                var programStream = await HttpClient.GetStreamAsync(ProgramFile);
                Output = "";
                _zMachine2.RunFile(programStream, false);
            }
            catch (Exception e)
            {
                Output = e.Message;
            }
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            ReadyForNextCommand();
        }

        private void RunCommand()
        {
            if (!string.IsNullOrEmpty(Value))
            {
                var cmd = Value;
                Output += $"{BlazorUserIo.Prompt} {cmd}\n";
                Value = string.Empty;
                
                StateHasChanged();

                _zMachine2.ContinueTillNextRead(cmd);
                StateHasChanged();
            }
        }

        private void ReadyForNextCommand()
        {
            MachineOutputElement.ScrollToBottom(JsRuntime);
            MachineInputElement.Focus(JsRuntime);
        }


        public void OnKeypress(KeyboardEventArgs e)
        {
            _lastKeyPressedWasEnter = e.Key.ToLower() == "enter";
            RunCommand();
        }
        public void OnKeydown(KeyboardEventArgs e)
        {
            if (e.Code == "ArrowUp")
            {
                Value = "QUIT";
                MachineInputElement.SetSelectionRange(999, 0, JsRuntime);
            }
        }
    }
}
