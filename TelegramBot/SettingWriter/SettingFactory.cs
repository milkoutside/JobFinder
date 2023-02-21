using JobFinder.Models;
using JobFinder.TelegramState.StateMachine;

namespace JobFinder.SettingWriter;

public abstract class SettingFactory
{
    public abstract Task<SettingState> CreateWriter(ISetting setting, long user);

    public abstract Task<ISetting> CreateReader(List<ISetting> settings);

}