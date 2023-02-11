using TelegramBot.Core.TelegramState.StateMachine;
using TelegramBot.Models;

namespace TelegramBot.Parser;

public interface IParser
{
    public Task<List<string>> SearchVacancies(List<SettingState> settings);
    
}