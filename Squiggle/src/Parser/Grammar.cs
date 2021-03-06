using System.Linq;
using System.Collections.Generic;
using Sprache;
using Squiggle.Commands;

namespace Squiggle.Parser
{
    
    public class SquiggleGrammar
    {
        // static readonly Parser<string> NewLine =
        //     Parse.String(Environment.NewLine).Text();
        //  static readonly Parser<string> LineTerminator =
        //     Parse.Return("").End().XOr(
        //     Line.End()).Or(
        //     Line);

        static readonly Parser<char> LiteralLineContent = Parse.AnyChar.Except(Parse.LineTerminator).Except(Parse.LineEnd);
        static readonly Parser<string> Line = LiteralLineContent.AtLeastOnce().Text();
        static readonly Parser<IEnumerable<char>> LineEnd = Parse.LineEnd.XOr(Parse.LineTerminator).XOr(Parse.String("\\r\\n")).XOr(Parse.String("\\n")).XOr(Parse.String("\n")).XOr(Parse.String("\r\n"));
        public static readonly Parser<SquiggleCommandGroup> Commands =
            from introLines in LineEnd.Many()
            from commands in Parse.Ref(()=>SpeakerText).XOr(Parse.Ref(() => Instruction)).Many()
            from outroLines in LineEnd.Many()
            select new SquiggleCommandGroup(commands.ToList());
		public static readonly Parser<string> CommandParameter = 
                (from content in Parse.CharExcept(' ').Until(Parse.LineTerminator).Text()
                select content).Token();
		public static readonly Parser<string> Identifier = Parse.Letter.AtLeastOnce().Text().Token();
		public static readonly Parser<string> BigIdentifier = 
                        (Parse.Chars(":/_")
                        .XOr(Parse.LetterOrDigit))
                        .AtLeastOnce().Text().Token();
		public static readonly Parser<char> Colon = Parse.Char(':');
		public static readonly Parser<char> OpenBracket = Parse.Char('[');
		public static readonly Parser<char> CloseBracket = Parse.Char(']');
		public static readonly Parser<SquiggleCommand> Instruction = 
                from startBuffer in LineEnd.Many()
                from open in OpenBracket.AtLeastOnce().Token()
                from commandParams in BigIdentifier.Many()
                from close in CloseBracket.Token()
                from endBuffer in LineEnd.Many()
                select Internal.CreateCommandForCommandTrigger(commandParams.ToArray());
                // select new SquiggleCommand(){Args = commandParams.ToArray()};
		public static readonly Parser<Dialog> SpeakerText = 
                from startBuffer in LineEnd.Many()
                from lead in Parse.LetterOrDigit.AtLeastOnce().Text()
                from speaker in Parse.AnyChar.Until(Colon).Token().Text()
                from dialog in Parse.AnyChar.Until(LineEnd).Text()
                from endBuffer in LineEnd.Many()

                select new Dialog(lead+speaker,dialog);
    }
}