namespace TelegramBot.Parser.Core;

public abstract class ParserCreator
{
    public abstract IParser CreateParser();

    public abstract ICompare Compare();
}