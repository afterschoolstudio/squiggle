using System;
using System.Linq;
using System.Collections.Generic;
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
        public string Parsed = "";
        public SquiggleCommand(string[] args)
        {
            Args = args;
            Parsed = "";
            for (int i = 0; i < Args.Length; i++)
            {
                Parsed += $"({i}){Args[i]} ";
            }
        }

        public SquiggleCommand GetCommand()
        {
            if(this is Dialog)
            {
                return this as Dialog;
            }
            else
            {
                //should cache this so we only do it once
                var options = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(t => t.IsSubclassOf(typeof(SquiggleCommand)) && !t.IsAbstract));
                foreach (var opt in options)
                {
                    Console.WriteLine("found squiggle type " +opt.Name);
                    var attr = (SquiggleCommandAttribute) Attribute.GetCustomAttribute(opt, typeof (SquiggleCommandAttribute)); 
                    if(attr != null)
                    {
                        if(Args[0] == attr.CommandTrigger)
                        {
                            return (SquiggleCommand)Activator.CreateInstance(opt,new object[] {Args});
                        }
                    }  
                }
                //just return this raw if it doesnt work
                return this;

            }
        }
    }

    public class SquiggleCommandGroup
    {
        public List<SquiggleCommand> SquiggleCommands = new List<SquiggleCommand>();
        public SquiggleCommandGroup(IEnumerable<SquiggleCommand> commands)
        {
            foreach (var c in commands)
            {
                SquiggleCommands.Add(c.GetCommand());
            }
        }
    }
}