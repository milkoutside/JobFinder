using Telegram.Bot;

namespace JobFinder.Services;

public class TelegramClient
{
    private TelegramBotClient _botClient;
 


    public async Task<TelegramBotClient> GetBot()
    {
        if (_botClient != null)
        {
            return _botClient;
        }
            
        _botClient = new TelegramBotClient("6229861807:AAFyVSpiz4QrRDp5ELgws1ZCfEV99jcPi7I");

        var hook = "https://aa91-94-45-110-42.eu.ngrok.io/api/message/update";
        await _botClient.SetWebhookAsync(hook);

        return _botClient;
    }
}