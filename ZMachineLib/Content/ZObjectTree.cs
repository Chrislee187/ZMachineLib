using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ZMachineLib.Managers;

namespace ZMachineLib.Content
{
    public class ZObjectTree : IZObjectTree
    {
        private readonly Dictionary<ushort, IZMachineObject> _dict;

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private Dictionary<int, byte[]> DefaultProperties { get; set; }

        public ZObjectTree(ZHeader header,
            IMemoryManager manager,
            ZAbbreviations abbreviations)
        {
            var objectTableData = manager.AsSpan(header.ObjectTable);// memory.AsSpan(header.ObjectTable);

            var (defaultProps, ptr) = GetDefaultProps(objectTableData);
            DefaultProperties = defaultProps;

            var lastObject = false;

            ushort objNumber = 1;
            _dict = new Dictionary<ushort, IZMachineObject>();

            while (!lastObject && objNumber < 255)
            {
                // Objects end when the properties start!
                ushort objectAddress = (ushort) (header.ObjectTable+ptr);

                var min = _dict.Values.Any() ? _dict.Values.Min(v => v.PropertiesAddress) : ushort.MaxValue;
                lastObject = objectAddress >= min;
                if (!lastObject)
                {
                    var zObj = new ZMachineObject(objNumber, objectAddress, header, manager, abbreviations, DefaultProperties);
                    _dict.Add(objNumber, zObj);
                    ptr += zObj.BytesRead;

                    objNumber++;
                }
            }

        }

        public IZMachineObject GetOrDefault(ushort key)
        {
            if (_dict.ContainsKey(key))
            {
                return _dict[key];
            }
            else
            {
                return ZMachineObject.Object0;
            }
        }

        private (Dictionary<int, byte[]> defaultProps, ushort bytesRead) GetDefaultProps(Span<byte> data)
        {
            // Section 12.2 - Default Property Table
            // NOTE: Default properties values are fixed at 2 bytes (words)
            const int propCountV3 = 31;
            var defaultProps = new Dictionary<int, byte[]>();

            for (int i = 0; i < propCountV3; i++)
            {
                defaultProps.Add(i + 1, data.Slice(i * 2, 2).ToArray());
            }

            return (defaultProps, propCountV3 * 2);
        }
        
        #region IReadOnlyDictionary<>

        public IEnumerator<KeyValuePair<ushort, IZMachineObject>> GetEnumerator() 
            => _dict.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => ((IEnumerable)_dict).GetEnumerator();

        public int Count => _dict.Count;

        public bool ContainsKey(ushort key) 
            => _dict.ContainsKey(key);

        public IZMachineObject this[ushort key] => GetOrDefault(key);

        public IEnumerable<ushort> Keys => _dict.Keys;

        public IEnumerable<IZMachineObject> Values => _dict.Values;

        public bool TryGetValue(ushort key, out IZMachineObject value)
        {
            var found = _dict.TryGetValue(key, out value);
            if (found) value.RefreshFromMemory();
            return found;
        }
        #endregion
    }
}