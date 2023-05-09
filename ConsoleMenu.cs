namespace morskoyBoy
{
    public static class ConsoleMenu {

        public static void showTitle(string element) {
            Console.Clear();
            MyExtensions.origRow = 2; 
            MyExtensions.origCol = 15; 
            int letterHeight = 8;
            int letterSpacing = 4;
            Console.ForegroundColor = ConsoleColor.White;
            MyExtensions.writeLetterM(letterHeight, element);

            MyExtensions.origCol += letterHeight + letterSpacing;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            MyExtensions.writeLetterO(letterHeight, element);
            Console.ForegroundColor = ConsoleColor.White;

            MyExtensions.origCol += letterHeight + letterSpacing;  
            MyExtensions.writeLetterR(letterHeight, element);

            MyExtensions.origCol += letterHeight + letterSpacing;
            MyExtensions.writeLetterS(letterHeight, element);

            MyExtensions.origCol += letterHeight + letterSpacing;  
            MyExtensions.writeLetterK(letterHeight, element);

            MyExtensions.origCol += letterHeight + letterSpacing;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            MyExtensions.writeLetterO(letterHeight, element);
            Console.ForegroundColor = ConsoleColor.White;

            MyExtensions.origCol += letterHeight + letterSpacing;
            MyExtensions.writeLetterY(letterHeight, element);

            MyExtensions.origRow = 12; 
            MyExtensions.origCol = 39; 

            MyExtensions.writeLetterB(letterHeight, element);
            
            MyExtensions.origCol += letterHeight + letterSpacing;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            MyExtensions.writeLetterO(letterHeight, element);
            Console.ForegroundColor = ConsoleColor.White;

            MyExtensions.origCol += letterHeight + letterSpacing;
            MyExtensions.writeLetterY(letterHeight, element);
            
            Console.Write("\n\n\n\n\tНажмите любую клавишу для продолжения... ");
            Console.ReadKey();    
        }

        public static void startOfGame(ConsoleGame game, byte isFirstGame) {
            Console.Clear();
            game.printFieldAndFlotilia(game.GamerField, game.GamerFlotilia, "ВАШЕ ПОЛЕ", "ВАША ФЛОТИЛИЯ");
            if (isFirstGame == 0) {
                Console.WriteLine("\n\tДобро пожаловать в классическую игру \"Морской бой\"!");
                Console.WriteLine("\tНа данном поле будет располагаться Ваша флотилия.");
            }
            else 
                Console.WriteLine("\n\tСыграем еще один раунд;)");
            Console.WriteLine("\tЕсли хотите заполнить поле сами, нажмите \"1\"");
            Console.WriteLine("\tДля автоматического рандомного заполнения поля, нажмите \"2\"");
            byte options;
            Console.Write("\t");
            while((!Byte.TryParse(Console.ReadLine(), out options)) || (options != 1 && options != 2)) { 
                Console.WriteLine("\tНеизвестная команда, попробуйте еще раз.");
                Console.Write("\t");
            }
            switch(options){
                case 1: 
                string alert;
                if (game.putAllGamerBoats(game.GamerField, out alert))
                    Console.WriteLine(alert);
                break;
                case 2:
                if(game.GamerFlotilia.putAllBoatsRandomly(game.GamerField)) {
                    Console.Clear();
                    game.printFieldAndFlotilia(game.GamerField, game.GamerFlotilia, "ВАШЕ ПОЛЕ", "ВАША ФЛОТИЛИЯ");
                    Console.WriteLine("\n\tВаши корабли установлены");
                }
                break;
                default:
                startOfGame(game, isFirstGame);
                break;
            }

            if (game.putAllComputerBoats()) {
                    Console.Write("\t.");
                    Thread.Sleep(900);
                    Console.Write(".");
                    Thread.Sleep(900);
                    Console.Write(".");
                    Thread.Sleep(900);
                    Console.WriteLine("Противник также установил все корабли, можно начинать БОЙ!");
                }

            Console.Write("\n\tНажмите любую клавишу для продолжения... ");
            Console.ReadKey();
                
        }

        public static byte endOfGame(ConsoleGame game) {
            byte options = 0;
            Console.WriteLine("\n\tЧтобы посмотреть статистику игры, нажмите \"1\"");
            while (options >= 0) {
            Console.WriteLine("\tЧтобы сыграть еще раз, нажмите \"2\"");
            Console.WriteLine("\tЧтобы выйти из игры, нажмите \"3\"");
            
            Console.Write("\t");
            if(Byte.TryParse(Console.ReadLine(), out options)) {
                switch(options){
                    case 1: 
                        game.PrintGameStatistics(); 
                        Console.Write("\n\tНажмите любую клавишу для продолжения... ");
                        Console.ReadKey();
                        Console.WriteLine();
                        break;
                    case 2: return 1;
                    case 3: return 0;
                    default:
                        Console.WriteLine("\tНеизвестная команда, попробуйте еще раз.");
                        options = endOfGame(game);
                        break;
                }
            }
            else {
                Console.WriteLine("\tНеизвестная команда, попробуйте еще раз.");
                options = endOfGame(game);
                break;        
            }
            }
            return options;      
        }
    }
}