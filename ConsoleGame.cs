using System.Text.RegularExpressions;

namespace morskoyBoy{

    
    public partial class ConsoleGame {

        public Flotilia GamerFlotilia { get; set; }
        public Flotilia ComputerFlotilia { get; set; }

        public Field GamerField { get; set; }
        public Field ComputerField { get; set; }

        public ConsoleGame() {
            GamerFlotilia = new Flotilia();
            ComputerFlotilia = new Flotilia();
            GamerField = new Field();
            ComputerField = new Field();
        } 

        static public void getDataForBoatPosition(string name, int size, int index, out string coord, out string pos){
            if (name == "Линкор")
                Console.Write($"\n\tВведите данные для установки корабля типа {name} ");
            else 
                Console.Write($"\n\tВведите данные для установки {index+1}-го корабля типа {name} ");
            if (size == 1) 
                Console.WriteLine($"({size} ячейка).");
            else 
                Console.WriteLine($"({size} ячейки).");
            Console.WriteLine("\tКоординаты первой ячейки (формат А1 или а1):");
            Console.Write("\t");
            while (String.IsNullOrEmpty(coord = Console.ReadLine()!)) {
                MyExtensions.WriteInRed("\tКоординаты не были введены!!");
                Console.WriteLine("\tВведите координаты первой ячейки еще раз (формат А1 или а1):");
                Console.Write("\t");
            }
            if (size > 1) {
                Console.WriteLine("\tПоложение корабля (В или в - вертикально вниз, Г или г - горизонтально вправо):");
                Console.Write("\t");
                while (String.IsNullOrEmpty(pos = Console.ReadLine()!)) {
                    MyExtensions.WriteInRed("\tПоложение корабля не было указано!!");
                    Console.WriteLine("\tУкажите положение корабля еще раз (В или в - вертикально вниз, Г или г - горизонтально вправо):");
                    Console.Write("\t");
                }
            }
            else pos = "В";
        }

        static public bool convertDataToCoord(string coordinates, out int num, out int letter, out string message) {
            string pattern = "^([А-Иа-и]|К|к)([1-9]|10)$";
            if (Regex.IsMatch(coordinates, pattern)) {

                if(char.IsUpper(coordinates[0])) {
                    if (coordinates[0] == 'К')
                        letter = (int)(coordinates[0]) - 1041;
                    else letter = (int)(coordinates[0]) - 1040;
                }   
                else {
                    if(coordinates[0] == 'к')
                        letter = (int)(coordinates[0]) - 1073;
                    else letter = (int)(coordinates[0]) - 1072;
                }
               
               num = Int32.Parse(coordinates[1..]) - 1;
           
                message = "\tКоординаты установлены!";
                return true;
            }
            else {
                num = 0;
                letter = 0;
                message = "\tНеверный формат координат ячейки! Попробуйте еще раз:";
                return false;
            }  
        }

        static public string convertCoordToData(int num, int letter) {
            if (num == -1 || letter == -1) {
                return "?-?";
            } else {
                int add = letter == 9? 1041 : 1040; 
                return Convert.ToChar(letter + add) + "-" + (num + 1);
            }
            
        }   

        static public bool getBoatPosition(string coordinates, string direction, out boatPosition boatPos,  out string message) {
            var bP = new boatPosition();
            int num, letter;
            if (convertDataToCoord(coordinates, out num, out letter, out message)) {
                
                bP.CoordNum = num;
                bP.CoordLetter = letter;

                if (direction == "В" || direction == "в") bP.Direction = boatDirection.Vert;
                else if (direction == "Г" || direction == "г") bP.Direction = boatDirection.Horiz;
                else {
                    boatPos = bP;
                    message = "\tНеверный формат положения корабля!";
                    return false;
                }

                boatPos = bP;
                return true;
            }
            else {
                boatPos = bP;
                return false;
            }   
        }

        public bool putAllGamerBoats(Field field, out string message) {
            Console.Clear();
            printFieldAndFlotilia(GamerField, GamerFlotilia, "ВАШЕ ПОЛЕ", "ВАША ФЛОТИЛИЯ");
            //message ="\tУстановите Ваши корабли на поле боя так, чтобы они не касались друг друга ни сторонами, ни углами.";
            message = "";
            Console.WriteLine("\n\tУстановите Ваши корабли на поле боя так, чтобы они не касались друг друга ни сторонами, ни углами.");
            Console.WriteLine("\tСначала определите координаты первой ячейки, от которой корабль будет располагаться либо вправо, либо вниз.");
            Console.WriteLine("\tДалее определите направление.");
            var posData = new boatPosition();
            string coord, pos;

            for (int i = 0; i < GamerFlotilia.Flot.Length; i++) {
                    for (int j = 0; j < GamerFlotilia.Flot[i].Length; j++) 
                    {
                        do {
                            MyExtensions.WriteInGreen("\n" + message);
                            do {
                                getDataForBoatPosition(GamerFlotilia.Flot[i][j].Name, 
                                                    GamerFlotilia.Flot[i][j].Size,
                                                    j, out coord, out pos);
                                if (!getBoatPosition(coord, pos, out posData, out message)) {
                                    MyExtensions.WriteInRed(message);
                                    continue;
                                }
                                else break;
                            } while (true);
                        } while (!GamerFlotilia.Flot[i][j].PutBoat(field, posData, ref GamerFlotilia.counters[i], out message));
                        GamerFlotilia.Flot[i][j].setSettledBoat(field, i, j);
                        Console.Clear();
                        printFieldAndFlotilia(GamerField, GamerFlotilia, "ВАШЕ ПОЛЕ", "ВАША ФЛОТИЛИЯ");
                    }
                }
            message = "\n\tВсе корабли флотилии установлены!";
            return true;
        }

