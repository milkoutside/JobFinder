namespace JobFinder.TelegramState.StateMachine
{
    public interface IState
    {
        public long Id { get; set; }
        
        public string ActionCommand { get; set; }
        
        public string StateCommand { get; set; }
        public string Message { get; set; }
       
    }
}