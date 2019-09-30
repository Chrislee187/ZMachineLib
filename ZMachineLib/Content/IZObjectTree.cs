using System.Collections.Generic;

namespace ZMachineLib.Content
{
    public interface IZObjectTree : IReadOnlyDictionary<ushort, IZMachineObject>
    {
        IZMachineObject GetOrDefault(ushort key);
    }
}