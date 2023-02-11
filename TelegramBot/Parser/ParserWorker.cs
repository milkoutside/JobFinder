using ConsoleApp2.SettingWritter;
using JobFinder;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Data;

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
        if (listVacancies != null)
        {
            await _context.Vacancies.InsertManyAsync(dataVacancies);
        }

        foreach (var VARIABLE in listVacancies)
        {
            await client.SendTextMessageAsync(message.Chat.Id, $"Новая вакансия по вашем критериям {VARIABLE}");
        }
    }
    
    public Task Start()
    {
        throw new NotImplementedException();
    }
}