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
        // public SquiggleCommand GetCommand()
        // {
        //     if(this is Dialog)
        //     {
        //         return this as Dialog;
        //     }
        //     else
        //     {
        //         //should cache this so we only do it once
        //         var customCommands = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(t => t.IsSubclassOf(typeof(SquiggleCommand)) && !t.IsAbstract));
        //         foreach (var command in customCommands)
        //         {
        //             var attr = (SquiggleCommandAttribute) Attribute.GetCustomAttribute(command, typeof (SquiggleCommandAttribute)); 
        //             if(attr != null)
        //             {
        //                 if(Args[0] == attr.CommandTrigger)
        //                 {
        //                     var returnCommand = (SquiggleCommand)Activator.CreateInstance(command);
        //                     foreach (var prop in returnCommand.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy))
        //                     {
        //                         var bindAttr = (ArgAttribute) Attribute.GetCustomAttribute(prop, typeof (ArgAttribute)); 
        //                         if(bindAttr != null && bindAttr.ArgumentIndex <= Args.Length && bindAttr.ArgumentIndex > 0)
        //                         {
        //                             Console.WriteLine("checking prop " + prop.PropertyType.Name);
        //                             switch (prop.PropertyType.Name)
        //                             {
        //                                 case nameof(String):
        //                                     prop.SetValue(returnCommand,Args[bindAttr.ArgumentIndex]);
        //                                     break;
        //                                 case nameof(Int32):
        //                                     prop.SetValue(returnCommand,Int32.Parse(Args[bindAttr.ArgumentIndex]));
        //                                     break;
        //                                 case nameof(Single):
        //                                     prop.SetValue(returnCommand,Single.Parse(Args[bindAttr.ArgumentIndex]));
        //                                     break;
        //                                 case nameof(Boolean):
        //                                     prop.SetValue(returnCommand,Boolean.Parse(Args[bindAttr.ArgumentIndex]));
        //                                     break;
        //                                 default:
        //                                     Console.WriteLine($"Tried to bind property {prop.Name} on {returnCommand.GetType().Name} with invalid type {prop.PropertyType.Name} - No value will be set");
        //                                     break;
        //                             }
        //                         }
        //                     }
        //                     foreach (var field in returnCommand.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy))
        //                     {
        //                         var bindAttr = (ArgAttribute) Attribute.GetCustomAttribute(field, typeof (ArgAttribute)); 
        //                         if(bindAttr != null && bindAttr.ArgumentIndex <= Args.Length && bindAttr.ArgumentIndex > 0)
        //                         {
        //                             Console.WriteLine("checking field " + field.FieldType.Name);
        //                             switch (field.FieldType.Name)
        //                             {
        //                                 case nameof(String):
        //                                     field.SetValue(returnCommand,Args[bindAttr.ArgumentIndex]);
        //                                     break;
        //                                 case nameof(Int32):
        //                                     field.SetValue(returnCommand,Int32.Parse(Args[bindAttr.ArgumentIndex]));
        //                                     break;
        //                                 case nameof(Single):
        //                                     field.SetValue(returnCommand,Single.Parse(Args[bindAttr.ArgumentIndex]));
        //                                     break;
        //                                 case nameof(Boolean):
        //                                     field.SetValue(returnCommand,Boolean.Parse(Args[bindAttr.ArgumentIndex]));
        //                                     break;
        //                                 default:
        //                                     Console.WriteLine($"Tried to bind field {field.Name} on {returnCommand.GetType().Name} with invalid type {field.FieldType.Name} - No value will be set");
        //                                     break;
        //                             }
        //                         }
        //                     }
        //                     return returnCommand;
        //                 }
        //             }  
        //         }
        //         //just return this raw if it doesnt work
        //         return this;

        //     }
        // }
    }

    public class SquiggleCommandGroup
    {
        public List<SquiggleCommand> SquiggleCommands = new List<SquiggleCommand>();
        public SquiggleCommandGroup(IEnumerable<SquiggleCommand> commands)
        {
            SquiggleCommands = commands.ToList();
            // foreach (var c in commands)
            // {
            //     SquiggleCommands.Add(c.GetCommand());
            // }
        }
    }
}