        public bool putAllComputerBoats() {
            int settlementOption = MyExtensions.rand.Next(0,3);
            switch (settlementOption) {
                case 0:
                    return ComputerFlotilia.putAllBoatsRandomly(ComputerField);
                case 1:
                    return ComputerFlotilia.putAllBoatsTight(ComputerField); 
                case 2:
                    return ComputerFlotilia.putAllBoatsAlongEdges(ComputerField);
                default:
                    return false;
            }
        }

        public void printFieldAndFlotilia(Field field, Flotilia flotilia, string title1, string title2) {
            MyExtensions.origCol = 0;
            MyExtensions.origRow = 0; 
            MyExtensions.WriteAt(title1, 25, 1);
            field.PrintField(16, 3, flotilia, false);

            MyExtensions.origCol = 0;
            MyExtensions.origRow = 0; 
            int shift = 47;
            MyExtensions.WriteAt(title2, shift+3, 1);
            flotilia.printFlotilia(shift, 5, boatStatus.OK, true);

            Console.WriteLine("\n");
        }
        public void printBothFields() {
            MyExtensions.origCol = 0;
            MyExtensions.origRow = 0; 
            MyExtensions.WriteAt("ВАШЕ ПОЛЕ", 19, 1);
            GamerField.PrintField(10, 3, GamerFlotilia, false);
            MyExtensions.origCol = 0;
            MyExtensions.origRow = 0; 
            GamerFlotilia.printFlotilia(38, 5, boatStatus.Sunk, false, getPrevStroke(compStrokes).HittedBoat);

            MyExtensions.origCol = 0;
            MyExtensions.origRow = 0; 
            MyExtensions.WriteAt("ПОЛЕ ПРОТИВНИКА", 57, 1);
            ComputerField.PrintField(50, 3, ComputerFlotilia, true);
            MyExtensions.origCol = 0;
            MyExtensions.origRow = 0; 
            ComputerFlotilia.printFlotilia(80, 5, boatStatus.Sunk, false, getPrevStroke(gamerStrokes).HittedBoat);

            MyExtensions.origCol = 0;
            MyExtensions.origRow = 0; 
            MyExtensions.WriteAt("Предыдущий удар: " 
                              + convertCoordToData(getPrevStroke(compStrokes).num,getPrevStroke(compStrokes).letter) 
                              + " " + getPrevStroke(compStrokes).showStrokeResult(GamerField,GamerFlotilia), 14, 17);

            MyExtensions.WriteAt("Предыдущий удар: " 
                              + convertCoordToData(getPrevStroke(gamerStrokes).num,getPrevStroke(gamerStrokes).letter)
                              + " " + getPrevStroke(gamerStrokes).showStrokeResult(ComputerField,ComputerFlotilia), 55, 17);
            
            Console.WriteLine("\n\t   " + " Количество ударов: " + (compStrokes.Count-1) 
                              + "\t\t\t" + " Количество ударов: " + (gamerStrokes.Count-1));                  
            Console.WriteLine("\n");
        } 

        public void PrintGameStatistics() {
            MyExtensions.origRow = Console.CursorTop;
            MyExtensions.origCol = Console.CursorLeft;
            MyExtensions.WriteAt("Статистика игры", 24, 0);
            MyExtensions.WriteAt(MyExtensions.writeHorizLine(64), 8, 1);
            var colsnames = new String[]{"Показатель", "Вы", "Противник"};
            var rowsnames = new String[]{"Количество попаданий", "Коэфициент попаданий, в %", "Неисследованное поля соперника, в %", 
                                         "Количество ударов", "Продолжительность боя, в минутах:"};
            var gamerData = new double[]{HittedStrokeCount(gamerStrokes),
                                         Math.Round( (double)HittedStrokeCount(gamerStrokes)/(double)(gamerStrokes.Count)*100, 2 ),
                                         ComputerField.noHittedCellsCount()};
            var compData = new double[]{HittedStrokeCount(compStrokes),
                                         Math.Round( (double)HittedStrokeCount(compStrokes)/(double)(compStrokes.Count)*100, 2 ),
                                         GamerField.noHittedCellsCount(), compStrokes.Count-1, ConsoleGame.gameTimer};
            int topcursor = 2;
            for (int i = 0; i <= rowsnames.Count(); i++) {
                if (i == 0) {
                    for (int j = 0; j < colsnames.Count(); j++) {
                        if (j == 0) MyExtensions.WriteAt(colsnames[j], 8, i+topcursor);
                        else MyExtensions.WriteAt(colsnames[j], 8 + 13*(j+2), i+topcursor);
                    }
                    MyExtensions.WriteAt(MyExtensions.writeHorizLine(64), 8, 3);
                    topcursor = 3;
                }
                else if (i > 0 && i < rowsnames.Count()-1) {
                    MyExtensions.WriteAt(rowsnames[i-1], 8, i+topcursor); 
                    MyExtensions.WriteAt(Convert.ToString(gamerData[i-1]), 47, i+topcursor);
                    MyExtensions.WriteAt(Convert.ToString(compData[i-1]), 60, i+topcursor);
                    if (i == rowsnames.Count()-2)   
                        MyExtensions.WriteAt(MyExtensions.writeHorizLine(64), 8, 7);
                } 
                else  {
                    MyExtensions.WriteAt(rowsnames[i-1], 8, i+1+topcursor);
                    MyExtensions.WriteAt(Convert.ToString(compData[i-1]), 50, i+1+topcursor);
                }  
                MyExtensions.WriteAt(MyExtensions.writeHorizLine(64), 8, 10);
            }
        }
    }
}


