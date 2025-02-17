using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace FinalProject {
    public class Game {
        public int goalNumber;
        public int range;
        public int guessCount;
        public int maxGuessCount;
        private bool gameRunning = true;
        public int winStreak = 0;
        public Game(int r, int g) {
            guessCount = g;
            maxGuessCount = g;
            range = r;
            Random rand = new Random();
            goalNumber = rand.Next(1,range + 1);
            gameRunning = true;
        }

        /// <summary>
        /// Handles a guess submitted in the game.
        /// </summary>
        /// <param name="browserStorage">Local Storage</param>
        /// <param name="guess">The player's guess</param>
        /// <param name="mode">The game mode</param>
        /// <returns>Whether the guess is too high or too low.</returns>
        public string SubmitGuess(ProtectedLocalStorage browserStorage, int guess, string mode) {
            if (gameRunning) {
                guessCount--;
                if (guess != goalNumber && guessCount == 0) {
                    gameRunning = false;
                    SubmitScoreAsync(browserStorage, winStreak, mode);
                    winStreak = 0;
                    return $"Too bad... The number I was thinking of was {goalNumber}";
                }
                if (guess < goalNumber) {
                    return "That's too low!";
                }
                if (guess > goalNumber) {
                    return "That's too high!";
                }
                if (guess == goalNumber) {
                    gameRunning = false;
                    winStreak++;
                    return $"Congratulations! You've guessed the number! I was thinking of {goalNumber}.";
                }
            }
            return "";
        }

        /// <summary>
        /// Resets the game object's variables for a new game
        /// </summary>
        /// <param name="browserStorage">Local Storage</param>
        /// <param name="mode">The game mode</param>
        public void ResetGame(ProtectedLocalStorage browserStorage, string mode) {
            if (gameRunning) {
                    SubmitScoreAsync(browserStorage, winStreak, mode);
                    winStreak = 0;}
            Random rand = new Random();
            goalNumber = rand.Next(1,range + 1);
            guessCount = maxGuessCount - (int)Math.Sqrt(winStreak);
            gameRunning = true;
        }

        public async Task SubmitScoreAsync(ProtectedLocalStorage scoring, int i, string mode) {
            var result = await scoring.GetAsync<Score>(mode);
            if (result.Success) {
                result.Value.scores.Add(DateTime.Now, i);
                await scoring.SetAsync(mode, result.Value);
            }
            else {
                Score score = new Score();
                score.scores.Add(DateTime.Now, i);
                await scoring.SetAsync(mode, score);
            }
        }
    }
}