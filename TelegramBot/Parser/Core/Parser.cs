using JobFinder.TelegramState.StateMachine;
using OpenQA.Selenium;

namespace JobFinder.Parser.Core;

public abstract class Parser : IParser
{
    private IWebDriver _driver;

    public abstract Task<List<string>> SearchVacancies(List<SettingState> settings);
}