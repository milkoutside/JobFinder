using TelegramBot;
using TelegramBot.Core.TelegramState.StateMachine;
using TelegramBot.Models;

namespace ConsoleApp2.SettingWritter;

public class SettingWriter : ISettingWriter
{
    public  async Task<SettingState> SetSetting(ISetting setting,long user)
    {

        SettingState userSettings = new SettingState();
        userSettings.UserId = user;
        userSettings.SiteName = setting.SiteName;
        userSettings.Path = setting.Path;
        userSettings.XCard = setting.XCard;
        userSettings.XPublished = setting.XPublished;
        return userSettings;

    }
}