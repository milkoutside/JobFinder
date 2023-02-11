using TelegramBot.Core.TelegramState.StateMachine;

namespace TelegramBot.Parser.Core;

public interface IParser
{
    public Task<List<string>> SearchVacancies(List<SettingState> settings);
    
}