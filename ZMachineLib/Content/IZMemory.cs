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
        IOperandManager OperandManager { get; }
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
}