using System.Diagnostics;
using System.Linq;

namespace morskoyBoy
{

    public partial class ConsoleGame {

        public class Stroke {
            public int num, letter;
            public cellStatus strokeResult;
            public bool areNearbyCellsChecked {set; get;}  // добавить проверку в конструктор

            public int[]? HittedBoat { get; }

            public Stroke() {
                num = -1;
                letter = -1;
                strokeResult = cellStatus.Missed;
                HittedBoat = null;
            }
            public Stroke(int n, int l, cellStatus status, Field field) {
                num = n;
                letter = l;
                strokeResult = status;
                if (strokeResult == cellStatus.Hitted)
                    HittedBoat = field.getSettledBoat(num, letter);
                else HittedBoat = null;
            }

            public void setHittedBoatStatus(Flotilia flotilia, Field field) {
                if (strokeResult == cellStatus.Hitted && HittedBoat != null) {
                    flotilia.Flot[HittedBoat[0]][HittedBoat[1]].setBoatStatus(field);
                } 
            }

            public string showStrokeResult(Field field, Flotilia flotilia) {
                if (num == -1 || letter == -1) 
                    return "     ";
                else {
                    if (strokeResult == cellStatus.Missed)
                        return "Мимо ";
                    else if (strokeResult == cellStatus.Hitted && HittedBoat != null) {
                        if (flotilia.Flot[HittedBoat[0]][HittedBoat[1]].Status == boatStatus.Damaged)
                            return "Ранил";
                        else if ((flotilia.Flot[HittedBoat[0]][HittedBoat[1]].Status == boatStatus.Sunk))
                            return "Убил ";
                        else return "";
                    }
                    else return "";
                }
            }
    

        }


        public List<Stroke> compStrokes = new List<Stroke>(){new Stroke()};
        public List<Stroke> gamerStrokes = new List<Stroke>(){new Stroke()};

        public Stroke getPrevStroke(List<Stroke> strokes) {
            return strokes[strokes.Count-1];
        }
        public bool isTargetSunk;
        public bool isTargetFound;

        public void StrikeComputer() {
            int num, letter;
            string? coord;
            string message;
            Console.WriteLine("\t\tВведите координаты для удара (формат А1 или а1):");
            do {
                Console.Write("\t\t");
                coord = Console.ReadLine();
                if (String.IsNullOrEmpty(coord)) {
                    Console.WriteLine("\t\tКоординаты не были введены!!");
                    Console.WriteLine("\t\tВведите координаты первой ячейки еще раз (формат А1 или а1):");
                    continue;
                } 
                if (convertDataToCoord(coord, out num, out letter, out message)) {
                    if (ComputerField.HitCell(num, letter, out cellStatus result)){
                        var curStroke = new Stroke(num, letter, result, ComputerField);
                        gamerStrokes.Add(curStroke);
                        curStroke.setHittedBoatStatus(ComputerFlotilia, ComputerField);
                    }
                        
                    else {
                        Console.WriteLine("\t\tПо этой ячейке Вы уже ударяли или здесь тонет корабль. Попробуйте еще раз!");
                        StrikeComputer();
                    }
                    break;
                }
                else {
                    MyExtensions.WriteInRed("\t" + message);
                    continue;
                } 
            } while(true);
            
        }
        

        public void StrikeGamerRandomly() {
            if(!StrikeGamer(MyExtensions.rand.Next(0,10), MyExtensions.rand.Next(0,10)))
                StrikeGamerRandomly();
        }

        public bool StrikeGamer(int num, int letter) {
            if(!GamerField.HitCell(num, letter, out cellStatus status))
                return false;
            else { 
                Stroke currentStroke = new Stroke(num, letter, status, GamerField);
                compStrokes.Add(currentStroke); 
                currentStroke.setHittedBoatStatus(GamerFlotilia, GamerField);
                return true; 
            }
        }

        
       

        public void StrikeGamerNearby() {
            Stroke? lastHittingStroke = compStrokes.FindLast(x => x.strokeResult == cellStatus.Hitted);
            int[] boat = new int[2];
            if (lastHittingStroke != null && lastHittingStroke.HittedBoat != null) boat = lastHittingStroke.HittedBoat;
            var indexOfHittingStrokes = compStrokes.Select((stroke, i) => new {stroke, i})
                .Where(s => s.stroke.strokeResult == cellStatus.Hitted && s.stroke.HittedBoat != null  
                    && s.stroke.HittedBoat[0] == boat[0]
                    && s.stroke.HittedBoat[1] == boat[1])
                .OrderByDescending(s => s.i)
                .Select(s => s.i)
                .ToList();
            
            foreach (int i in indexOfHittingStrokes) {
                var currentStroke = compStrokes[i];
                if (!currentStroke.areNearbyCellsChecked) {
                    var nearbyCells = GamerField.getNearbyCells(currentStroke.num, currentStroke.letter); 

                    if (indexOfHittingStrokes.Count() > 1) { 
                        int iLast = indexOfHittingStrokes[indexOfHittingStrokes.Count-1];
                        if (i == iLast) iLast = indexOfHittingStrokes[indexOfHittingStrokes.Count-2];
                        if (compStrokes[i].num == compStrokes[iLast].num) {  
                            var filterednearbyCells = from cell in nearbyCells
                            where cell.Item1 == compStrokes[i].num && cell.Item3 == false
                            select cell;
                            foreach (var item in filterednearbyCells) {
                                if(StrikeGamer(item.Item1, item.Item2)) return;
                            }
                            currentStroke.areNearbyCellsChecked = true;
                        } 
                        else if (compStrokes[i].letter == compStrokes[iLast].letter) {
                            var filterednearbyCells = from cell in nearbyCells
                            where cell.Item2 == compStrokes[i].letter && cell.Item3 == false
                            select cell;
                            foreach (var item in filterednearbyCells) {
                                if (StrikeGamer(item.Item1, item.Item2)) return;   
                            }
                            currentStroke.areNearbyCellsChecked = true;
                        } else StrikeGamerDiagonally();
                    }
                    else if ((indexOfHittingStrokes.Count() == 1)) {
                        nearbyCells.Shuffle();
                        foreach (var item in nearbyCells) {
                            if (!GamerField.isCellHitted(item.Item1,item.Item2)) {
                                StrikeGamer(item.Item1, item.Item2);
                                return;  
                            } 
                        }
                        currentStroke.areNearbyCellsChecked = true;
                    }
                    else StrikeGamerDiagonally();
                } 
            }
        }
        
