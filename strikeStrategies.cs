namespace morskoyBoy
{
    public partial class ConsoleGame {
        public class strikeStrategies {

            public struct strikeIndices { 
                public int i; 
                public int j; 
                public strikeIndices(int i, int j) { this.i = i; this.j = j; }
            }
            static List<strikeIndices> setOfStrokes = fillSetOfStrokes();

            static public List<strikeIndices> fillSetOfStrokes() {
                var fullSetOfStrokes = new List<strikeIndices>();
                for (int i = 0; i < 10; i++) {
                    for (int j = 0; j < 10; j++) {
                        fullSetOfStrokes.Add(new strikeIndices(i,j));
                    }  
                }
                return fullSetOfStrokes;
            }

            static public List<strikeIndices> getlinesSetOfStrokes() {
                List<strikeIndices> list = new List<strikeIndices>();
                int randomOption = MyExtensions.rand.Next(2);
                if (MyExtensions.rand.Next(2) == 0) {
                    for (int i = randomOption; i <= 18; i+=2) {
                        list.AddRange(setOfStrokes.Where((x) => x.i + x.j == i).ToList());
                    } 
                }
                else {
                    for (int i = randomOption; i < 10; i+=2) {
                        list.AddRange(setOfStrokes.Where((x) => Math.Abs(x.j - x.i) == i).ToList());
                    }
                }
                if (MyExtensions.rand.Next(2) == 0) {
                    list.Reverse();
                }
                return list; 
            }

            static public List<strikeIndices> getpointsSetOfStrokes() {
                int randomOption = MyExtensions.rand.Next(2);
                List<strikeIndices> list = new List<strikeIndices>();
                if (MyExtensions.rand.Next(2) == 0) {
                    for (int i = 0; i < 10; i++) {
                        for (int j = ( i%2 == randomOption ? 0:1 ); j < 10; j += 2) {
                            list.Add(setOfStrokes.Find( (x) => x.i == i && x.j == j ));
                        }
                    } 
                } else {
                    for (int j = 0; j < 10; j++) {
                        for (int i = ( j%2 == randomOption ? 0:1 ); i < 10; i += 2) {
                            list.Add(setOfStrokes.Find( (x) => x.i == i && x.j == j ));
                        }
                    }
                }
                if (MyExtensions.rand.Next(2) == 0) {
                    list.Reverse();
                }
                return list; 
            }

        }

    }
}