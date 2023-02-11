using TelegramBot;
using TelegramBot.Models;

namespace ConsoleApp2.SettingWritter;

public class SettingReader : ISettingReader
{
    public Task<UserSettings> ReadSetting(List<ISetting> setting)
    {
        return new Task<UserSettings>(null);
    }
}