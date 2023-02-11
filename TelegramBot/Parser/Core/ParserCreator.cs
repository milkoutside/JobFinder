using TelegramBot.Core.TelegramState.StateMachine;

namespace TelegramBot.Parser;

public abstract class ParserCreator
{
    public abstract IParser CreateParser();

    public abstract ICompare Compare();
}