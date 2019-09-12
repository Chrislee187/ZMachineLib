using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ZMachineLib.Operations.Kind0
{
    public class Restore : GameStateBase
    {
        public Restore(ZMachine2 machine,
            IZMachineIo io)
            : base(Kind0OpCodes.Restore, machine, io, 
                null, null)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var stream = Io.Restore();
            if (stream != null)
            {
                RestoreState(stream);
            }


            if (Version < 5)
            {
                Jump(true);
            }
            else
            {
                StoreWordInVariable(
                    Memory[Stack.Peek().PC++], 
                    1);
            }
        }

        private void RestoreState(Stream stream)
        {
            var br = new BinaryReader(stream);
            stream.Position = 0;
            ReadParseAddr = br.ReadUInt16();
            ReadTextAddr = br.ReadUInt16();
            stream.Read(Memory, 0, DynamicMemorySize - 1);
            var dcs = new DataContractJsonSerializer(typeof(Stack<ZStackFrame>));
            SetStack((Stack<ZStackFrame>)dcs.ReadObject(stream));
            stream.Dispose();
        }
    }
}