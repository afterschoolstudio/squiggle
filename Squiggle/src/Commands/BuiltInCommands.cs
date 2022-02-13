using System;
using System.Threading;
using Squiggle.Events;

namespace Squiggle.Commands
{
    
    //speaker commands are used directly by the parser, no need for attribute
    public class Dialog : SquiggleCommand
    {
        public class Data
        {
            public string Speaker; 
            public string Text; 
        }
        public Data DialogData;
        public Dialog(Data d)
        {
            DialogData = d;
            Args = new string[]{d.Speaker,d.Text}; 
        }
        public override void Execute()
        {
            Squiggle.Events.Commands.CompleteDialog += OnDialogComplete;
            Squiggle.Events.Dialog.EmitDialog?.Invoke(this,DialogData);
        }

        void OnDialogComplete(Dialog dialogCommand)
        {
            if(dialogCommand == this)
            {
                Squiggle.Events.Commands.CompleteDialog -= OnDialogComplete;
                CommandExecutionComplete?.Invoke();
            }
        }
        public override void Cleanup()
        {
            Squiggle.Events.Commands.CompleteDialog -= OnDialogComplete;
        }
        
    }

    [SquiggleCommand("wait")]
    public class Wait : SquiggleCommand
    {
        [Arg(1)] public int WaitMS;
        public override void Execute()
        {
            Thread.Sleep(WaitMS);
            CommandExecutionComplete?.Invoke();
        }
    }


}