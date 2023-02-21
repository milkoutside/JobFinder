using JobFinder.Commands;
using JobFinder.Data;
using JobFinder.Services;
using JobFinder.TelegramState.StateMachine;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace JobFinder.Controllers;

[ApiController]
[Route("api/message/update")]
public class TelegramBotController : ControllerBase
{
    private static TelegramBotClient _botClient;
 
    private static List<Command> commandsList;

    private readonly DataContext _context;
    
    public TelegramBotController()
    {
        _context = new DataContext();

        commandsList = new List<Command>();
    }


    [HttpPost]
    public async Task<IActionResult> Update([FromBody]object update)
    {
        // /start => register user

        var upd = JsonConvert.DeserializeObject<Update>(update.ToString());

        var _client = await new TelegramClient().GetBot();

        if (upd?.Message?.Chat == null && upd?.CallbackQuery == null)
        {
            return Ok();
        }

        try
        {
            commandsList = Command.SetListCommands();
        
            var message  = upd.Message;
        
            var tgState = _context.StateMachine.Find(e => e.Id == message.Chat.Id).FirstOrDefault();
        
            if (tgState == null)
            {
                tgState = new State();
            
                tgState.Id = message.Chat.Id;
            
                await _context.StateMachine.InsertOneAsync(tgState);
            
            }

            switch (message)
            {
                case var m when tgState.ActionCommand != "":
                
                    foreach (var command in commandsList.Where(command => command.Equals(tgState.ActionCommand)))
                    {
                        await command.Execute(tgState.ActionCommand,message, _client,tgState);
                        
                        break;
                    }
                
                    break;
            
                case var m when m.Text.Contains("/"):
                
                    foreach (var command in commandsList.Where(command => command.Equals(m.Text)))
                    {
                        await command.Execute(message.Text,message, _client,tgState);
                        
                        break;
                    }
                
                    break;
            
                default:
                    await _client.SendTextMessageAsync(message.Chat.Id, "Неизвестная команда!" +
                                                                          "\nВведите команду /help, чтобы посмотреть все команды.");
                    tgState.ActionCommand = "";
                
                    break;
                
            }
            await _context.StateMachine.ReplaceOneAsync((e => e.Id == message.Chat.Id), tgState);

        }
        catch (Exception e)
        {
            return Ok();
        }
            
        return Ok();
    }
}