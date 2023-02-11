using ConsoleApp2.SettingWritter;
using MongoDB.Driver;
using StateMachineBot;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.TelegramState.StateMachine;
using TelegramBot.Data;

namespace TelegramBot.Core.Commands;

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
    public override async void Execute(String command,Message message, ITelegramBotClient client, State state)
    {
        
        var commandsList = SetListCommands();
        if (state.StateCommand == "")
        {
            state.ActionCommand = "/switchsettings";
            state.StateCommand = "start";
        }
        switch (state.StateCommand)
        {
            case "start":
                await client.SendTextMessageAsync(message.Chat.Id, "Добавить настройки для:" +
                                                                   "\n1. Rabota.ua" +
                                                                   "\n2. Work.Ua" +
                                                                   "\n3. Dou.ua" +
                                                                   "\n4. Назад");
                state.ActionCommand = "/switchsettings";
                state.StateCommand = "choice";
                break;
            case "choice":
                
                state.ActionCommand = "/createsettings";
                state.StateCommand = "";
                await _context.StateMachine.ReplaceOneAsync((e => e.Id == message.Chat.Id), state);
                var currentState = await _context.SettingState.FindAsync(f => f.UserId == message.Chat.Id).Result.FirstOrDefaultAsync();
                if(currentState == null)
                    await _context.SettingState.InsertOneAsync(await _settingRabota.CreateWriter(new RabotaUa(),message.Chat.Id));
                
                switch (message.Text)
                {
                    
                    case "1":
                        foreach (var cm in commandsList)
                        {
                          
                            if (cm.Equals(state.ActionCommand))
                            {
                                cm.Execute(state.ActionCommand,message, client,state);
                                break;
                            }
                        }
                        break;
                }
                break;
        }
    
        await _context.StateMachine.ReplaceOneAsync((e => e.Id == message.Chat.Id), state);
    }
}