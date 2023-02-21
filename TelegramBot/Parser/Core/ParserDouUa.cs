using System.Globalization;
using JobFinder.TelegramState.StateMachine;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace JobFinder.Parser.Core;

public class ParserDouUa : Parser
{
    public override async Task<List<string>> SearchVacancies(List<SettingState> settings)
    {
         List<string> result = new List<string>();
                
         List<string> timePublished = new List<string>()
         {
                  
             DateTime.Now.ToString("d MMMM yyyy", new CultureInfo("uk-UA")),
             DateTime.Now.AddDays(-1).ToString("d MMMM yyyy", new CultureInfo("uk-UA")),
             DateTime.Now.AddDays(-2).ToString("d MMMM yyyy", new CultureInfo("uk-UA")),
         };

        IWebDriver _driver = new ChromeDriver();
        
        
        for (int i = 0; i < settings.Count; i++)
        {
            
            _driver.Url = settings[i].Path;
            
            IWebElement moreButton;
            for (int k = 0; k < 30; k++)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                
                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)"); 
                moreButton = _driver.FindElement(By.XPath("//div[contains(@class,'more-btn')]/a"));
                if(moreButton.Text != "")
                    moreButton?.Click();
                Thread.Sleep(100);

            }
            
            List<IWebElement> cards = _driver.FindElements(By.XPath(settings[i].XCard)).ToList();
            
            List<IWebElement> published = _driver.FindElements(By.XPath(settings[i].XPublished)).ToList();
            
            List<IWebElement> hotCards = _driver.FindElements(By.XPath("//li[contains(@class,'l-vacancy __hot')]")).ToList();
            
            for (int j =  0; j < cards.Count - hotCards.Count; j++)
            {
                
                if (timePublished.Contains(published[j].Text))
                {
                    result.Add(cards[j + hotCards.Count].GetAttribute("href"));
                }
                
            }

            
     
        }
        _driver.Quit();
        _driver.Dispose();
        return result;
    }
}