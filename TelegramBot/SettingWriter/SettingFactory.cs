using TelegramBot.Core.TelegramState.StateMachine;

namespace TelegramBot.SettingWriter;

public abstract class SettingFactory
{
    public abstract Task<SettingState> CreateWriter(ISetting setting, long user);

    public abstract Task<ISetting> CreateReader(List<ISetting> settings);

}