using System;
using Squiggle.Commands;
using DialogCommand = Squiggle.Commands.Dialog;

namespace Squiggle
{
    public static class Events
    {
        public static class Runner
        {
            public static Action BeganExecution;
            public static Action CompletedExecution;
        } 
        public static class Commands
        {
            public static Action<SquiggleCommand> CommandExecutionStarted;
            public static Action<SquiggleCommand> CommandExecutionCompleted;
            public static class Dialog
            {
                public static Action<DialogCommand,DialogCommand.Data> EmitDialog;
                public static Action<DialogCommand> CompleteDialog;
            }
        }
    }
}