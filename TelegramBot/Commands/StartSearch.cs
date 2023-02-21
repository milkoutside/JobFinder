using JobFinder.Data;
using JobFinder.SearchObserver;
using JobFinder.TelegramState;
using JobFinder.TelegramState.StateMachine;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace JobFinder.Commands;

public class StartSearch : Command
{
    public override string Name => "/startsearch";

    private readonly DataContext _context;
    public StartSearch()
    {
        _context = new DataContext();

    }

 

    public override async Task Execute(String command, Message message, ITelegramBotClient client, State state)
    {
    
        
        var currentSearch = await _context.SearchState.FindAsync(f => f.UserId == message.Chat.Id).Result
            .FirstOrDefaultAsync();
        
        var settings = await _context.GetSettings.FindAsync(f => f.UserId == message.Chat.Id).Result.ToListAsync();
        if (currentSearch != null)
            await client.SendTextMessageAsync(message.Chat.Id,
                "Поиск уже работает! Остановите текущий прежде чем начать новый - /stopsearch ");
       
        if(currentSearch == null && settings != null)
        {
            var searchState = new SearchState()
            {
                UserId = message.Chat.Id,
                
                IsWork = "true"
                
            };
            
            await _context.SearchState.InsertOneAsync(searchState);
            
            Search search = new Search();
            
            Searching searching = new Searching(_context,message,client);
            
            search.State = true;
            
            search.Attach(searching,message.Chat.Id);
            
            search.Notify(message.Chat.Id);

            await client.SendTextMessageAsync(message.Chat.Id, "Поиск начат! Подождите немного для результатов.");
        }
      

  

    }






    
}








