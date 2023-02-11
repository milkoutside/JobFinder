using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Parser;

public interface IParserWorker
{
    public Task Work(Message message, ITelegramBotClient client,long id);

    public Task Start();
}