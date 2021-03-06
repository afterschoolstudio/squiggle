using Squiggle;
using Squiggle.Commands;

string testScript = @"
Test: Hello, I'm speaking
[wait 1000]
Another: Now It's me
[wait 1000]
Another: Here's a sample command
[sampleCustom someArgForACustomCommand]
Final: And Now I'm Here
";


// Squiggle.Core.Run(squiggleText : testScript,
//                   runnerOptions : new Squiggle.Runner.Options(){Debug = true},
//                   dialogHandler : (command) => {Squiggle.Events.Commands.CompleteDialog?.Invoke(command);});

string myScript = "Speaker: Hello! The Speaker is talking!";
Squiggle.Core.Run(  squiggleText : myScript,
                    runnerOptions : new Squiggle.Runner.Options(){Debug = true});
                    
[SquiggleCommand("sampleCustom")]
public class SampleCustom : SquiggleCommand
{
    [Arg(1)] public string LogText;
    public override void Execute()
    {
        Console.WriteLine($"you passed the arg: {LogText} to your custom command");
        CommandExecutionComplete?.Invoke();
    }
}
