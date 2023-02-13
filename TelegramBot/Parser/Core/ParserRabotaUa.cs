using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TelegramBot.Core.TelegramState.StateMachine;

namespace TelegramBot.Parser.Core;

public class ParserRabotaUa : Parser
{
    public override async Task<List<string>> SearchVacancies(List<SettingState> settings)
    {     
        List<string> result = new List<string>();
        
        IWebDriver _driver;
        
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

            List<string> timePublished = new List<string>() { "1 день назад", "2 дня назад", "3 дня назад","1 час назад"  };
            
            for (int j = 1; j < 24; j++)
            {
                switch (j % 10)
                {
                    case var num when j > 5 && j < 21:
                        
                        timePublished.Add($"{j} часов назад" );
                        
                        break;
                    
                    case 1:
                        
                        timePublished.Add($"{j} час назад" );
                        
                        break;
                    
                    case 2:
                    case 3:
                    case 4:
                        
                        timePublished.Add($"{j} часа назад" );
                        
                        break;
                    
                    default:
                        
                        timePublished.Add($"{j} часов назад" );
                        
                        break;
                }
      
            }
            List<IWebElement> cards = _driver.FindElements(By.XPath(settings[i].XCard)).ToList();
            
            List<IWebElement> published = _driver.FindElements(By.XPath(settings[i].XPublished)).ToList();
            
            List<IWebElement> hotCards = _driver.FindElements(By.XPath("//alliance-jobseeker-desktop-vacancies-list//alliance-vac-list-status-label")).ToList();
            
            for (int j =  0; j < cards.Count - hotCards.Count; j++)
            {
                
                if (timePublished.Contains(published[j].Text))
                {
                    result.Add(cards[j + hotCards.Count].GetAttribute("href"));
                }

               


                
            }
            _driver.Close();
            
            _driver.Quit();
          
        }
        
        return result;
    }
}