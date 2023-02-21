using JobFinder.Models;
using JobFinder.TelegramState.StateMachine;

namespace JobFinder.SettingWriter;

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