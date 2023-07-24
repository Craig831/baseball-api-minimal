namespace Api.Models
{
    public class ScoreUpdate
    {
        public int Id { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public bool IsFinal { get; set; }
    }
}
