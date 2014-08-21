namespace LeanBattleship.Model
{
    public class Round
    {
        public long Id { get; set; }

        public string PlayerOneCell { get; set; }
        
        public string PlayerTwoCell { get; set; }

        public string PlayerOneVersion { get; set; }

        public string PlayerTwoVersion { get; set; }

        public string ServerVersion { get; set; }
    }
}
