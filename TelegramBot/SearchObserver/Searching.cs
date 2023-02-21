using JobFinder.Data;
using JobFinder.Parser;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace JobFinder.SearchObserver;

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
        _parserWorker = new ParserWorker(context);

    }
    public async Task Update(ISubject subject)
    {

        if ((subject as Search).State == false)
        {
          await StopSearch();
        }
        else
        {
          await  StartSearch();
        }
    }

    private async Task StartSearch()
    {
       await StartLoop(_message, _client);
  
    }

    private async Task StopSearch()
    {
       await StopLoop();
        
    }

    
    private async Task StartLoop(Message message, ITelegramBotClient client)
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

    private async Task StopLoop()
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