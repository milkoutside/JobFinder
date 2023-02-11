namespace TelegramBot.Parser.Core;

public class WorkCreator : ParserCreator
{
    public override IParser CreateParser()
    {
        return new ParserWorkUa();
    }

    public override ICompare Compare()
    {
        return new Compare();
    }
}