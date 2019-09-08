namespace ZMachineLib.Operations
{
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
}