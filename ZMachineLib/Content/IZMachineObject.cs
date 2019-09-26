﻿using System.Collections.Generic;

namespace ZMachineLib.Content
{
    public interface IZMachineObject
    {
        bool TestAttribute(ushort attr);
        void ClearAttribute(ushort attr);
        void SetAttribute(ushort attr);
        ulong Attributes { get; }
        string Name { get; }
        ushort Address { get; set; }
        ushort Sibling { get; set; }
        ushort Parent { get; set; }
        ushort Child { get; set; }
        ushort PropertyHeader { get; set; }
        Dictionary<int, bool> AttributeFlags { get; set; }
        ushort PropertiesAddress { get; set; }
        IDictionary<int, ZProperty> Properties { get;  }
        ushort ObjectNumber { get; set; }
        byte BytesRead { get; }
        ZMachineObject RefreshFromMemory();

        ZProperty GetProperty(int i);
    }
}