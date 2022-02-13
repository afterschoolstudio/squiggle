using System;
using System.Linq;
using System.Collections.Generic;
using Squiggle.Commands;
using Squiggle.Events;

namespace Squiggle
{
    public class Runner
    {
        List<SquiggleCommand> commands;
        SquiggleCommand workingCommand;
        Options options;
        string GUID;

        public Action BeganExecution;
        public Action CompletedExecution;
        public Action<SquiggleCommand> CommandExecutionStarted;
        public Action<SquiggleCommand> CommandExecutionCompleted;
        Action<Squiggle.Commands.Dialog> DialogHandler;
        public Runner(List<SquiggleCommand> commands, Options runnerOptions, Action<Squiggle.Commands.Dialog> dialogHandler = null)
        {
            GUID = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            this.commands = commands;
            options = runnerOptions;
            DialogHandler = dialogHandler;
            if(DialogHandler == null)
            {
                DialogHandler = (command) => {Squiggle.Events.Commands.CompleteDialog?.Invoke(command);};
            }
            Squiggle.Events.Dialog.EmitDialog += DialogHandler;
            
            if(runnerOptions.AutoStart)
            {
                Start();
            }
        }

        public void Start()
        {
            Log($"Beginning Runner Execution With {commands.Count} Commands");
            BeganExecution?.Invoke();
            workingCommand = commands.FirstOrDefault();
            if(workingCommand == null)
            {
                Log($"No Commands Found, Exiting Runner");
                //empty list, end the runner
                Cleanup();
                CompletedExecution?.Invoke();
            }
            else
            {
                workingCommand.CommandExecutionComplete += OnCurrentCommandComplete;
                Log($"Starting Execution for Command Type {workingCommand.GetType().Name} With Args: {workingCommand.Parsed}");
                CommandExecutionStarted?.Invoke(workingCommand);
                workingCommand.Execute();
            }
        }

        void TryExecuteNextCommand()
        {
            var commandIndex = commands.IndexOf(workingCommand);
            if(commandIndex == commands.Count - 1)
            {
                Log($"Execution Complete, Exiting");
                //we're at the end of the list, end the runner
                Cleanup();
                CompletedExecution?.Invoke();
            }
            else
            {
                //grab the next command
                workingCommand = commands[commandIndex+1];
                workingCommand.CommandExecutionComplete += OnCurrentCommandComplete;
                Log($"Starting Execution for Command Type {workingCommand.GetType().Name} With Args: {workingCommand.Parsed}");
                //execute the command
                workingCommand.Execute();
            }
        }

        void OnCurrentCommandComplete()
        {
            Log($"Completed Execution for Command Type {workingCommand.GetType().Name} With Args: {workingCommand.Parsed}");
            workingCommand.CommandExecutionComplete -= OnCurrentCommandComplete;
            CommandExecutionCompleted?.Invoke(workingCommand);
            TryExecuteNextCommand();
        }

        public void Cleanup()
        {
            if(DialogHandler != null)
            {
                Squiggle.Events.Dialog.EmitDialog -= DialogHandler;
            }
            foreach (var command in commands)
            {
                workingCommand.CommandExecutionComplete -= OnCurrentCommandComplete;
                workingCommand.Cleanup();
            }
        }

        void Log(string text)
        {
            if(options.Debug)
            {
                Console.WriteLine($"Squiggle Runner {GUID}: {text}");
            }
        }

        public class Options
        {
            public bool AutoStart = true;
            public bool Debug = false;
        }
    }
}