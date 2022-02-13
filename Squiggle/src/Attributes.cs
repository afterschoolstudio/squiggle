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
}