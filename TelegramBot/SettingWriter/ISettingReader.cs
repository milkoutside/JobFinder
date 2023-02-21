using JobFinder.Models;

namespace JobFinder.SettingWriter;

public interface ISettingReader
{
    public Task<UserSettings> ReadSetting(List<ISetting> setting);
}