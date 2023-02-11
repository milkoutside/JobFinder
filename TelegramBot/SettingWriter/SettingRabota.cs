using TelegramBot.Core.TelegramState.StateMachine;

namespace TelegramBot.SettingWriter;

public class SettingRabota : SettingFactory
{
    
    public override Task<SettingState> CreateWriter(ISetting setting, long id)
    {
        return new SettingWriter().SetSetting(setting, id);
    }

    public override Task<ISetting> CreateReader(List<ISetting> settings)
    {
        throw new NotImplementedException();
    }
}