using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.SearchObserver;
using TelegramBot.Core.TelegramState;
using TelegramBot.Core.TelegramState.StateMachine;
using TelegramBot.Data;
using TelegramBot.Parser;

namespace TelegramBot.Core.Commands.Command;

public class StopSearch : Command
{
    public override string Name => "/stopsearch";

    private readonly DataContext _context;

    
    
    public StopSearch()
    {
        _context = new DataContext();
    }


    public override async Task Execute(String command, Message message, ITelegramBotClient client, State state)
    {
        
        var chatId = message.Chat.Id;
        var searchState = await _context.SearchState.FindAsync(f => f.UserId == message.Chat.Id).Result.FirstOrDefaultAsync();

        if (searchState == null)
        {
            await client.SendTextMessageAsync(message.Chat.Id,
                "Нечего останавливать! Начните поиск, чтобы что-то остановить.");
        }
        else
        {
            try
            {
                Search search = new Search();
            
                search.State = false;
            
                search.Notify(message.Chat.Id);
            
                search.Detach(message.Chat.Id);
                
            }
            catch (Exception e)
            {
                await _context.SearchState.DeleteOneAsync(u => u.UserId == message.Chat.Id);
                
                await client.SendTextMessageAsync(message.Chat.Id, "Поиск остановлен!");
            }

            
  
        }









    }
   
 





    
}








