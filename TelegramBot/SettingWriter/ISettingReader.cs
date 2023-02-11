using TelegramBot.Models;

namespace TelegramBot.SettingWriter;

public interface ISettingReader
{
    public Task<UserSettings> ReadSetting(List<ISetting> setting);
}