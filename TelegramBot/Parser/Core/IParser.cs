using JobFinder.TelegramState.StateMachine;

namespace JobFinder.Parser.Core;

public interface IParser
{
    public Task<List<string>> SearchVacancies(List<SettingState> settings);
    
}