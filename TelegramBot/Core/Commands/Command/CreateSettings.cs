using MongoDB.Driver;
using StateMachineBot;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Core.TelegramState;
using TelegramBot.Core.TelegramState.StateMachine;
using TelegramBot.Data;
using TelegramBot.Models;

namespace TelegramBot.Core.Commands;

public class CreateSettings : Command
{

    private readonly DataContext _context;
    private readonly StateMachine _stateMachine;

    public CreateSettings()
    {
        _context = new DataContext();
        _stateMachine = new StateMachine(_context);
    }

    public override string Name => "/createsettings";


    public override async void Execute(String command,Message message, ITelegramBotClient client,State state)
    {    
        
        var chatId = message.Chat.Id;

        var setting = await _context.SettingState
            .FindAsync(s => s.UserId == chatId)
            .Result.FirstOrDefaultAsync() ?? new SettingState();
        switch (state.StateCommand)
        {
            case "":
                await client.SendTextMessageAsync(chatId, $"Давайте введем настройки для {setting.SiteName}");
                state.StateCommand = "searchType";
                await client.SendTextMessageAsync(chatId, "Поиск по категории или по должности?" +
                                                          "\n 1. Категории" + "\n 2. Должности");
                break;
            case "searchType":
                switch (message.Text)
                {
                    case "1":
                        await client.SendTextMessageAsync(chatId, "Категория?" +
                                                                  "\n Пример: .NET | QA");
                        state.StateCommand = "post";
                        break;
                    case "2":
                    {
                        await client.SendTextMessageAsync(chatId, "Должность?" +
                                                                  "\n Пример: c# | java");
          
                        state.StateCommand = "job";
                        break;
                    }
                    default:
                        await client.SendTextMessageAsync(chatId, "Неверный параметр! Повторите попытку:");
                        state.StateCommand = "searchType";
                        break;
                }
                break;
            case "post":
                if (string.IsNullOrEmpty(message.Text))
                {
                    await client.SendTextMessageAsync(chatId,
                        "Вы ввели пустое поле! Пожалуйста,введите корректную должность и категорию:");
                }
                else
                {
                    setting.Category = message.Text;
                    setting.Job = "";
                    state.StateCommand = "city";
                    await client.SendTextMessageAsync(chatId,
                        "Город, где будете работать? (Ввводить на русском и с маленькой буквы) " +
                        "\n Пример: киев | днепропетровск");
                }
                break;
            case "job":
                if (string.IsNullOrEmpty(message.Text))
                {
                    await client.SendTextMessageAsync(chatId,
                        "Вы ввели пустое поле! Пожалуйста,введите корректную должность и категорию:");
                }
                else
                {
                    setting.Category = "";
                    setting.Job = message.Text;
                    state.StateCommand = "city";
                    await client.SendTextMessageAsync(chatId,
                        "Город, где будете работать? (Ввводить на русском и с маленькой буквы) " +
                        "\n Пример: киев | днепропетровск");
                }
                break;
            case "city":
                if (string.IsNullOrEmpty(message.Text))
                {
                    await client.SendTextMessageAsync(chatId,
                        "Вы ввели пустое поле! Пожалуйста,введите корректный город работы:");
                }
                else
                {
                    setting.City = message.Text;
                    UserSettings uSetting = new UserSettings()
                    {
                        UserId = setting.UserId,
                        SiteName = setting.SiteName,
                        Path = setting.Path,
                        Job = setting.Job,
                        Category = setting.Category,
                        City = setting.City,
                        XCard = setting.XCard,
                        XPublished = setting.XPublished
                
                    }; 
                    await client.SendTextMessageAsync(chatId,
                        "Скопируйте ссылку и проверьте на правильность данные. Если поиск правильный, то вам выдаст вакансии, которые вы ищете" +
                        $"\n Rabota.ua: {uSetting.Path}");

                    await _context.Settings.InsertOneAsync(uSetting);
                    await _context.SettingState.DeleteOneAsync(d => d.UserId == chatId);
                    await _stateMachine.DeleteState(chatId);
                }
                break;



        }

        await _context.SettingState.ReplaceOneAsync((s => s.UserId == chatId),setting);
        await _stateMachine.UpdateState(state);
    }
}