        public List<strikeStrategies.strikeIndices>[] listOfSets = new List<strikeStrategies.strikeIndices>[2] {
                                                                   strikeStrategies.getlinesSetOfStrokes(), 
                                                                   strikeStrategies.getpointsSetOfStrokes()};
        public int currentPosition;
        public List<strikeStrategies.strikeIndices>? currentSetOfStrokes;
        
        public void StrikeGamerDiagonally() {
            if (currentSetOfStrokes == null && compStrokes.Count > 1) {
                StrikeGamerRandomly();
            }
            else {
                if (compStrokes.Count == 1) {
                    int option = MyExtensions.rand.Next(0,listOfSets.Length);
                    currentSetOfStrokes = listOfSets[option];

                }
                if (currentSetOfStrokes != null) {
                    foreach (var (value, i) in currentSetOfStrokes.Select((value, i) => ( value, i )).Where(v => v.i >= currentPosition)) {
                        if (StrikeGamer(value.i, value.j)) {
                            currentPosition = i+1;
                            return;
                        } 
                    }  
                    currentSetOfStrokes = null;  
                    StrikeGamerRandomly(); 
                } else {
                    currentSetOfStrokes = null;
                    StrikeGamerRandomly();
                }                        
            }
        }

        public void StrikeGamer() {
            if (compStrokes.Last().strokeResult == cellStatus.Hitted) {
                isTargetFound = true;
                int[] boat = GamerField.getSettledBoat(compStrokes.Last().num, compStrokes.Last().letter);
                if (GamerFlotilia.Flot[boat[0]][boat[1]].Status == boatStatus.Sunk)
                {
                    isTargetSunk = true;
                    isTargetFound = false;
                    StrikeGamerDiagonally();
                }
                else {
                    isTargetSunk = false;
                    StrikeGamerNearby();
                }   
            }
            else if (compStrokes.Last().strokeResult == cellStatus.Missed) {
                if (isTargetFound && !isTargetSunk) {
                    StrikeGamerNearby();
                }
                else StrikeGamerDiagonally();
            }  
            else StrikeGamerDiagonally();  
        }

        static double gameTimer;

        public void Fight() {
            var timer = new Stopwatch();
            timer.Start();
            while (!(ComputerFlotilia.areAllBoatsSunk() || GamerFlotilia.areAllBoatsSunk())) {
                Console.Clear();
                printBothFields();
                StrikeComputer();
                StrikeGamer();
            };
            timer.Stop();
            gameTimer = (double)(timer.ElapsedMilliseconds/60000);
            Console.Clear();
            printBothFields();
            if (GamerFlotilia.areAllBoatsSunk() && !ComputerFlotilia.areAllBoatsSunk()) {
                MyExtensions.WriteInRed("\t\t\tВы проиграли"); 
                Console.WriteLine("\n\t" + "Чтобы расскрыть поле противника, нажмите \"1\"");
                Console.WriteLine("\t" + "Чтобы продолжить, нажмите \"2\"");
                byte options;
                Console.Write("\t");
                while((!Byte.TryParse(Console.ReadLine(), out options)) || (options != 1 && options != 2)) { 
                    Console.WriteLine("\tНеизвестная команда, попробуйте еще раз.");
                    Console.Write("\t");
                }     
                if (options == 1) {
                    Console.WriteLine();
                    ComputerField.PrintField(16,Console.CursorTop+2,ComputerFlotilia, false);
                    Console.WriteLine();
                } 
                else return;
            }
            else if (ComputerFlotilia.areAllBoatsSunk() && !GamerFlotilia.areAllBoatsSunk()) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\t\t\tВЫ ВЫИГРАЛИ!!!");
                Console.ForegroundColor = ConsoleColor.White;
            } 
            else  Console.WriteLine("\t\t\tНИЧЬЯ!");
        }

        public int HittedStrokeCount(List<Stroke> strokes) {
            int count = 0;
            foreach (var stroke in strokes)
                if (stroke.strokeResult == cellStatus.Hitted)
                    count++;
            return count;
        }

    }
}