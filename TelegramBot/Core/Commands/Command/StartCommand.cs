using StateMachineBot;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.TelegramState;
using TelegramBot.Core.TelegramState.StateMachine;
using TelegramBot.Data;
using TelegramBot.Parser;

namespace TelegramBot.Core.Commands;

public class StartCommand : Command
{
    public override string Name => "/start";
    private readonly DataContext _context;
    private ParserWorker _parserWorker;
    private readonly StateMachine _stateMachine;
    

    public StartCommand()
    {
        _context = new DataContext();
        _stateMachine = new StateMachine(_context);
    }

    public async override void Execute(String command,Message message, ITelegramBotClient client,State state)
    {
        _parserWorker = new ParserWorker();
        await _parserWorker.Work(message, client, message.Chat.Id);
        var chatId = message.Chat.Id;
        await _stateMachine.DeleteState(chatId);
 
    }
}