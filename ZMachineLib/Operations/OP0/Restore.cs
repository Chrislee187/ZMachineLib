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
            
            if (Memory.Header.Version < 5)
            {
                Memory.Jump(true);
            }
            else
            {
                Memory.VariableManager.Store(Memory.GetCurrentByteAndInc(), 1);
            }
        }

        private void RestoreState(Stream stream)
        {
            stream.Position = 0;
            stream.Read(((MemoryManager)(Memory.Manager)).Buffer, 0, Memory.Header.DynamicMemorySize - 1);

            var dcs = new DataContractJsonSerializer(typeof(ZStackFrame[]));
            var zStackFrames = (ZStackFrame[])dcs.ReadObject(stream);
            
            Memory.Stack.Clear();
            foreach (var zStackFrame in zStackFrames.ToArray().Reverse())
            {
                Memory.Stack.Push(zStackFrame);
            }

            stream.Dispose();
        }
    }
}