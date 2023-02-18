using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Data;
using TelegramBot.Parser;
using ParserWorker = Parser.Core.ParserWorker;


namespace TelegramBot.Core.SearchObserver;

public class Searching : IObserver
{


    private CancellationTokenSource _cts;
    


    private IParserWorker _parserWorker;

    private readonly Message _message;
    
    private readonly ITelegramBotClient _client;

    public Searching( DataContext context,Message message, ITelegramBotClient client)
    {
        _message = message;
        _client = client;
        _cts = new CancellationTokenSource();
        _parserWorker = new Parser.ParserWorker(context);

    }
    public void Update(ISubject subject)
    {

        if ((subject as Search).State == false)
        {
           StopSearch();
        }
        else
        {
            StartSearch();
        }
    }
    public void StartSearch()
    {
        StartLoop(_message, _client);
  
    }
    public void StopSearch()
    {
        StopLoop();
        
    }

    
    private async void StartLoop(Message message, ITelegramBotClient client)
    {
  
        try
        {
            
            await Task.Run(async()=> await RunLoopAsync(_cts.Token,message,client),_cts.Token);
            
        }
        catch (Exception ex)
        {
            await client.SendTextMessageAsync(message.Chat.Id, "Неизвестная ошибка!");
            Console.WriteLine(ex);
        }
 
    }

    private void StopLoop()
    {
        _cts.Cancel();
    
    }
    private async Task RunLoopAsync(CancellationToken token,Message message, ITelegramBotClient client)
    {
        try
        {
            while (true)
            {
                if (_cts.Token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                    return;
                }
                
                await _parserWorker.Start(message, client,_cts);

                await Task.Delay(100000, token); // подождать одну секунду
            }
        }
        catch (OperationCanceledException)
        {
            _cts.Dispose();
        } 
    }




}