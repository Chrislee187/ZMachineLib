using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ZMachineLib.Operations.Kind0
{
    public class Restore : GameStateBase
    {
        public Restore(ZMachine2 machine,
            IZMachineIO io)
            : base(Kind0OpCodes.Restore, machine, io, 
                null, null)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var stream = Io.Restore();
            if (stream != null)
                RestoreState(stream);

            if (Machine.Version < 5)
            {
                Jump(stream != null);
            }
            else
            {
                StoreWordInVariable(
                    Machine.Memory[Machine.Stack.Peek().PC++], 
                    (ushort)(stream != null ? 1 : 0));
            }
        }

        private void RestoreState(Stream stream)
        {
            var br = new BinaryReader(stream);
            stream.Position = 0;
            Machine.ReadParseAddr = br.ReadUInt16();
            Machine.ReadTextAddr = br.ReadUInt16();
            stream.Read(Machine.Memory, 0, Machine.DynamicMemorySize - 1);
            var dcs = new DataContractJsonSerializer(typeof(Stack<ZStackFrame>));
            Machine.Stack = (Stack<ZStackFrame>)dcs.ReadObject(stream);
            stream.Dispose();
        }
    }
}