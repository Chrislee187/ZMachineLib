namespace ZMachineLib.Content
{
    public class V2CharProvider
    {
        private readonly string[] _charTables =
        {
            @"abcdefghijklmnopqrstuvwxyz",
            @"ABCDEFGHIJKLMNOPQRSTUVWXYZ",
            $" \n" + @"0123456789.,!?_#'""/\-:()"
        };
        private int _currentV2CharTable = 0;
        private ZAbbreviations _abbreviations;

        public V2CharProvider(ZAbbreviations abbreviations)
        {
            _abbreviations = abbreviations;
        }

        public void ResetTable()
        {
            _currentV2CharTable = 0;
        }
        public (string, int inc) GetCharTable(byte zChar)
        {
            if (zChar == 2)
            {
                return (_charTables[WrapTableIndex(_currentV2CharTable + 1)], 1);
            } 
                
            if (zChar == 3)
            {
                return (_charTables[WrapTableIndex(_currentV2CharTable - 1)], 1);
            }

            int inc = 0;                
            if (zChar == 4)
            {
                inc = 1;
                _currentV2CharTable = WrapTableIndex(_currentV2CharTable+1);
            }
            else if (zChar == 5)
            {
                inc = 1;
                _currentV2CharTable = WrapTableIndex(_currentV2CharTable - 1);
            }

            return (_charTables[_currentV2CharTable], inc);
        }

        private int WrapTableIndex(int val)
        {
            if (val == 3) return 0;
            if (val == -1) return 2;

            return val;
        }
    }
}