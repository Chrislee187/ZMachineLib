using System.Collections.Generic;

namespace ZMachineLib
{
    public abstract class ZMachineHelper
    {
        private readonly ZMachine2 _machine;
        protected ZMachineHelper(ZMachine2 machine)
        {
            _machine = machine;
        }

        protected byte[] Memory => _machine.Memory;
        protected ushort GlobalsTable => _machine.Header.Globals;
        protected ushort ObjectTable => _machine.Header.ObjectTable;
        protected VersionedOffsets Offsets => _machine.VersionedOffsets;
        protected Stack<ZStackFrame> Stack => _machine.Stack;
        protected byte Version => _machine.Header.Version;
        protected ZsciiString ZsciiString => _machine.ZsciiString;

    }
}