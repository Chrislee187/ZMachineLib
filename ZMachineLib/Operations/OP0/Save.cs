using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using ZMachineLib.Content;
using ZMachineLib.Extensions;
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

        public override void Execute(List<ushort> args)
        {
            var state = CreateState();

            try
            {
                var saveSuccessful = Io.Save(state);
                state.Dispose();
                if (Contents.Header.Version < 5)
                {
                    Contents.Jump(saveSuccessful);
                }
                else
                {
                    Contents.VariableManager.Store(Contents.GetCurrentByteAndInc(), saveSuccessful.ToOneOrZero());
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

            Debug.Assert(Contents.ReadParseAddr == 0);
            Debug.Assert(Contents.ReadTextAddr == 0);


            bw.Write(Contents.ReadParseAddr);
            bw.Write(Contents.ReadTextAddr);
            bw.Write(((MemoryManager)(Contents.Manager)).Buffer, 0, Contents.Header.DynamicMemorySize - 1);

            var frames = (Contents.Stack as Stack<ZStackFrame>).Select(f => f).ToArray();
            //            ZStackFrame[] frames = new ZStackFrame[stack.Count];
            //            stack.CopyTo(frames,0);
            var dcs = new DataContractJsonSerializer(typeof(ZStackFrame[]));
            dcs.WriteObject(ms, frames);
            return ms;
        }
    }
}