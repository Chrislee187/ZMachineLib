namespace ZMachineLib.Content
{
    public class VersionedOffsets
    {
        public static VersionedOffsets For(byte version)
        {
            VersionedOffsets of = new V3VersionedOffsets();
            if (version > 4)
            {
                of = new V5VersionedOffsets();
            }

            return of;
        }

        public ushort Parent { get; protected set; }
        public ushort Sibling { get; protected set; }
        public ushort Child { get; protected set; }
        public int Property { get; protected set; }
        public int ObjectSize { get; protected set; }
        public int PropertyDefaultTableSize { get; protected set; }
    }

    public class V3VersionedOffsets : VersionedOffsets
    {
        public V3VersionedOffsets()
        {
            Parent = 4;
            Sibling = 5;
            Child = 6;
            Property = 7;
            ObjectSize = 9;
            PropertyDefaultTableSize = 62;
        }
    }

    public class V5VersionedOffsets : VersionedOffsets
    {
        public V5VersionedOffsets()
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