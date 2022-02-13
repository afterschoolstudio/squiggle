using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Squiggle.Commands
{
    public abstract class Command
    {
        public Action CommandExecutionComplete;
        public virtual void Execute()
        {
            CommandExecutionComplete?.Invoke();
        }
        public virtual void Cleanup() {}
    }

    public class SquiggleCommand : Command
    {
        public string[] Args;
        public string Parsed
        {
            get
            {
                var s = "";
                for (int i = 0; i < Args.Length; i++)
                {
                    s += $"({i}){Args[i]} ";
                }
                return s;
            }
        }
    }

    public class SquiggleCommandGroup
    {
        public List<SquiggleCommand> SquiggleCommands = new List<SquiggleCommand>();
        public SquiggleCommandGroup(List<SquiggleCommand> commands)
        {
            SquiggleCommands = commands;
        }
    }
}