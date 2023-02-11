using OpenQA.Selenium;
using TelegramBot.Core.TelegramState.StateMachine;

namespace TelegramBot.Parser.Core;

public abstract class Parser : IParser
{
    private IWebDriver _driver;

    public abstract Task<List<string>> SearchVacancies(List<SettingState> settings);
}