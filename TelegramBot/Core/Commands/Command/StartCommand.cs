using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.TelegramState.StateMachine;
using TelegramBot.Data;
using TelegramBot.Parser;

namespace TelegramBot.Core.Commands.Command;

public class StartCommand : Command
{
    public override string Name => "/start";

    private readonly DataContext _context;

    private ParserWorker _parserWorker;

    private readonly StateMachine _stateMachine;


    public StartCommand()
    {
        _context = new DataContext();
        _stateMachine = new StateMachine(_context);
    }

    public async override void Execute(String command, Message message, ITelegramBotClient client, State state)
    {
        bool isWork = true;


        _parserWorker = new ParserWorker();
        state.ActionCommand = "/start";
        await _stateMachine.UpdateState(state);

        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken ct = cts.Token;
        if (message.Text == "/stop")
        {
            
            cts.Cancel();
            cts.Dispose();
            await client.SendTextMessageAsync(message.Chat.Id, "end");
        }

        await Task.Run(async () =>
        {
            if (ct.IsCancellationRequested == false)
            {
                while (true)
                {


                    if (ct.IsCancellationRequested)
                        break;

                    await _parserWorker.Work(message, client, message.Chat.Id);
                    Thread.Sleep(60000);

                }
            }

            Console.WriteLine("end");
        },ct);
      


    }
}








