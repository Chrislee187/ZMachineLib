using System;
using ZMachineLib.Extensions;
using ZMachineLib.Managers;

namespace ZMachineLib.Content
{
    public interface IZMemory
    {
        IZStack Stack { get; set; }
        ZHeader Header { get; }
        ZDictionary Dictionary { get; }
        ZAbbreviations Abbreviations { get; }
        IZObjectTree ObjectTree { get; }
        IMemoryManager Manager { get; }
        IVariableManager VariableManager { get; }
        OperandManager OperandManager { get; }
        VersionedOffsets Offsets { get; }
        ushort DictionaryWordStart { get; }
        byte PeekNextByte();
        byte PeekPreviousByte();
        byte PeekCurrentByte();
        byte GetCurrentByteAndInc();

        bool TerminateOnInput { get; set; }

        bool Running { get; set; }
        ZGlobals Globals { get; set; }

        void Restart();
        void Jump(bool flag);
        string GetZscii(ushort address);
        string GetZscii(byte[] data);
    }

    public class ZMemory : IZMemory
    {
        private readonly Action _restart;
        public IZStack Stack { get; set; }
        public ZHeader Header { get; }
        public ZDictionary Dictionary { get; }
        public ZAbbreviations Abbreviations { get; }
        public IZObjectTree ObjectTree { get; }
        public IMemoryManager Manager { get; }
        public IVariableManager VariableManager { get; }
        public OperandManager OperandManager { get; }
        public ZGlobals Globals { get; set; }


        public VersionedOffsets Offsets { get; private set; }
        public ushort DictionaryWordStart => (ushort) (Header.Dictionary + Dictionary.WordStart);

        public ZMemory(byte[] data,
            Action restart)
        {
            _restart = restart;
            Header = new ZHeader(data.AsSpan(0, 31));
            if (Header.Version > 3) throw new NotSupportedException("ZMachine > V3 not currently supported");

            // Version specific offsets - not used every yet
            Offsets = VersionedOffsets.For(Header.Version);

            Manager = new MemoryManager(data);

            // ZMachine tables
            Abbreviations = new ZAbbreviations(Header, Manager);
            Dictionary = new ZDictionary(Header, Manager, Abbreviations);
            ObjectTree = new ZObjectTree(Header, Manager, Abbreviations);
            Globals = new ZGlobals(Header, Manager);

            // Custom Stack with some abstractions for better testing.
            Stack = new ZStack();

            // Simple managers to abstract variable and argument usage
            VariableManager = new VariableManager(Stack, Globals);
            OperandManager = new OperandManager(Manager, Stack, VariableManager);
        }

        public byte PeekCurrentByte() => Manager.Get(Stack.GetPC());
        public byte PeekNextByte() => Manager.Get(Stack.GetPC() + 1);
        public byte PeekPreviousByte() => Manager.Get(Stack.GetPC() - 1);

        /// <summary>
        /// Get the byte pointed to by the current Program Counter and increment the counter by 1
        /// </summary>
        /// <returns></returns>
        public byte GetCurrentByteAndInc() => Manager.Get(Stack.GetPCAndInc());

        public static uint GetPackedAddress(ushort address, byte version = 3)
        {
            return version <= 3 
                ? (uint) (address * 2) 
                : version <= 5 
                    ? (uint) (address * 4) 
                    : 0;
        }

        public bool TerminateOnInput { get; set; }
        public bool Running { get; set; }

        public void Restart()
        {
            _restart();
        }

        public void Jump(bool flag)
        {
            // TODO: Work out whats going on here and refactor
            var offset = GetCurrentByteAndInc();
            short newOffset;

            bool branch = (offset & 0x80) == 0x80;
            Log.Write($" [{branch.ToString().ToUpper()}] ");

            var executeBranch = branch && flag || !(branch || flag);

            if ((offset & 0x40) == 0x40)
            {
                offset = (byte)(offset & 0x3f);

                if (offset == 0 && executeBranch)
                {
                    Log.Write(" RFALSE ");
                    OperationReturnBoolean(false);
                    return;
                }

                if (offset == 1 && executeBranch)
                {
                    Log.Write(" RTRUE ");
                    OperationReturnBoolean(true);
                    return;
                }

                newOffset = (short)(offset - 2);
            }
            else
            {
                var offset2 = GetCurrentByteAndInc();
                var final = (ushort)((offset & 0x3f) << 8 | offset2);

                // this is a 14-bit number, so set the sign bit properly because we can jump backwards
                if ((final & 0x2000) == 0x2000)
                    final |= 0xc000;

                newOffset = (short)(final - 2);
            }

            if (executeBranch)
                Stack.IncrementPC((uint)newOffset);

            Log.Write($"-> { Stack.GetPC():X5}");
        }

        public string GetZscii(ushort address)
        {
            return ZsciiString.Get(Manager.AsSpan(address), Abbreviations);
        }

        public string GetZscii(byte[] data)
        {
            return ZsciiString.Get(data, Abbreviations);
        }
        private void OperationReturnBoolean(bool val)
        {
            // NOTE: This does the same as the RTrue/RFalse operations
            if (Stack.Pop().StoreResult)
            {
                VariableManager.Store(GetCurrentByteAndInc(), val.ToOneOrZero());
            }
        }
    }
}