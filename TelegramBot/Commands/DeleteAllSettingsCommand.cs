using JobFinder.Data;
using JobFinder.TelegramState.StateMachine;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace JobFinder.Commands;

public class DeleteAllSettings : Command
{
    private DataContext _context = new DataContext();
    public override string Name => "/deleteall";
    public override async Task Execute(string command, Message message, ITelegramBotClient client, State state)
    {
        var settings = await _context.GetSettings.FindAsync(s => s.UserId == message.Chat.Id).Result.FirstOrDefaultAsync();
        
        if(settings == null)
             await client.SendTextMessageAsync(message.Chat.Id,"Нечего удалять!");
        else
        {
            await _context.GetSettings.DeleteManyAsync(u => u.UserId == message.Chat.Id);

            await _context.StateMachine.DeleteManyAsync(u=>u.Id == message.Chat.Id);
            
            await client.SendTextMessageAsync(message.Chat.Id,"Настройки удалены!");
        }

        
    }
}