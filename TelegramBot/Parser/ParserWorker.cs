using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.TelegramState;
using TelegramBot.Core.TelegramState.StateMachine;
using TelegramBot.Data;
using TelegramBot.Models;
using TelegramBot.Parser.Core;
using TelegramBot.SettingWriter;

namespace TelegramBot.Parser;

public class ParserWorker : IParserWorker
{
    
    private  ParserCreator _parserCreator;

    private DataContext context;
    
    
    public ParserWorker(DataContext _context)
    {
        context = _context;
    }

    public async Task Work(Message message, ITelegramBotClient client,CancellationTokenSource cts)
    {
        _parserCreator = new RabotaCreator();

        var setting = await context.GetSettings.FindAsync(f => f.UserId == message.Chat.Id)
            .Result.ToListAsync();

        var rabotaData = await _parserCreator.CreateParser().SearchVacancies(setting);
        
        var pastVacancies = new List<string>(( await context.Vacancies.FindAsync(v=>v.UserId == message.Chat.Id)
            .Result.ToListAsync()??new List<Vacancies>()).Select(s=>s.Href)).ToList();
        
        var listVacancies = await _parserCreator.Compare().CompareVacancies(rabotaData,pastVacancies);
        
        var dataVacancies = new List<Vacancies>();
        
        for (int i = 0; i < listVacancies.Count; i++)
        {
            var v = new Vacancies();
            
            v.Href = listVacancies[i];
            
            v.UserId = message.Chat.Id;
            
            dataVacancies.Add(v);
        }

        if (cts.IsCancellationRequested)
        {
            cts.Token.ThrowIfCancellationRequested();
            return;
        }
        

        if (listVacancies.Count > 0)
        {
            await context.Vacancies.InsertManyAsync(dataVacancies);
            
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
    
    public async Task Start(Message message, ITelegramBotClient client,CancellationTokenSource cts)
    {
        await Work(message, client, cts);
    }
}