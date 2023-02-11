using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.TelegramState.StateMachine;

namespace TelegramBot.Core.Commands.Command;

public abstract class Command
{
    public abstract string Name { get; }

    public abstract void Execute(String command,Message message, ITelegramBotClient client,State state);
    public bool Equals(string command)
    {
        return command.Equals(this.Name);
    }

    public static List<Command> SetListCommands()
    {        // Write here all bot commands
        List<Command> list = new List<Command>();
      
        list.Add(new StartCommand()); 
        
        list.Add(new CreateSettings());
        
        list.Add(new SwitchCreateSettings());
      
     
     
     return list;
    }

}