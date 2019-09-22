using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using ZMachineLib.Content;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP0
{
    public class Save : GameSaveBase
    {
        public Save(ZMachine2 machine, 
            IFileIo io) 
            : base(OpCodes.Save, machine, io)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var state = CreateState();
            try
            {
                var val = Io.Save(state);
                if (Machine.Contents.Header.Version < 5)
                {
                    Jump(val);
                }
                else
                {
                    ushort value = (ushort)(val ? 1 : 0);
                    Contents.VariableManager.StoreWord(GetNextByte(), value);
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
            bw.Write(Machine.ReadParseAddr);
            bw.Write(Machine.ReadTextAddr);
            bw.Write(Machine.Memory, 0, Machine.Contents.Header.DynamicMemorySize - 1);
            var dcs = new DataContractJsonSerializer(typeof(Stack<ZStackFrame>));
            dcs.WriteObject(ms, Machine.Stack);
            ms.Position = 0;
            return ms;
        }
    }
}