using JobFinder.Models;
using JobFinder.TelegramState.StateMachine;

namespace JobFinder.SettingWriter;

public interface ISettingWriter
{
    public Task<SettingState> SetSetting(ISetting setting, long user);
}