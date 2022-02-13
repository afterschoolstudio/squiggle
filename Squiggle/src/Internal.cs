using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using Squiggle.Commands;

namespace Squiggle
{
    internal static class Internal
    {
        public static Dictionary<string,Type> CustomCommandMap()
        {
            var dict = new Dictionary<string,Type>();
            var customClasses = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(t => t.IsSubclassOf(typeof(SquiggleCommand)) && !t.IsAbstract));
            foreach (var c in customClasses)
            {
                var attr = (SquiggleCommandAttribute) Attribute.GetCustomAttribute(c, typeof (SquiggleCommandAttribute)); 
                if(attr != null)
                {
                    dict.Add(attr.CommandTrigger,c);
                }
                else
                {
                    if(c.Name != "Dialog")
                    {
                        Console.WriteLine($"Unreachable Squiggle Command class detected: {c.Name}");
                    }
                }
            }
            return dict;
        }

        public static SquiggleCommand CreateCommandForCommandTrigger(string[] args)
        {
            var trigger = args[0];
            var map = CustomCommandMap();
            var returnCommand = ((SquiggleCommand)Activator.CreateInstance(map[trigger]));
            returnCommand.Args = args;

            foreach (var prop in returnCommand.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy))
            {
                var bindAttr = (ArgAttribute) Attribute.GetCustomAttribute(prop, typeof (ArgAttribute)); 
                if(bindAttr != null && bindAttr.ArgumentIndex <= returnCommand.Args.Length && bindAttr.ArgumentIndex > 0)
                {
                    switch (prop.PropertyType.Name)
                    {
                        case nameof(String):
                            prop.SetValue(returnCommand,returnCommand.Args[bindAttr.ArgumentIndex]);
                            break;
                        case nameof(Int32):
                            prop.SetValue(returnCommand,Int32.Parse(returnCommand.Args[bindAttr.ArgumentIndex]));
                            break;
                        case nameof(Single):
                            prop.SetValue(returnCommand,Single.Parse(returnCommand.Args[bindAttr.ArgumentIndex]));
                            break;
                        case nameof(Boolean):
                            prop.SetValue(returnCommand,Boolean.Parse(returnCommand.Args[bindAttr.ArgumentIndex]));
                            break;
                        default:
                            Console.WriteLine($"Tried to bind property {prop.Name} on {returnCommand.GetType().Name} with invalid type {prop.PropertyType.Name} - No value will be set");
                            break;
                    }
                }
            }
            foreach (var field in returnCommand.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy))
            {
                var bindAttr = (ArgAttribute) Attribute.GetCustomAttribute(field, typeof (ArgAttribute)); 
                if(bindAttr != null && bindAttr.ArgumentIndex <= returnCommand.Args.Length && bindAttr.ArgumentIndex > 0)
                {
                    switch (field.FieldType.Name)
                    {
                        case nameof(String):
                            field.SetValue(returnCommand,returnCommand.Args[bindAttr.ArgumentIndex]);
                            break;
                        case nameof(Int32):
                            field.SetValue(returnCommand,Int32.Parse(returnCommand.Args[bindAttr.ArgumentIndex]));
                            break;
                        case nameof(Single):
                            field.SetValue(returnCommand,Single.Parse(returnCommand.Args[bindAttr.ArgumentIndex]));
                            break;
                        case nameof(Boolean):
                            field.SetValue(returnCommand,Boolean.Parse(returnCommand.Args[bindAttr.ArgumentIndex]));
                            break;
                        default:
                            Console.WriteLine($"Tried to bind field {field.Name} on {returnCommand.GetType().Name} with invalid type {field.FieldType.Name} - No value will be set");
                            break;
                    }
                }
            }

            return returnCommand;
        }
    }
    
}