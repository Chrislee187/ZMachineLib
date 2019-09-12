using System.IO;

namespace ZMachineLib
{
    public interface IFileIo
    {
        bool Save(Stream s);
        Stream Restore();
    }
}