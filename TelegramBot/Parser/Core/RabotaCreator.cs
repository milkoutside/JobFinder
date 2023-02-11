using TelegramBot.Core.TelegramState.StateMachine;

namespace TelegramBot.Parser;

public class RabotaCreator : ParserCreator
{
    public override IParser CreateParser()
    {
        return new ParserRabotaUa();
    }

    public override ICompare Compare()
    {
        return new Compare();
    }
}