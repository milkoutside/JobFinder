using JobFinder.Models;

namespace JobFinder.SettingWriter;

public class SettingReader : ISettingReader
{
    public Task<UserSettings> ReadSetting(List<ISetting> setting)
    {
        return new Task<UserSettings>(null);
    }
}