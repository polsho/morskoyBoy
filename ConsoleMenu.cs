namespace morskoyBoy
{
    public static class ConsoleMenu {

        public static void showTitle(string element) {
            Console.Clear();
            MyExtensions.origRow = 2; 
            MyExtensions.origCol = 10; 
            int letterHeight = 8;
            int letterSpacing = 4;


            for (int i = 0; i < letterHeight; i++) {
                MyExtensions.WriteAt(element, 0, i);
                if (i <= letterHeight/2) {
                    MyExtensions.WriteAt(element, i, i);
                    MyExtensions.WriteAt(element, letterHeight - i, i);
                }
                MyExtensions.WriteAt(element, letterHeight, i);
            }

            MyExtensions.origCol += letterHeight + letterSpacing;
            for (int i = 0; i < letterHeight; i++) {
                if (i == 0 || i == 7) {
                    MyExtensions.WriteAt(element, 3, i);
                    MyExtensions.WriteAt(element, 5, i);
                }
                if (i == 1 || i == 6) {
                    MyExtensions.WriteAt(element, 1, i);
                    MyExtensions.WriteAt(element, 7, i);
                }
                if (i >= letterHeight/4 && i < letterHeight/4 + letterHeight/2) {
                    MyExtensions.WriteAt(element, 0, i);
                    MyExtensions.WriteAt(element, letterHeight, i);
                }
            }

            MyExtensions.origCol += letterHeight + letterSpacing;  
            for (int i = 0; i < letterHeight; i++) {
                if (i == 0 || i == 4) {
                    for (int j = 0; j < letterHeight ; j+=2)
                        MyExtensions.WriteAt(element, j, i);
                }
                if (i == 1 || i == 3) {
                    MyExtensions.WriteAt(element, 0, i);
                    MyExtensions.WriteAt(element, 7, i);
                }
                if (i == 2) {
                    MyExtensions.WriteAt(element, 0, i);
                    MyExtensions.WriteAt(element, 8, i);
                }
                else MyExtensions.WriteAt(element, 0, i);
            }

            MyExtensions.origCol += letterHeight + letterSpacing;
            for (int i = 0; i < letterHeight; i++) {
                if (i == 0 || i == 7) {
                    MyExtensions.WriteAt(element, 3, i);
                    MyExtensions.WriteAt(element, 5, i);
                }
                if (i == 1 || i == 6) {
                    MyExtensions.WriteAt(element, 1, i);
                    MyExtensions.WriteAt(element, 7, i);
                }
                if (i >= letterHeight/4 && i < letterHeight/4 + letterHeight/2) {
                    MyExtensions.WriteAt(element, 0, i);
                    //MyExtensions.WriteAt(element, letterHeight, i);
                }
            }

            Console.WriteLine("\n\n\tНажмите любую клавишу для продолжения... ");
            Console.Write("\t");
            Console.ReadKey();    
        }

        public static void startOfGame(ConsoleGame game) {
            Console.Clear();
            game.printFieldAndFlotilia(game.GamerField, game.GamerFlotilia, "ВАШЕ ПОЛЕ", "ВАША ФЛОТИЛИЯ");
            Console.WriteLine("\n\tДобро пожаловать в классическую игру \"Морской бой\"!");
            Console.WriteLine("\tНа данном поле будет располагаться Ваша флотилия.");
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
                startOfGame(game);
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
            Console.WriteLine("\n\tЧтобы посмотреть статистику игры, нажмите \"1\"");
            Console.WriteLine("\tЧтобы сыграть еще раз, нажмите \"2\"");
            Console.WriteLine("\tЧтобы выйти из игры, нажмите \"3\"");
            byte options;
            Console.Write("\t");
            if(Byte.TryParse(Console.ReadLine(), out options)) {
                switch(options){
                    case 1: 
                        game.PrintGameStatistics(); 
                        Console.WriteLine("\n\n\tНажмите любую клавишу для продолжения... ");
                        Console.Write("\t");
                        Console.ReadKey();
                        options = endOfGame(game); 
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
            }
            return options;      
        }
    }
}