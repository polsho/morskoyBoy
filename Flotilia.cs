namespace morskoyBoy {

    public class Flotilia {

        public Boat[][] Flot { get; set; }

        public int[] counters { get; set; }

        public Flotilia() {
            Flot = new Boat[4][];
            counters = new int[4];
            for (int i = 0; i < Flot.Length; i++) {
                Flot[i] = new Boat[i + 1];
                counters[i] = i + 1;
            }  
            for (int i = 0; i < Flot.GetLength(0); i++) {
                for (int j = 0; j < Flot[i].Length; j++) {
                    switch(i) {
                        case 0: Flot[i][j] = new Battleship(); break;
                        case 1: Flot[i][j] = new Cruiser();    break;
                        case 2: Flot[i][j] = new Destroyer();  break;
                        case 3: Flot[i][j] = new Powerboat();  break;
                    }
                }
            }    
        }

        public bool putAllBoats (Field field, boatPosition[][] p, out string message) {   // оставить на случай не консольной игры
            if (Flot.Length == p.Length) {
                for (int i = 0; i < Flot.Length; i++) {
                    for (int j = 0; j < Flot[i].Length; j++) {
                        if(!Flot[i][j].PutBoat(field, p[i][j], ref counters[i], out message)) {
                            return false;
                        }
                        else Flot[i][j].setSettledBoat(field, i, j);
                    }
                }
                message ="Все корабли флотилии установлены!";
                return true; 
            } 
            else {
                message = "Не хватает позиций для установки всех кораблей!";
                return false;
            }       
        }

        public bool putAllBoatsRandomly(Field field) {                      // Добавить исключение бесконечного цикла
            var randomPos = new boatPosition();
            string message; 
            int iterationsCount = 0;
            for (int f = 0; f < Flot.Length; f++) {
                for (int j = 0; j < Flot[f].Length; j++) 
                {
                    if (iterationsCount > 100) {
                        Console.WriteLine("РАНДОМНАЯ УСТАНОВКА КОРАБЛЕЙ ЗАСТРЯЛА");
                        putAllBoatsRandomly(field);
                    } else {
                        randomPos.setData(MyExtensions.rand.Next(0,10), MyExtensions.rand.Next(0,10), (boatDirection)MyExtensions.rand.Next(0,2));
                    if(!Flot[f][j].PutBoat(field, randomPos, ref counters[f], out message)) {
                        j--;
                        iterationsCount++;
                    }
                    else Flot[f][j].setSettledBoat(field, f, j);
                    }
                    
                }    
            }
            return true;            
        }

        public bool putAllBoatsTight(Field field) {                  
            var randomPos = new boatPosition();
            string message;
            int iterationsCount = 0;
            int[][] limits = new int[][]{new int[]{0,6}, new int[]{5,10}, new int[]{0,10}}; 
            int[] randomNum = limits[MyExtensions.rand.Next(0,3)];
            int[] randomLetter = (randomNum == limits[2]? limits[MyExtensions.rand.Next(0,2)]:limits[2]);
            int i = 0;
            for (; i < 3; i++) {
                for (int j = 0; j < Flot[i].Length; j++) {
                    if (iterationsCount > 100) {
                        Console.WriteLine("УСТАНОВКА КОРАБЛЕЙ КУЧКОЙ ЗАСТРЯЛА");
                        putAllBoatsTight(field);
                    } else {
                    randomPos.setData(MyExtensions.rand.Next(randomNum[0],randomNum[1]), 
                                      MyExtensions.rand.Next(randomLetter[0],randomLetter[1]), 
                                      (boatDirection)MyExtensions.rand.Next(0,2));
                    if(!Flot[i][j].PutBoat(field, randomPos, ref counters[i], out message)) {
                        j--;
                        iterationsCount++;
                    }
                    else Flot[i][j].setSettledBoat(field, i, j);
                    }
                }      
            }
            for (; i < Flot.Length; i++) {
                for (int j = 0; j < Flot[i].Length; j++) {
                    randomPos.setData(MyExtensions.rand.Next(0,10), MyExtensions.rand.Next(0,10), (boatDirection)MyExtensions.rand.Next(0,2));
                    if(!Flot[i][j].PutBoat(field, randomPos, ref counters[i], out message)) {
                        j--;
                    }
                    else Flot[i][j].setSettledBoat(field, i, j);
                }      
            }
            return true; 
        }

        public bool putAllBoatsAlongEdges(Field field) {                  
            var randomPos = new boatPosition();
            string message; 
            int iterationsCount = 0;
            int[][] limits = new int[][]{new int[]{0,1}, new int[]{9,10}, new int[]{0,10}}; 
            int numOfBoatTypes = MyExtensions.rand.Next(1,4);
            int i = 0;
            for (; i < numOfBoatTypes; i++) {
                for (int j = 0; j < Flot[i].Length; j++) {
                    if (iterationsCount > 100) {
                        Console.WriteLine("УСТАНОВКА КОРАБЛЕЙ ПО КРАЯМ ЗАСТРЯЛА");
                        putAllBoatsAlongEdges(field);
                    } else {
                    int[] randomNum = limits[MyExtensions.rand.Next(0,3)];
                    int[] randomLetter = (randomNum == limits[2]? limits[MyExtensions.rand.Next(0,2)]:limits[2]);
                    randomPos.setData(MyExtensions.rand.Next(randomNum[0],randomNum[1]), 
                                      MyExtensions.rand.Next(randomLetter[0],randomLetter[1]), 
                                      (boatDirection)MyExtensions.rand.Next(0,2));
                    if(!Flot[i][j].PutBoat(field, randomPos, ref counters[i], out message)) {
                        j--;
                        iterationsCount++;
                    }
                    else Flot[i][j].setSettledBoat(field, i, j);
                    }
                }      
            }
            for (; i < Flot.Length; i++) {
                for (int j = 0; j < Flot[i].Length; j++) {
                    randomPos.setData(MyExtensions.rand.Next(0,10), MyExtensions.rand.Next(0,10), (boatDirection)MyExtensions.rand.Next(0,2));
                    if(!Flot[i][j].PutBoat(field, randomPos, ref counters[i], out message)) {
                        j--;
                    }
                    else Flot[i][j].setSettledBoat(field, i, j);
                }      
            }
            return true; 
        }

        public string showBoatByStatus (int type, int indexNum, boatStatus status) {
            string boat = "         ";
            if (Flot[type][indexNum].Status == status) {
                boat = "";
                for (int i = 0; i < 4; i++)
                    boat += ((i < Flot[type][indexNum].Size? (char)cellStatus.Filled : " ") + " ");
            }
            return boat;
        }

        public void printFlotilia(int origX, int origY, boatStatus status, bool showNames, int[]? boat = null) {
            MyExtensions.origCol = origX;
            MyExtensions.origRow = origY;
            if (boat == null) boat = new int[]{-1,-1}; 

            int currentYPos = 0;
            for (int i = 0; i < Flot.Length; i++) {
                currentYPos += Flot[i].Length-1;
                int j = 0;
                for (; j < Flot[i].Length; j++) 
                {
                    MyExtensions.WriteAt("", 0, j + currentYPos);
                    if (showNames){
                        Console.Write((j+1) + " " + Flot[i][0].Name + " ");
                        MyExtensions.WriteAt("", 0 + 12, j + currentYPos);
                    }
                    if (i == boat[0] && j == boat[1]) {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write(showBoatByStatus(i,j, status));
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else Console.Write(showBoatByStatus(i,j, status));
                }   
            }
        } 

        public bool areAllBoatsSunk() {
            for (int i = 0; i < Flot.Length; i++){
                for (int j = 0; j < Flot[i].Length; j++) {
                    if (!(Flot[i][j].Status == boatStatus.Sunk))
                        return false;
                }
            }
            return true;
        }
    }
}