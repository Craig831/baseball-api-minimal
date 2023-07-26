using Api.Models;

namespace Api.Data
{
    public static class DbInitializer
    {
        //public static void Initialize(GameDb gameDb)
        public static void Initialize(WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetService<GameDb>();
            db.Database.EnsureCreated();

            if (!db.Games.Any()) 
            {
                List<Game> games = new List<Game>
                {
                    new Game {
                        Id =  1,
                        GameDateTime = new DateTime(2023, 7, 23, 22, 0, 0, 0),
                        HomeTeamId =  1,
                        AwayTeamId =  2,
                        HomeTeamScore =  0,
                        AwayTeamScore =  0,
                        IsFinal =  false
                      },
                    new Game {
                        Id =  2,
                        GameDateTime =  new DateTime(2023, 7, 23, 22, 0, 0, 0),
                        HomeTeamId =  3,
                        AwayTeamId =  4,
                        HomeTeamScore =  0,
                        AwayTeamScore =  0,
                        IsFinal =  false
                      },
                    new Game {
                        Id =  3,
                        GameDateTime =  new DateTime(2023, 7, 23, 22, 0, 0, 0),
                        HomeTeamId =  2,
                        AwayTeamId =  1,
                        HomeTeamScore =  0,
                        AwayTeamScore =  0,
                        IsFinal =  false
                      },
                    new Game {
                        Id =  4,
                        GameDateTime =  new DateTime(2023, 7, 23, 22, 0, 0, 0),
                        HomeTeamId =  4,
                        AwayTeamId =  3,
                        HomeTeamScore =  0,
                        AwayTeamScore =  0,
                        IsFinal =  false
                      },
                    new Game {
                        Id =  5,
                        GameDateTime =  new DateTime(2023, 7, 23, 22, 0, 0, 0),
                        HomeTeamId =  1,
                        AwayTeamId =  4,
                        HomeTeamScore =  0,
                        AwayTeamScore =  0,
                        IsFinal =  false
                      },
                    new Game {
                        Id =  6,
                        GameDateTime =  new DateTime(2023, 7, 23, 22, 0, 0, 0),
                        HomeTeamId =  2,
                        AwayTeamId =  3,
                        HomeTeamScore =  0,
                        AwayTeamScore =  0,
                        IsFinal =  false
                      },
                    new Game {
                        Id =  7,
                        GameDateTime =  new DateTime(2023, 7, 23, 22, 0, 0, 0),
                        HomeTeamId =  4,
                        AwayTeamId =  1,
                        HomeTeamScore =  0,
                        AwayTeamScore =  0,
                        IsFinal =  false
                      },
                    new Game {
                        Id =  8,
                        GameDateTime =  new DateTime(2023, 7, 23, 22, 0, 0, 0),
                        HomeTeamId =  3,
                        AwayTeamId =  2,
                        HomeTeamScore =  0,
                        AwayTeamScore =  0,
                        IsFinal =  false
                      }
                };
                foreach(Game gm in games)
                {
                    db.Add(gm);
                }
                db.SaveChanges();
            }
            
            if (!db.Teams.Any()) 
            {
                List<Team> teams = new List<Team>
                {
                    new Team {
                        Id = 1,
                        TeamName = "Team One",
                        Abbreviation = "ONE"
                    },
                    new Team {
                        Id = 2,
                        TeamName = "Team Two",
                        Abbreviation = "TWO"
                    },
                    new Team {
                        Id = 3,
                        TeamName = "Team Three",
                        Abbreviation = "THR"
                    },
                    new Team {
                        Id = 4,
                        TeamName = "Team Four",
                        Abbreviation = "FOR"
                    }
                };
                foreach (Team tm in teams)
                {
                    db.Add(tm);
                }
                db.SaveChanges();
            }
        }
    }
}
