using System.Linq;

namespace morskoyBoy
{
    public enum cellStatus {
      Empty    = '\u007e'  // ~
    , Filled   = '\u25a0'  // ■
    , Hitted   = '\u2666'  // ♦
    , Blocked  = '\u2219'  // ∙
    , Missed   = '\u0078'  // x
    }

    public class Field {
        Cell[ , ] cells;

        private class Cell {
            public cellStatus Status { get; private set;}
            public bool isHitted { get; set;}

            public int[] settledBoat { get; set;}
            public Cell() { 
                Status = cellStatus.Empty; 
                isHitted = false;
                settledBoat = new int[2]{-1,-1};
            }

            public void StatusToBlocked() {
                if (Status == cellStatus.Empty || Status == cellStatus.Blocked)
                    Status = cellStatus.Blocked;
            }
            public bool StatusToFilled() {
                if (Status != cellStatus.Empty) return false;
                else {
                    Status = cellStatus.Filled;
                    return true;
                }
            }
            public void StatusToEmpty() {
                Status = cellStatus.Empty;
            }

            public bool StatusToHittedOrMissed(out cellStatus result) {
                if (Status == cellStatus.Filled){
                    Status = cellStatus.Hitted;
                    result = cellStatus.Hitted;
                    return true;
                }
                else if (Status == cellStatus.Empty || Status == cellStatus.Blocked) {
                    Status = cellStatus.Missed;
                    result = cellStatus.Missed;
                    return true;
                }
                else {result = cellStatus.Missed; return false; }
            } 
            public cellStatus hideCellStatus() {
                if (isHitted)
                    return Status;
                else return cellStatus.Empty;
            }
        }

    
        public Field() { 
            this.cells = new Cell[10,10];

            for (int i = 0; i < cells.GetLength(0); i++) {
                for (int j = 0; j < cells.GetLength(1); j++) {
                    cells[i,j] = new Cell();
                }  
            }
        }

        public void clearField(){
           for (int i = 0; i < cells.GetLength(0); i++) {
                for (int j = 0; j < cells.GetLength(1); j++) {
                    cells[i,j].StatusToEmpty();
                    cells[i,j].settledBoat[0] = -1;
                    cells[i,j].settledBoat[1] = -1;
                }  
            } 
        }

        public cellStatus GetCellStatus(int num, int letter){
            return cells[num, letter].Status;
        }

        public bool isCellHitted(int num, int letter){
            return cells[num, letter].isHitted;
        }

        public int[] getSettledBoat(int num, int letter) {
            return cells[num, letter].settledBoat;
        }
        public void setSettledBoat(int num, int letter, int[] boat) {
            cells[num, letter].settledBoat[0] = boat[0];
            cells[num, letter].settledBoat[1] = boat[1];
        }

        public bool FillCell(int CoordNum, int CoordLetter) {    
            return cells[CoordNum,CoordLetter].StatusToFilled();
        }
        public void UnfillCell(int CoordNum, int CoordLetter) {    
            cells[CoordNum,CoordLetter].StatusToEmpty();
        }

        public void EncloseCell(int CoordNum, int CoordLetter) {
            for (int i = CoordNum - 1; i <= CoordNum + 1; i++) {
                for (int j = CoordLetter - 1; j <= CoordLetter + 1; j++) {
                    if ( (i >= 0 && i <= 9) && (j >= 0 && j <= 9) ) {
                        cells[i, j].StatusToBlocked();
                    }
                }
            }
        }
        public void EncloseHittedCell(int CoordNum, int CoordLetter) {
            for (int i = CoordNum - 1; i <= CoordNum + 1; i++) {
                for (int j = CoordLetter - 1; j <= CoordLetter + 1; j++) {
                    if ( (i >= 0 && i <= 9) && (j >= 0 && j <= 9) ) {
                        cells[i, j].isHitted = true;
                    }
                }
            }
        }
        public bool HitCell(int CoordNum, int CoordLetter, out cellStatus result) {
            if (cells[CoordNum, CoordLetter].isHitted != true &&
                cells[CoordNum, CoordLetter].StatusToHittedOrMissed(out result)) {
                cells[CoordNum, CoordLetter].isHitted = true;
                return true;
            }
            else {
                result = cellStatus.Missed;
                return false;
            } 
        }

        public List<Tuple<int, int, bool>> getNearbyCells (int num, int letter) {
            var nearbyCells = new List<Tuple<int, int, bool>>(); 
            if (num > 0)    nearbyCells.Add(new Tuple<int, int, bool>(num - 1, letter, cells[num - 1, letter].isHitted));
            if (num < 9)    nearbyCells.Add(new Tuple<int, int, bool>(num + 1, letter, cells[num + 1, letter].isHitted));
            if (letter > 0) nearbyCells.Add(new Tuple<int, int, bool>(num, letter - 1, cells[num, letter - 1].isHitted));
            if (letter < 9) nearbyCells.Add(new Tuple<int, int, bool>(num, letter + 1, cells[num, letter + 1].isHitted));
            return nearbyCells;
        }

        public cellStatus hideCellStatus(int num, int letter) {
            return cells[num,letter].hideCellStatus();
        }

        public void PrintColoredCell(int num, int letter, Flotilia flotilia, bool isHidden) {
            cellStatus status;
            if (isHidden) status = hideCellStatus(num, letter);
            else status = cells[num,letter].Status;
            switch(status) {
                case cellStatus.Empty:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write((char)status + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case cellStatus.Blocked:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write((char)status + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case cellStatus.Hitted:
                    if(flotilia.Flot[getSettledBoat(num,letter)[0]][getSettledBoat(num,letter)[1]].Status == boatStatus.Sunk) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write((char)status + " ");
                        Console.ForegroundColor = ConsoleColor.White;
                    } else {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write((char)status + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                default:
                    Console.Write((char)status + " ");
                    break;
            }
        }

        public int noHittedCellsCount() {
            int count = 0;
            foreach (var cell in cells)
                if (!cell.isHitted)
                    count++;
            return count;
        }

        static string lettersForPrint = "А Б В Г Д Е Ж З И К ";

        public void PrintField(int origX, int origY, Flotilia flotilia, bool hiddenCells) {
            MyExtensions.origCol = origX;
            MyExtensions.origRow = origY; 
            MyExtensions.WriteAt(lettersForPrint, 5, 0);
            MyExtensions.WriteAt(MyExtensions.writeHeader(21), 3, 1);
            for (int i = 0; i < 10; i++) {
                MyExtensions.WriteAt("", 0, i + 2);
                Console.Write(((i < 9)? " ":"") + (i + 1) + " │ ");
                for (int j = 0; j < 10; j++) {
                    PrintColoredCell(i, j, flotilia, hiddenCells);
                }
                Console.Write("│\n");
            }
            MyExtensions.WriteAt(MyExtensions.writeBottom(21), 3, 12);
        }

    }
}