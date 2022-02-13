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
        public Dialog(string speaker, string dialog) : base(new string[]{speaker,dialog})
        {
            DialogData = new Data()
            {
                Speaker = speaker,
                Text = dialog
            };
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

    [SquiggleCommand("timer")]
    public class Timer : SquiggleCommand
    {
        public int WaitMS => Int32.Parse(Args[1]);
        public Timer(string[] args) : base(args){}
        public override void Execute()
        {
            Thread.Sleep(WaitMS);
            CommandExecutionComplete?.Invoke();
        }
    }


}