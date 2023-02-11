using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TelegramBot.Core.TelegramState.StateMachine;

namespace TelegramBot.Parser.Core;

public class ParserDouUa : Parser
{
    public override async Task<List<string>> SearchVacancies(List<SettingState> settings)
    {
        IWebDriver _driver;
        List<string> result = new List<string>();
        for (int i = 0; i < settings.Count; i++)
        {


            _driver = new ChromeDriver();

            _driver.Url = settings[i].Path;
            for (int k = 0; k < 30; k++)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("window.scrollTo(0, scrollY+350)");
                Thread.Sleep(100);

            }
            
            List<IWebElement> elements = _driver.FindElements(By.XPath(settings[i].XCard)).ToList();
            for (int j = 0; j < elements.Count; j++)
            {
                result.Add(elements[i].GetAttribute("href"));
            }
            _driver.Close();
            _driver.Quit();


            return result;
        }

        return result;
    }
}