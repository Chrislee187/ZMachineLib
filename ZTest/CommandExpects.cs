namespace ZTest
{
    public class CommandExpects
    {
        public int LineNo { get; }
        public string Command { get; }
        public string Expectation { get; }

        public bool HasCommand => !string.IsNullOrEmpty(Command);
        public bool HasExpectation => !string.IsNullOrEmpty(Expectation);

        public CommandExpects(string command, string expectation, int lineNo)
        {
            LineNo = lineNo;
            Command = command;
            Expectation = expectation;
        }
    }
}