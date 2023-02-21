using JobFinder.Data;
using JobFinder.Models;
using JobFinder.Parser.Core;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace JobFinder.Parser;

public class ParserWorker : IParserWorker
{
    
    private  ParserCreator _parserCreator;

    private DataContext context;
    
    
    public ParserWorker(DataContext _context)
    {
        context = _context;
    }

    private async Task Work(Message message, ITelegramBotClient client,CancellationTokenSource cts)
    {
        _parserCreator = new RabotaCreator();

        var setting = await context.GetSettings.FindAsync(f => f.UserId == message.Chat.Id)
            .Result.ToListAsync();

        List<string> listVacancies = new List<string>(await _parserCreator.CreateParser().SearchVacancies(setting.Where(s => s.SiteName.Contains("Rabota")).ToList()));
        
        var pastVacancies = new List<string>(( await context.Vacancies.FindAsync(v=>v.UserId == message.Chat.Id)
            .Result.ToListAsync()??new List<Vacancies>()).Select(s=>s.Href)).ToList();
        
        var compareVacancies = await _parserCreator.Compare().CompareVacancies(listVacancies,pastVacancies);

        _parserCreator = new WorkCreator();
        
        listVacancies = new List<string>(await _parserCreator.CreateParser().SearchVacancies(setting.Where(s => s.SiteName.Contains("Work")).ToList()));
        
        compareVacancies.AddRange(listVacancies);
        
        _parserCreator = new DouCreator();
        
        listVacancies = new List<string>(await _parserCreator.CreateParser().SearchVacancies(setting.Where(s => s.SiteName.Contains("Dou")).ToList()));
        
        compareVacancies.AddRange(listVacancies);
       
        var dataVacancies = compareVacancies.Select(t => new Vacancies { Href = t, UserId = message.Chat.Id }).ToList();

        if (cts.IsCancellationRequested)
        {
            cts.Token.ThrowIfCancellationRequested();
            return;
        }
        

        if (compareVacancies.Count > 0)
        {
            await context.Vacancies.InsertManyAsync(dataVacancies);
            
            foreach (var v in compareVacancies)
            {
                await client.SendTextMessageAsync(message.Chat.Id, $"Новая вакансия по вашем критериям {v}");
            }
        }
        else
        {
            await client.SendTextMessageAsync(message.Chat.Id, "Новые вакансий не найдено! Продолжим поиски через 10 минут.");
        }
    }
    
    public async Task Start(Message message, ITelegramBotClient client,CancellationTokenSource cts)
    {
        await Work(message, client, cts);
    }
}