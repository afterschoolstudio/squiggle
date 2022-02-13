using System;

namespace Squiggle
{
    [System.AttributeUsage(System.AttributeTargets.Class)]  
    public class SquiggleCommandAttribute : Attribute
    {
        public string CommandTrigger {get; protected set;}
        public SquiggleCommandAttribute(string commandStart)
        {
            CommandTrigger = commandStart;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]  
    public class ArgAttribute : Attribute
    {
        public int ArgumentIndex {get; protected set;}
        public ArgAttribute(int argIndex)
        {
            ArgumentIndex = argIndex;
        }
    }
}