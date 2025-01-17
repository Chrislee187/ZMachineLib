﻿using System.Collections.Generic;

namespace ZMachineLib.Content
{
    public interface IZMachineObject
    {
        IZAttributes Attributes { get; }
        string Name { get; }
        ushort Address { get; }
        ushort Sibling { get; set; }
        ushort Parent { get; set; }
        ushort Child { get; set; }
        ushort PropertiesAddress { get; set; }
        IDictionary<int, ZProperty> Properties { get;  }
        ushort ObjectNumber { get; set; }
        byte BytesRead { get; }
        ZMachineObject RefreshFromMemory();

        ZProperty GetPropertyOrDefault(int i);
    }
}