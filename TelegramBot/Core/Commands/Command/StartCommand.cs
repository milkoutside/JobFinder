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
    

    private ParserWorker _parserWorker = new ParserWorker();

    private readonly StateMachine _stateMachine;


    public StartCommand()
    {
        _context = new DataContext();
        _stateMachine = new StateMachine(_context);
    }

    public override async void Execute(String command, Message message, ITelegramBotClient client, State state)
    {
        state.ActionCommand = "/start";
        
        var searchState = await _context.SearchState.FindAsync((e => e.UserId == message.Chat.Id))
            .Result.FirstOrDefaultAsync()??new SearchState()
        {
            _cts = new CancellationTokenSource(),
            
            UserId = message.Chat.Id,
            
            IsWork = "true"
        };
        
        _cts = searchState._cts;
        
        if (message.Text == "/stop")
        {
            searchState.IsWork = "false";
            
            await _stateMachine.DeleteState(message.Chat.Id);
            
            await _context.SearchState.ReplaceOneAsync((f=>f.UserId == message.Chat.Id),searchState);
            
            await client.SendTextMessageAsync(message.Chat.Id, "Поиск остановлен!");
            
            StopLoop();
        }

        if (message.Text == "/start")
        {
            searchState.UserId = message.Chat.Id;
            
            searchState.IsWork = "true";
            
            await _context.SearchState.InsertOneAsync(searchState);
            
            StartLoop(message, client);

        }
    }
   
    private CancellationTokenSource _cts;
    private async Task RunLoopAsync(CancellationToken token,Message message, ITelegramBotClient client )
    {
        try
        {
            while (true)
            {
                var find = await _context.SearchState.FindAsync((e => e.UserId == message.Chat.Id)).Result.FirstOrDefaultAsync()??new SearchState();
               
                if (find.IsWork == "false")
                {
                    await _context.SearchState.DeleteOneAsync(f => f.UserId == message.Chat.Id);
                    
                    return;
                }

                await _parserWorker.Work(message, client, message.Chat.Id);
                
                await Task.Delay(120000, token); 
            }
        }
        catch (OperationCanceledException)
        { } 
    }
    private async void StartLoop(Message message, ITelegramBotClient client)
    {

        try
        {
            using (_cts = new CancellationTokenSource())
            {
                await RunLoopAsync(_cts.Token,message,client);
            }
        }
        catch (Exception ex)
        {
            // ... ex.Message
        }
        _cts = null;
    }

    private void StopLoop()
    {
        _cts?.Cancel();
    }


    
}








