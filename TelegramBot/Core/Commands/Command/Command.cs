using StateMachineBot;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.TelegramState.StateMachine;

namespace TelegramBot.Core.Commands;

public abstract class Command
{
    public abstract string Name { get; }

    public abstract void Execute(String command,Message message, ITelegramBotClient client,State state);
    public bool Equals(string command)
    {
        return command.Equals(this.Name);
    }

    public static List<Command> SetListCommands()
    {
        List<Command> list = new List<Command>();
        // Write here all bot commands
        list.Add(new StartCommand()); 
        list.Add(new CreateSettings());
        list.Add(new SwitchCreateSettings());
      
     
     
     return list;
    }

}