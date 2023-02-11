using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Commands;
using TelegramBot.Core.TelegramState.StateMachine;
using TelegramBot.Data;

namespace TelegramBot.Core;

public class BotWorker
{
    private static TelegramBotClient _botClient;
 
    private static List<Command> commandsList;

    private readonly DataContext _context;
    public BotWorker()
    {
        _context = new DataContext();
        _botClient = new TelegramBotClient($"{SettingsBot.Key}");
        commandsList = new List<Command>();
    }

    public async void Start()
    {
       await Worker();
    }
   
    private async Task Worker()
    {
        _botClient.StartReceiving(Update, Error);
        
    }

    private async Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        Console.WriteLine(arg2);
    }

    private async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        commandsList = Command.SetListCommands();
        var message  = update.Message;
        
        var tgState = _context.StateMachine.Find(e => e.Id == message.Chat.Id).FirstOrDefault();
        if (tgState == null)
        {
            tgState = new State();
            tgState.Id = message.Chat.Id;
            await _context.StateMachine.InsertOneAsync(tgState, cancellationToken: token);
        }

        switch (message)
        {
            case var m when tgState.ActionCommand != "":
                foreach (var command in commandsList)
                {
                    if (command.Equals(tgState.ActionCommand))
                    {
                        command.Execute(tgState.ActionCommand,message, botClient,tgState);
                        break;
                    }
                }
                break;
            case var m when m.Text.Contains("/"):
                foreach (var command in commandsList)
                {
                    if (command.Equals(m.Text))
                    {
                        command.Execute(message.Text,message, botClient,tgState);
                        break;
                    }
                }
                break;
            default:
                await botClient.SendTextMessageAsync(message.Chat.Id, "Неизвестная команда!" +
                                                                      "\nВведите команду /help, чтобы посмотреть все команды.");
                    tgState.ActionCommand = "";
                break;
                
        }
        await _context.StateMachine.ReplaceOneAsync((e => e.Id == message.Chat.Id), tgState);
      
            
    }

    private async Task updateHandler(ITelegramBotClient arg1, Update arg2, CancellationToken arg3)
    {
        
    }
}