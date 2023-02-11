using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TelegramBot.Core.TelegramState.StateMachine;
using TelegramBot.Models;

namespace TelegramBot.Parser;

public abstract class Parser : IParser
{
    private IWebDriver _driver;

    public abstract Task<List<string>> SearchVacancies(List<SettingState> settings);
}