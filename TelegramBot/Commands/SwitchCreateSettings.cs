using JobFinder.Data;
using JobFinder.Models;
using JobFinder.SettingWriter;
using JobFinder.TelegramState.StateMachine;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace JobFinder.Commands;

public class SwitchCreateSettings : Command
{
    private readonly StateMachine _stateMachine;
    
    private readonly DataContext _context = new DataContext();
    
    private readonly SettingRabota _settingRabota = new SettingRabota();
    public SwitchCreateSettings()
    {

        _stateMachine = new StateMachine(_context);
    }

    public override string Name  => "/switchsettings";
    public override async Task Execute(String command,Message message, ITelegramBotClient client, State state)
    {
        
        var commandsList = SetListCommands();
        
        if (state.StateCommand == "")
        {
            state.ActionCommand = "/switchsettings";
            
            state.StateCommand = "start";
        }

        state.Id = message.Chat.Id;
        
        switch (state.StateCommand)
        {
            case "start":
                
                await client.SendTextMessageAsync(message.Chat.Id, "Добавить настройки для(указать цифру):" +
                                                                   "\n1. Rabota.ua" +
                                                                   "\n2. Work.Ua" +
                                                                   "\n3. Dou.ua" +
                                                                   "\n4. Удалить все настройки");
                state.ActionCommand = "/switchsettings";
                
                state.StateCommand = "choice";
                
                break;
            
            case "choice":

                state.StateCommand = "";
                
                await _stateMachine.UpdateState(state);
                
                var currentState = await _context.SettingState.FindAsync(f => f.UserId == message.Chat.Id).Result.FirstOrDefaultAsync();
                
                if(currentState != null)
                    await _context.SettingState.DeleteManyAsync(u=>u.UserId == message.Chat.Id);
                
                switch (message.Text)
                {
                    
                    case "1":
                        
                        state.ActionCommand = "/createrabota";
                        
                        await _context.SettingState.InsertOneAsync(await _settingRabota.CreateWriter(new RabotaUa(),message.Chat.Id));
                        
                        foreach (var cm in commandsList)
                        {
                          
                            if (cm.Equals(state.ActionCommand))
                            {
                               await cm.Execute(state.ActionCommand,message, client,state);
                                
                                break;
                            }
                        }
                        
                        break;
                    
                    case "2":
                        
                        state.ActionCommand = "/creatework";
                        
                        await _context.SettingState.InsertOneAsync(await _settingRabota.CreateWriter(new WorkUa(),message.Chat.Id));
                        
                        foreach (var cm in commandsList)
                        {
                          
                            if (cm.Equals(state.ActionCommand))
                            {
                                await cm.Execute(state.ActionCommand,message, client,state);
                                
                                break;
                            }
                        }
                        
                       
                        break;
                    case "3":
                      
                        state.ActionCommand = "/createdou";
                       
                        await _context.SettingState.InsertOneAsync(await _settingRabota.CreateWriter(new DouUa(),message.Chat.Id));
                       
                        foreach (var cm in commandsList.Where(cm => cm.Equals(state.ActionCommand)))
                        {
                            await cm.Execute(state.ActionCommand,message, client,state);
                                
                            break;
                        }
                        
                        break;
                    
                    case "4":
                        state.ActionCommand = "/deleteall";
                        
                        foreach (var cm in commandsList.Where(cm => cm.Equals(state.ActionCommand)))
                        {
                            await cm.Execute(state.ActionCommand,message, client,state);
                                
                            break;
                        }
                        
                        break;
                    
                    default:
                        
                        state.StateCommand = "choice";
                        
                        await _stateMachine.UpdateState(state);
                        
                        await client.SendTextMessageAsync(message.Chat.Id, "Неверный параметр! Выберите ещё раз:");
                        
                        break;
                }
                
                break;
        }

        await _stateMachine.UpdateState(state);
    }
}