using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ZMachineLib.Operations.OP0
{
    public class Restore : GameSaveBase
    {
        public Restore(ZMachine2 machine,
            IFileIo io)
            : base(OpCodes.Restore, machine, io)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var stream = Io.Restore();
            if (stream != null)
            {
                RestoreState(stream);
            }


            if (Machine.Header.Version < 5)
            {
                Jump(true);
            }
            else
            {
                VariableManager.StoreWord(Machine.Memory[Machine.Stack.Peek().PC++], 1);
            }
        }

        private void RestoreState(Stream stream)
        {
            var br = new BinaryReader(stream);
            stream.Position = 0;
            Machine.ReadParseAddr = br.ReadUInt16();
            Machine.ReadTextAddr = br.ReadUInt16();
            stream.Read(Machine.Memory, 0, Machine.Header.DynamicMemorySize - 1);
            var dcs = new DataContractJsonSerializer(typeof(Stack<ZStackFrame>));
            Machine.Stack = (Stack<ZStackFrame>)dcs.ReadObject(stream);
            stream.Dispose();
        }
    }
}