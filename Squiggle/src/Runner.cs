using System;
using System.Linq;
using System.Collections.Generic;
using Squiggle.Commands;

namespace Squiggle
{
    public class Runner
    {
        List<SquiggleCommand> commands;
        SquiggleCommand workingCommand;
        Options options;
        public Runner(List<SquiggleCommand> commands, Options opts)
        {
            this.commands = commands;
            options = opts;
            if(opts.AutoStart)
            {
                Start();
            }
        }

        public void Start()
        {
            workingCommand = commands.FirstOrDefault();
            if(workingCommand == null)
            {
                //empty list, end the runner
                Events.Runner.CompletedExecution?.Invoke();
            }
            else
            {
                workingCommand.CommandExecutionComplete += OnCurrentCommandComplete;
                Events.Commands.CommandExecutionStarted?.Invoke(workingCommand);
                workingCommand.Execute();
            }
        }

        void TryExecuteNextCommand()
        {
            var commandIndex = commands.IndexOf(workingCommand);
            if(commandIndex == commands.Count - 1)
            {
                //we're at the end of the list, end the runner
                Events.Runner.CompletedExecution?.Invoke();
            }
            else
            {
                //grab the next command
                workingCommand = commands[commandIndex+1];
                workingCommand.CommandExecutionComplete += OnCurrentCommandComplete;
                //execute the command
                workingCommand.Execute();
            }
        }

        void OnCurrentCommandComplete()
        {
            workingCommand.CommandExecutionComplete -= OnCurrentCommandComplete;
            Events.Commands.CommandExecutionCompleted?.Invoke(workingCommand);
            TryExecuteNextCommand();
        }

        public void Cleanup()
        {
            foreach (var command in commands)
            {
                workingCommand.CommandExecutionComplete -= OnCurrentCommandComplete;
                workingCommand.Cleanup();
            }
        }

        public class Options
        {
            public bool AutoStart = true;
            public bool Debug = false;
        }
    }
}