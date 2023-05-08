
namespace morskoyBoy {      
    internal class Program
    { 
        private static void Main(string[] args)
        {
            Console.Title = "МОРСКОЙ БОЙ";
            ConsoleMenu.showTitle("■");
            byte flag = 1;
            byte numberOfGames = 0;
            while (flag == 1) {
                Console.Clear();
                var game = new ConsoleGame();
                ConsoleMenu.startOfGame(game, numberOfGames);
                game.Fight();
                flag = ConsoleMenu.endOfGame(game);
                numberOfGames++;
            }
        }
    }
}
