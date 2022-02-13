using System;
using System.Threading.Tasks;

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
        public override async void Execute()
        {
            Events.Commands.Dialog.EmitDialog?.Invoke(this,DialogData);
            Events.Commands.Dialog.CompleteDialog += OnDialogComplete;
        }

        void OnDialogComplete(Dialog dialogCommand)
        {
            if(dialogCommand == this)
            {
                Events.Commands.Dialog.CompleteDialog -= OnDialogComplete;
                CommandExecutionComplete?.Invoke();
            }
        }
        public override void Cleanup()
        {
            Events.Commands.Dialog.CompleteDialog -= OnDialogComplete;
        }
        
    }

    [SquiggleCommand("timer")]
    public class StartAudioInstanceCommand : SquiggleCommand
    {
        public int WaitMS => Int32.Parse(Args[1]);
        public StartAudioInstanceCommand(string[] args) : base(args){}
        public override async void Execute()
        {
            await Task.Delay(WaitMS);
            CommandExecutionComplete?.Invoke();
        }
    }


}