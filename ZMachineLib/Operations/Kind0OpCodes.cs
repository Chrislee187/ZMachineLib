namespace ZMachineLib.Operations
{
    public enum OpKinds
    {
        Unknown,
        Kind0,
        Kind1,
        Kind2
    }
    public enum Kind0OpCodes : ushort {
        RTrue = 0x00,
        RFalse = 0x01,
        Print = 0x02,
        PrintRet = 0x03,
        Nop = 0x04,
        Save = 0x05,
        Restore = 0x06, 
        Restart = 0x07,
        RetPopped = 0x08,
        Pop = 0x09,
        Quit = 0x0a,
        NewLine = 0x0b,
        ShowStatus = 0x0c,
        Verify = 0x0d,
        Piracy = 0x0f
    }

    public enum Kind1OpCodes : ushort
    {
        Jz = 0x00,
        GetSibling = 0x01,
        GetChild = 0x02,
        GetParent = 0x03,
        GetPropLen = 0x04,
        Inc = 0x05,
        Dec = 0x06,
        PrintAddr = 0x07,
        Call1S = 0x08,
        RemoveObj = 0x09,
        PrintObj = 0x0a,
        Ret = 0x0b,
        Jump = 0x0c,
        PrintPAddr = 0x0d,
        Load = 0x0e,
        // Version <= 4
        Not = 0x0f,
        //Version > 4
        Call1N = 0x0f
    }

    public enum Kind2OpCodes : ushort
    {
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
        Call2S = 0x19,
        Call2N = 0x1a,
        SetColor = 0x1b



    }
}