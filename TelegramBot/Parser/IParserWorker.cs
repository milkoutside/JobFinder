using Telegram.Bot;
using Telegram.Bot.Types;

namespace JobFinder.Parser;

public interface IParserWorker
{
    public Task Start(Message message, ITelegramBotClient client,CancellationTokenSource cts);
}