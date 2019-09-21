using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ZMachineLib.Extensions;

namespace ZMachineLib
{
    public class ZMachineContents
    {
        public byte Version { get; }
        public ZHeader Header { get; }
        public ZDictionary Dictionary { get; }
        public ZAbbreviations Abbreviations { get; }
        public ZObjectTree ObjectTree { get; }

        public byte[] DynamicMemory { get; }

        public ZMachineContents(byte[] data)
        {
            Version = data[0];

            if (Version > 3) throw new NotSupportedException("ZMachine > V3 not currently supported");

            Header = new ZHeader(data.AsSpan(0,31));
            DynamicMemory = data.AsSpan(0, Header.DynamicMemorySize).ToArray();
            Abbreviations = new ZAbbreviations(data.AsSpan(Header.AbbreviationsTable), DynamicMemory);
            Dictionary = new ZDictionary(data.AsSpan(Header.Dictionary), Abbreviations);

            ObjectTree = new ZObjectTree(DynamicMemory, Header, Abbreviations);
        }
    }

    public class ZObjectTree : IReadOnlyDictionary<ushort, ZMachineObject>
    {
        private readonly Dictionary<ushort, ZMachineObject> _dict;
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private IReadOnlyList<ushort> DefaultPropAddresses { get; set; }

        public ZObjectTree(Span<byte> dynamicMemory, ZHeader zHeader, ZAbbreviations abbreviations)
        {
            var objectTableData = dynamicMemory.Slice(zHeader.ObjectTable);
            var (defaultProps, ptr) = GetDefaultProps(objectTableData);
            DefaultPropAddresses = defaultProps;

            var lastObject = false;

            ushort objNumber = 1;
            _dict = new Dictionary<ushort, ZMachineObject>();
            while (!lastObject && objNumber < 255)
            {
                // Objects end when the properties start!
                ushort objectAddress = (ushort) (zHeader.ObjectTable+ptr);

                var min = _dict.Values.Any() ? _dict.Values.Min(v => v.PropertiesAddress) : ushort.MaxValue;
                lastObject = objectAddress >= min;
                if (!lastObject)
                {
                    var zObj = new ZMachineObject(objNumber, objectAddress, dynamicMemory, abbreviations);
                    _dict.Add(objNumber, zObj);
                    ptr += zObj.BytesRead;

                    objNumber++;
                }
            }

        }
        #region IReadOnlyDictionary<>

        private (IReadOnlyList<ushort> defaultProps, ushort bytesRead) GetDefaultProps(Span<byte> data)
        {
            const int propCountV3 = 31;
            var defaultProps = new List<ushort>();


            for (int i = 0; i < propCountV3; i++)
            {
                defaultProps.Add(data.Slice(i * 2, 2).GetUShort());
            }

            return (defaultProps, propCountV3 * 2);
        }

        public IEnumerator<KeyValuePair<ushort, ZMachineObject>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dict).GetEnumerator();
        }

        public int Count => _dict.Count;

        public bool ContainsKey(ushort key)
        {
            return _dict.ContainsKey(key);
        }

        public bool TryGetValue(ushort key, out ZMachineObject value)
        {
            return _dict.TryGetValue(key, out value);
        }

        public ZMachineObject this[ushort key] => _dict[key];

        public IEnumerable<ushort> Keys => _dict.Keys;

        public IEnumerable<ZMachineObject> Values => _dict.Values; 
        #endregion
    }
}