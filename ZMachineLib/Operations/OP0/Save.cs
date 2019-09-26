using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using ZMachineLib.Content;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP0
{
    public class Save : GameSaveBase
    {
        public Save(IZMemory memory,
            IFileIo io) 
            : base(OpCodes.Save, memory, io)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var state = CreateState();

            try
            {
                var val = Io.Save(state);
                state.Dispose();
                if (Contents.Header.Version < 5)
                {
                    Jump(val);
                }
                else
                {
                    ushort value = (ushort)(val ? 1 : 0);
                    Contents.VariableManager.StoreWord(Contents.GetCurrentByteAndInc(), value);
                }
            }
            catch
            {
                // ignored: we don't want to crash the machine so ignore any IO errors
            }
        }


        private Stream CreateState()
        {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            var dcs = new DataContractJsonSerializer(typeof(Stack<ZStackFrame>));

            bw.Write(Contents.ReadParseAddr);
            bw.Write(Contents.ReadTextAddr);
            bw.Write(((MemoryManager)(Contents.Manager)).Buffer, 0, Contents.Header.DynamicMemorySize - 1);
            dcs.WriteObject(ms, Contents.Stack);
            return ms;
        }
    }
}