using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.TelegramState;

namespace TelegramBot.Parser;

public interface IParserWorker
{
    public Task Start(Message message, ITelegramBotClient client,CancellationTokenSource cts);
}