using TelegramBot;
using TelegramBot.Core.TelegramState.StateMachine;
using TelegramBot.Models;

namespace ConsoleApp2.SettingWritter;

public interface ISettingWriter
{
    public Task<SettingState> SetSetting(ISetting setting, long user);
}