namespace ZMachineLib.Operations
{
    public class VersionOffsets
    {
        public static VersionOffsets For(byte version)
        {
            VersionOffsets of = new V3VersionOffsets();
            if (version > 4)
            {
                of = new V5VersionOffsets();
            }

            return of;
        }
        public int Parent { get; protected set; }
        public int Sibling { get; protected set; }
        public int Child { get; protected set; }
        public int Property { get; protected set; }
        public int ObjectSize { get; protected set; }
        public int PropertyDefaultTableSize { get; protected set; }
    }

    public class V3VersionOffsets : VersionOffsets
    {
        public V3VersionOffsets()
        {
            Parent = 4;
            Sibling = 5;
            Child = 6;
            Property = 7;
            ObjectSize = 9;
            PropertyDefaultTableSize = 62;
        }
    }

    public class V5VersionOffsets : VersionOffsets
    {
        public V5VersionOffsets()
        {
            Parent = 6;
            Sibling = 8;
            Child = 10;
            Property = 12;
            ObjectSize = 14;
            PropertyDefaultTableSize = 126;
        }
    }

}