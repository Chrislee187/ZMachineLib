using System;
using ZMachineLib.Managers;

namespace ZMachineLib.Content
{
    /// <summary>
    /// Section 6.2
    /// Global variables(variable numbers $10 to $ff) are stored in a table in the Z-machine's
    /// dynamic memory, at a byte address given in word 6 of the header.
    /// The table consists of 240 2-byte words and the initial values of the global variables
    /// are the values initially contained in the table.
    /// (It is legal for a program to alter the table's contents directly in play,
    /// though not for it to change the table's address.)
    /// </summary>
    public class ZGlobals : IZGlobals
    {
        private readonly IMemoryManager _manager;
        private readonly ZHeader _header;

        public const int NumberOfGlobals = 240;
        private const int GlobalNumberOffset = 0x10;

        public ZGlobals(ZHeader header, IMemoryManager manager)
        {
            _header = header;
            _manager = manager;
        }

        public ushort Get(byte globalNumber) => 
            _manager.GetUShort(AddressOfGlobal(globalNumber));

        public void Set(byte globalNumber, ushort value) 
            => _manager.SetUShort(AddressOfGlobal(globalNumber), value);

        private ushort AddressOfGlobal(byte globalNumber) 
            => (ushort) (_header.Globals + (globalNumber *2));

        public static byte GetGlobalsNumber(byte value)
        {
            if(value < 0x10) throw new ArgumentOutOfRangeException(nameof(value), "value is not a globals number index");
            return (byte) (value - GlobalNumberOffset);
        }
    }
}