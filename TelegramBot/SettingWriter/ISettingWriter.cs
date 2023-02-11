using TelegramBot.Core.TelegramState.StateMachine;

namespace TelegramBot.SettingWriter;

public interface ISettingWriter
{
    public Task<SettingState> SetSetting(ISetting setting, long user);
}