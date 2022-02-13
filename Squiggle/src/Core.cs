using Squiggle.Commands;
using Squiggle.Parser;
using Sprache;

namespace Squiggle
{
    public static class Core
    {
        public static SquiggleCommandGroup Parse(string squiggleText)
        {
            squiggleText = squiggleText.Replace("\r\n", "\\r\\n"); //need to replace these due to serialization
            // Clog.L($"text to parse: {Text.Replace("\n", "D")}"); also works - may need to do this as well for OSX line endings
            return SquiggleGrammar.Commands.Parse(squiggleText);
        }
        public static Runner Run(string squiggleText, Runner.Options opts) => Run(Parse(squiggleText),opts);
        public static Runner Run(SquiggleCommandGroup group, Runner.Options opts)
        {
            return new Runner(group.SquiggleCommands,opts);
        }
    }
}