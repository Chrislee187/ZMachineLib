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
}