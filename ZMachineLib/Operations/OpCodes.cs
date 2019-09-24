namespace ZMachineLib.Operations
{
    public enum OpCodes : byte
    {
        // 2OP - V3
        Je = 0x01,
        Jl = 0x02,
        Jg = 0x03,
        DecCheck = 0x04,
        IncCheck = 0x05,
        Jin = 0x06,
        Test = 0x07,
        Or = 0x08,
        And = 0x09,
        TestAttribute = 0x0a,
        SetAttribute = 0x0b,
        ClearAttribute = 0x0c,
        Store = 0x0d,
        InsertObj = 0x0e,
        LoadW = 0x0f,
        LoadB = 0x10,
        GetProp = 0x11,
        GetPropAddr = 0x12,
        GetNextProp = 0x13,
        Add = 0x14,
        Sub = 0x15,
        Mul = 0x16,
        Div = 0x17,
        Mod = 0x18,
        // 2OP - V4
        Call2S = 0x19,
        // 2OP -V5
        Call2N = 0x1a,
        SetColor = 0x1b, // NOTE: args differences between in v5/6
        // ReSharper disable once UnusedMember.Global
        // 2OP - V6
        @Throw = 0x1c,

        // 1OP - V3
        Jz = 0x80,
        GetSibling = 0x81,
        GetChild = 0x82,
        GetParent = 0x83,
        GetPropLen = 0x84,
        Inc = 0x85,
        Dec = 0x86,
        PrintAddr = 0x87,
        Call1S = 0x88, // V4
        RemoveObj = 0x89,
        PrintObj = 0x8a,
        Ret = 0x8b,
        Jump = 0x8c,
        PrintPAddr = 0x8d,
        Load = 0x8e,
        // V4
        Not = 0x8f,
        // V5
        Call1N = 0x8f,

        // 0OP
        RTrue = 0xb0,
        RFalse = 0xb1,
        Print = 0xb2,
        PrintRet = 0xb3,
        Nop = 0xb4,
        Save = 0xb5,
        Restore = 0xb6,
        Restart = 0xb7,
        RetPopped = 0xb8,
        Pop = 0xb9,
        Quit = 0xba,
        NewLine = 0xbb,
        ShowStatus = 0xbc,
        Verify = 0xbd,
        Extended = 0xbe,
        Piracy = 0xbf,

        // VAR OP
        Call = 0xe0,
        StoreW = 0xe1,
        StoreB = 0xe2,
        PutProp = 0xe3,
        Read = 0xe4,
        PrintChar = 0xe5,
        PrintNum = 0xe6,
        Random = 0xe7,
        Push = 0xe8,
        Pull = 0xe9, // TODO: >=v6 then additional functionality
        SplitWindow = 0xea,
        SetWindow = 0xeb,
        CallVs2 = 0xec,
        EraseWindow = 0xed,
        // ReSharper disable once UnusedMember.Global
        EraseLine = 0xee,// TODO: >=v6 then additional functionality
        SetCursor = 0xef,// TODO: >=v6 then additional functionality
        // ReSharper disable once UnusedMember.Global
        GetCursor = 0xf0,
        SetTextStyle = 0xf1,
        BufferMode = 0xf2,
        OutputStream = 0xf3,
        // ReSharper disable once UnusedMember.Global
        InputStream = 0xf4,
        SoundEffect = 0xf5,
        ReadChar = 0xf6,
        ScanTable = 0xf7,
        NotVar = 0xf8,
        CallVn = 0xf9,
        CallVn2 = 0xfa,
        // ReSharper disable once UnusedMember.Global
        Tokenise = 0xfb,
        // ReSharper disable once UnusedMember.Global
        EncodeText = 0xfc,
        CopyTable = 0xfd,
        PrintTable = 0xfe,
        CheckArgCount = 0xff
    }

    public static class OpCodesExtensions
    {
        public static OpCodes ToOpCode(this byte opCode)
        {
            // Masks the byte value opCode to match up to the enum
            byte code = opCode;
            if (opCode < 0x80) // 2OP:0-127
            {
                code = (byte)(opCode & 0x1f);
            }
            else if (opCode < 0xb0) // 1OP:128-175
            {
                code = (byte)(opCode & 0x8f);
            }
            else if (opCode < 0xc0) // 0OP:176-191
            {
                code = (byte)(opCode & 0xbf);
            }
            else if (opCode < 0xe0) // 2OP:176-191
            {
                code = (byte)(opCode & 0x1f);
            }

            return (OpCodes)code;
        }
    }
}