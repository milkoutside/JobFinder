using TelegramBot;
using TelegramBot.Models;

namespace ConsoleApp2.SettingWritter;

public interface ISettingReader
{
    public Task<UserSettings> ReadSetting(List<ISetting> setting);
}