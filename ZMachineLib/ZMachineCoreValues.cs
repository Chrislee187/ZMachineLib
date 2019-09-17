using System.Collections.Generic;

namespace ZMachineLib
{
    public abstract class ZMachineHelper
    {
        public ZMachine2 Machine { get; }

        protected ZMachineHelper(ZMachine2 machine)
        {
            Machine = machine;
        }

        protected byte[] Memory => Machine.Memory;
        protected ushort GlobalsTable => Machine.Header.Globals;
        protected ushort ObjectTable => Machine.Header.ObjectTable;
        protected VersionedOffsets Offsets => Machine.VersionedOffsets;
        protected Stack<ZStackFrame> Stack => Machine.Stack;
        protected byte Version => Machine.Header.Version;
        protected ZsciiString ZsciiString => Machine.ZsciiString;

    }
}