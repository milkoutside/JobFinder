using JobFinder.Data;
using JobFinder.Models;
using JobFinder.TelegramState.StateMachine;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace JobFinder.Commands;

public class CreateDouUa : Command
{
 private readonly DataContext _context;
 
    private readonly StateMachine _stateMachine;

    public CreateDouUa()
    {
        _context = new DataContext();
        
        _stateMachine = new StateMachine(_context);
    }
    public override string Name { get; } = "/createdou";
   public override async Task Execute(String command, Message message, ITelegramBotClient client, State state)
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
                
                await client.SendTextMessageAsync(chatId, "Поиск по категории или по должности?(указать цифру)" +
                                                          "\n 1. Категории" + "\n 2. Должности");
                break;
            case "searchType":
                switch (message.Text)
                {
                    case "1":
                        
                        await client.SendTextMessageAsync(chatId, "Категория?(указать цифру)" +
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
                        "Город, где будете работать? (Ввводить на укр и с большой буквы) " +
                        "\n Пример: Київ | Харків");
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
                        "Город, где будете работать? (Ввводить на укр и с большой буквы) " +
                        "\n Пример: Київ | Харків");
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
                        
                        City = setting.City,
                        
                        Job = setting.Job,
                        
                        Category = setting.Category,
                        
                        XCard = setting.XCard,
                        
                        XPublished = setting.XPublished
                
                    }; 
                    await client.SendTextMessageAsync(chatId,
                        "Скопируйте ссылку и проверьте на правильность данные. Если поиск правильный, то вам выдаст вакансии, которые вы ищете" +
                        $"\n Dou.ua: {uSetting.Path}");

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