using System.Linq;
using System.Text;
using ZMachineLib.Content;

namespace ZMachineLib
{
    public class ZmDebugger
    {
        private string PROMPT = ">";
        public (bool isDebug, string output) HandleDebugCommand(IZMemory memory, string commandLine)
        {
            var args = commandLine.Split(' ');
            var cmd = args.First();
            
            var sb = new StringBuilder();

            var debugging = cmd.StartsWith("!!");
            if (debugging)
            {
                switch (cmd.ToLower())
                {
                    case "!!header":
                        sb.Append(Format.Header(memory.Header));
                        break;
                    case "!!globals":
                        sb.Append(Format.Globals(memory.Globals));
                        break;
                    case "!!global":
                        byte globalNumber = byte.Parse(args[1]);
                        sb.Append(Format.Global(memory.Globals, globalNumber));
                        break;
                    case "!!object":
                        sb.Append(Format.Object(memory.ObjectTree, byte.Parse(args[1]), true));
                        break;
                    default:
                        sb.AppendLine($"Unknown debug command: {commandLine}\nValid commands: " +
                                      $"header, globals, global #, object #");
                        break;
                }

                sb.Append(PROMPT);
            }

            return (debugging, sb.ToString());
        }
    }
}