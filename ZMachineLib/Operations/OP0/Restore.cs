using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using ZMachineLib.Content;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP0
{
    public class Restore : GameSaveBase
    {
        public Restore(IZMemory memory,
            IFileIo io)
            : base(OpCodes.Restore, memory, io)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var stream = Io.Restore();
            if (stream != null)
            {
                RestoreState(stream);
            }


            if (Contents.Header.Version < 5)
            {
                Contents.Jump(true);
            }
            else
            {
                Contents.VariableManager.Store(Contents.GetCurrentByteAndInc(), 1);
            }
        }

        private void RestoreState(Stream stream)
        {
            stream.Position = 0;
            stream.Read(((MemoryManager)(Contents.Manager)).Buffer, 0, Contents.Header.DynamicMemorySize - 1);

            var dcs = new DataContractJsonSerializer(typeof(ZStackFrame[]));
            var zStackFrames = (ZStackFrame[])dcs.ReadObject(stream);
            
            Contents.Stack.Clear();
            foreach (var zStackFrame in zStackFrames.ToArray().Reverse())
            {
                Contents.Stack.Push(zStackFrame);
            }

            stream.Dispose();
        }
    }
}