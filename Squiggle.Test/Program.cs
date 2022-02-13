using Squiggle;
using Squiggle.Commands;

string testScript = @"
Test: Hello, I'm speaking
[timer 1000]

Another: Now It's me
[timer 1000]
Another: Here's a sample command
[sampleCustom someArgForACustomCommand]
Final: And Now I'm Here
";


Squiggle.Core.Run(testScript,new Squiggle.Runner.Options(){Debug = true});

[SquiggleCommand("sampleCustom")]
public class SampleCustom : SquiggleCommand
{
    public string LogText => Args[1];
    public SampleCustom(string[] args) : base(args){}
    public override void Execute()
    {
        Console.WriteLine($"you passed the arg: {LogText} to your custom command");
        CommandExecutionComplete?.Invoke();
    }
}
