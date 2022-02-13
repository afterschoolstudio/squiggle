using System;
using Squiggle.Commands;
using DialogCommand = Squiggle.Commands.Dialog;

namespace Squiggle.Events
{
    public static class Commands
    {
        public static Action<DialogCommand> CompleteDialog;
    }
    internal static class Dialog
    {
        internal static Action<DialogCommand,DialogCommand.Data> EmitDialog;

    }
}