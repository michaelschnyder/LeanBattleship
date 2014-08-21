namespace LeanBattleship.Web.Controllers
{
    public class MyMatchesDto
    {
        public long TournamentId { get; set; }
        public long MatchId { get; set; }
        public string MatchState { get; set; }
        public string WaitingFor { get; set; }
    }
}