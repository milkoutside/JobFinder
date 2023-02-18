using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.TelegramState.StateMachine;

namespace TelegramBot.Core.Commands.Command;

public class HelpCommand : Command
{
    public override string Name => "/help";
    public override async Task Execute(string command, Message message, ITelegramBotClient client, State state)
    {
        await client.SendTextMessageAsync(message.Chat.Id,
            "Добавить настройки - /switchsettings" 
            + "\nУдалить все настройки - /deleteall"
            + "\nНачать поиск - /startsearch" +
            "\nОстановить поиск - /stopsearch");
    }
}