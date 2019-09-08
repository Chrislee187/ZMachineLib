using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ZMachineLib.Operations.Kind0
{
    public class Save : GameStateBase
    {
        public Save(ZMachine2 machine, 
            IZMachineIO io,
            RTrue rTrue, RFalse rFalse) 
            : base(Kind0OpCodes.Save, machine, io, rTrue, rFalse)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var state = CreateState();
            var val = Io.Save(state);

            if (Machine.Version < 5)
            {
                Jump(val);
            }
            else
            {
                StoreWordInVariable(
                    Machine.Memory[Machine.Stack.Peek().PC++], 
                    (ushort)(val ? 1 : 0)
                    );
            }
        }


        public Stream CreateState()
        {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(Machine.ReadParseAddr);
            bw.Write(Machine.ReadTextAddr);
            bw.Write(Machine.Memory, 0, Machine.DynamicMemorySize - 1);
            var dcs = new DataContractJsonSerializer(typeof(Stack<ZStackFrame>));
            dcs.WriteObject(ms, Machine.Stack);
            ms.Position = 0;
            return ms;
        }
    }
}