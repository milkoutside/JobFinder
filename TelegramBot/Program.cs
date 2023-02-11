
using TelegramBot.Core;

namespace TelegramBot
{
    internal class Program
    {


        static void Main(string[] args)
        {
            BotWorker botWorker = new BotWorker(); 
            botWorker.Start();
            Console.ReadLine();
        }
        
    }
}