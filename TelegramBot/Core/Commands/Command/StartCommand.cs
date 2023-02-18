using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.TelegramState;
using TelegramBot.Core.TelegramState.StateMachine;
using TelegramBot.Data;
using TelegramBot.Parser;

namespace TelegramBot.Core.Commands.Command;

public class StartCommand : Command
{
    public override string Name => "/start";

    private readonly DataContext _context;


    private ParserWorker _parserWorker;

    private readonly StateMachine _stateMachine;


    public StartCommand()
    {

    }

    public override async Task Execute(String command, Message message, ITelegramBotClient client, State state)
    {
        var chatId = message.Chat.Id;

        await client.SendTextMessageAsync(chatId,
            "Это бот для поиска работы, который должен вам упростить поиск работы." +
            "\nДля нормальной работы, у вас должны быть такие шаги:" +
            "\n1.Вввод настроек(несколько или 1) - /switchsettings" +
            "\n2Потом активируйте поиск - /startsearch " +
            "\n3.Done!" +
            "\nДля более детальной помощи - /help" +
            "\nДодерживайтесь того, что указано в командах! Если не выполнять - есть большая вероятность ошибок, пока создатель их не пофиксит!" +
            "\nПриятного использования.");

    }
    


}








