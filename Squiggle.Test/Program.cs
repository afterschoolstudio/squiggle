using static Squiggle;

string testScript = @"
Test: Hello, I'm speaking.
[timer 3000]
Another: Now It's me";

Squiggle.Run(testScript,new Squiggle.Runner.Options());
