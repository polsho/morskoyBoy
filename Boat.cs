namespace morskoyBoy
{
    public enum boatStatus {
          OK
        , Intact  
        , Damaged
        , Sunk
    }
    public enum boatDirection {
          Vert
        , Horiz
    }

    public struct boatPosition {
        public int CoordLetter { get; set; }
        public int CoordNum { get; set; }
        public boatDirection Direction { get; set; }

        public boatPosition(int cl, int cn, boatDirection d) {
            CoordLetter = cl;
            CoordNum = cn;
            Direction = d;
        }

        public void setData (int CoordNum, int CoordLetter, boatDirection Direction) {
            this.CoordLetter = CoordLetter;
            this.CoordNum = CoordNum;
            this.Direction = Direction;
        }
    }

    
    public class Boat {
        public int Size { get; private set; }
        public boatStatus Status { get; set; }
        protected boatPosition pos;
        public int[,] OccupiedCells { get; set; }
        public string Name { get; protected set; }
        
        public Boat (int size) {
            Size = size;
            Status = boatStatus.OK;
            OccupiedCells = new int[size,2];
            Name = "Корабль";
        }
    
        public bool SetPosition(boatPosition p) {
            pos.Direction = p.Direction;
            switch(pos.Direction) {
                case boatDirection.Vert:
                    if ((p.CoordLetter >= 0 && p.CoordLetter <= 9) && (p.CoordNum >= 0 && p.CoordNum <= 10 - Size))
                        { pos.CoordNum = p.CoordNum; pos.CoordLetter = p.CoordLetter; return true; }
                    else return false;
                case boatDirection.Horiz:
                    if ((p.CoordLetter >= 0 && p.CoordLetter <= 10 - Size) && (p.CoordNum >= 0 && p.CoordNum <= 9))
                        { pos.CoordNum = p.CoordNum; pos.CoordLetter = p.CoordLetter; return true; }
                    else return false;
                default:
                    return false;
            }
        }
        public void SetOccupiedCells(){
            try {  
                if (pos.Direction == boatDirection.Horiz) {
                    for (int i = 0; i < Size; i++) {
                        OccupiedCells[i,0] = pos.CoordNum;
                        OccupiedCells[i,1] = pos.CoordLetter + i;
                    }
                    
                }
                else if (pos.Direction == boatDirection.Vert) {
                    for (int i = 0; i < Size; i++) {
                        OccupiedCells[i,0] = pos.CoordNum + i;
                        OccupiedCells[i,1] = pos.CoordLetter;
                    }
                }
            }
            catch(ArgumentOutOfRangeException e) { Console.WriteLine(e.Message); }  
        }

        public void setSettledBoat(Field field, int boatType, int boatNumber) {
            for (int i = 0; i < Size; i++) {
                    field.setSettledBoat(OccupiedCells[i,0],OccupiedCells[i,1], new int[]{boatType, boatNumber});
                }
        }

        public bool PutBoat(Field field) {
            try {    
                if (pos.Direction == boatDirection.Horiz) {
                    for (int i = pos.CoordLetter; i < pos.CoordLetter + Size; i++) {
                        if(!field.FillCell(pos.CoordNum, i)) {
                            for (int j = i - 1; j >= pos.CoordLetter; j--)
                                field.UnfillCell(pos.CoordNum, j); 
                            return false; 
                        } 
                    }
                }
                else if (pos.Direction == boatDirection.Vert) {
                    for (int i = pos.CoordNum; i < pos.CoordNum + Size; i++) {
                        if(!field.FillCell(i, pos.CoordLetter)) { 
                            for (int j = i - 1; j >= pos.CoordNum; j--)
                                field.UnfillCell(j, pos.CoordLetter);
                            return false; 
                        } 
                    }
                }
            }
            catch(ArgumentOutOfRangeException) { return false; }             
            Status = boatStatus.Intact;
            SetOccupiedCells();
            EncloseBoat(field);
            return true;
        }
        public bool PutBoat(Field field, boatPosition p, ref int count, out string message) {
            if (count > 0) {
                if (SetPosition(p)) {
                    if (PutBoat(field)) {
                        count--;
                        message = $"\t{Name} установлен!";
                        return true;
                    }
                    else {
                        message = "\tСлишком близко к другому кораблю!";
                        return false;
                    }
                }   
                else {
                    message = "\tНеверные данные для размещения корабля!";    
                    return false;
                }
            }
            else {
                message = "Количество кораблей данного типа превышано!";
                return false;
            } 
        }

        public bool UnputBoat() {
            if (Status == boatStatus.Intact) {
                for (int i = 0; i < Size; i++) {
                    OccupiedCells[i,0] = 0;
                    OccupiedCells[i,1] = 0; 
                }
                Status = boatStatus.OK;
                return true;
            } else return false;
        }

        public void EncloseBoat(Field field) {
            try {    
                for (int i = 0; i < Size; i++) {
                    field.EncloseCell(OccupiedCells[i,0],OccupiedCells[i,1]);
                }
            }
            catch(ArgumentOutOfRangeException e) { Console.WriteLine(e.Message); }             
        }
        public void EncloseSunkBoat(Field field) {
            try {    
                for (int i = 0; i < Size; i++) {
                    field.EncloseHittedCell(OccupiedCells[i,0],OccupiedCells[i,1]);
                }
            }
            catch(ArgumentOutOfRangeException e) { Console.WriteLine(e.Message); }             
        }
    
        public void setBoatDamaged(Field field) {   
            if (Status != boatStatus.Damaged) {
                for (int i = 0; i < Size; i++) {
                    if (field.GetCellStatus(OccupiedCells[i,0], OccupiedCells[i,1]) == cellStatus.Hitted)
                        Status = boatStatus.Damaged;
                }
            } 
        }  
        public bool setBoatSunk(Field field) {
            if (Status != boatStatus.Sunk) {
                    for (int i = 0; i < Size; i++) {
                    if (!(field.GetCellStatus(OccupiedCells[i,0], OccupiedCells[i,1]) == cellStatus.Hitted))
                        return false;
                }
                Status = boatStatus.Sunk;
                EncloseSunkBoat(field);
                return true;
            }
            else return true;
        }  

        public void setBoatStatus(Field field){
            if (!setBoatSunk(field))
                setBoatDamaged(field);
        }  
    }


    public class Battleship: Boat {

        public Battleship():base(4) {
            Name = "Линкор";
         }
    }

    public class Cruiser: Boat {
        public Cruiser():base(3) {
            Name = "Крейсер";
        }
    }

    public class Destroyer: Boat {
        public Destroyer():base(2) {
            Name = "Эсминец";
        }
    }

    public class Powerboat: Boat {
        public Powerboat():base(1) {
            Name = "Катер";
        }
    }
}
