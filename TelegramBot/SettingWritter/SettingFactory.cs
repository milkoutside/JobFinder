using TelegramBot;
using TelegramBot.Core.TelegramState.StateMachine;
using TelegramBot.Models;

namespace ConsoleApp2.SettingWritter;

public abstract class SettingFactory
{
    public abstract Task<SettingState> CreateWriter(ISetting setting, long user);

    public abstract Task<ISetting> CreateReader(List<ISetting> settings);

}