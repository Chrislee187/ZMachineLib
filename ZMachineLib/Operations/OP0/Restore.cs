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


            if (Version < 5)
            {
                Jump(true);
            }
            else
            {
                VariableManager.StoreWord(Memory[Stack.Peek().PC++], 1);
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