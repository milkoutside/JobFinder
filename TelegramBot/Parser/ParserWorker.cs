using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Data;
using TelegramBot.Models;
using TelegramBot.Parser.Core;
using TelegramBot.SettingWriter;

namespace TelegramBot.Parser;

public class ParserWorker : IParserWorker
{
    private readonly DataContext _context;

    private readonly SettingFactory _settingFactory = new SettingRabota();

    private  ParserCreator _parserCreator;
    
    public ParserWorker()
    {
        _context = new DataContext();
   
    }

    public async Task Work(Message message, ITelegramBotClient client,long id)
    {
        _parserCreator = new RabotaCreator();

        var rabotaSettings = await _context.GetSettings.FindAsync((f => f.UserId == id)).Result.ToListAsync();

        var rabotaData = await _parserCreator.CreateParser().SearchVacancies(rabotaSettings);
        
        var pastVacancies = new List<string>(( await _context.Vacancies.FindAsync(v=>v.UserId == message.Chat.Id)
            .Result.ToListAsync()??new List<Vacancies>()).Select(s=>s.Href)).ToList();
        
        var listVacancies = await _parserCreator.Compare().CompareVacancies(rabotaData,pastVacancies);
        
        var dataVacancies = new List<Vacancies>();
        
        for (int i = 0; i < listVacancies.Count; i++)
        {
            var v = new Vacancies();
            
            v.Href = listVacancies[i];
            
            v.UserId = id;
            
            dataVacancies.Add(v);
        }
        if (listVacancies.Count > 0)
        {
            await _context.Vacancies.InsertManyAsync(dataVacancies);
        }

        if (listVacancies.Count > 0)
        {
            foreach (var v in listVacancies)
            {
                await client.SendTextMessageAsync(message.Chat.Id, $"Новая вакансия по вашем критериям {v}");
            }
        }
        else
        {
            await client.SendTextMessageAsync(message.Chat.Id, "Новые вакансий не найдено! Продолжим поиски через 10 минут.");
        }

    

      
    }
    
    public Task Start()
    {
        throw new NotImplementedException();
    }
}