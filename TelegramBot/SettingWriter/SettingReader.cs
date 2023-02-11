using TelegramBot.Models;

namespace TelegramBot.SettingWriter;

public class SettingReader : ISettingReader
{
    public Task<UserSettings> ReadSetting(List<ISetting> setting)
    {
        return new Task<UserSettings>(null);
    }
}