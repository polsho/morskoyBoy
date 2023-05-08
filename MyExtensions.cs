static class MyExtensions {
    public static Random rand = new Random();
    public static void Shuffle<T>(this IList<T> list) {
        int n = list.Count;
            while (n > 1) {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
    }

    public static int origRow;
    public static int origCol;

    public static void WriteAt(string s, int x, int y)
    {
    try
        {
        Console.SetCursorPosition(origCol+x, origRow+y);
        Console.Write(s);
        }
    catch (ArgumentOutOfRangeException e)
        {
        Console.Clear();
        Console.WriteLine(e.Message);
        }
    }

    public static void WriteInRed(String str) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(str);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void WriteInGreen(String str) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(str);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void WriteInBlue(String str) {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(str);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static string writeHorizLine(int length) {
        string line = "";
        for (int i = 0; i < length; i++)
            line += "─";
        return line;
    }
    public static string writeHeader(int length) {
        string line = "┌";
        for (int i = 0; i < length; i++)
            line += "─";
        line += "┐";
        return line;
    }

    public static string writeBottom(int length) {
        string line = "└";
        for (int i = 0; i < length; i++)
            line += "─";
        line += "┘";
        return line;
    }

    public static void writeLetterO (int letterHeight, string element) {
        for (int i = 0; i < letterHeight; i++) {
                if (i == 0 || i == 7) {
                    WriteAt(element, 3, i);
                    WriteAt(element, 5, i);
                }
                if (i == 1 || i == 6) {
                    WriteAt(element, 1, i);
                    WriteAt(element, 7, i);
                }
                if (i >= letterHeight/4 && i < letterHeight/4 + letterHeight/2) {
                    WriteAt(element, 0, i);
                    WriteAt(element, letterHeight, i);
                }
            }
    }
    public static void writeLetterM (int letterHeight, string element) {
        for (int i = 0; i < letterHeight; i++) {
            MyExtensions.WriteAt(element, 0, i);
            if (i <= letterHeight/2) {
                MyExtensions.WriteAt(element, i, i);
                MyExtensions.WriteAt(element, letterHeight - i, i);
            }
            MyExtensions.WriteAt(element, letterHeight, i);
        }
    }   
    public static void writeLetterR (int letterHeight, string element) {
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
    }
    public static void writeLetterS (int letterHeight, string element) {
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
                }
            }
    }
    public static void writeLetterK (int letterHeight, string element) {
        for (int i = 0; i < letterHeight; i++) {
                MyExtensions.WriteAt(element, 0, i);
                if (i <= letterHeight/2-1)
                    MyExtensions.WriteAt(element, letterHeight-1-i, i);
                if (i == letterHeight/2-1)
                    MyExtensions.WriteAt(element, 2, i);
                if (i >= letterHeight/2)
                    MyExtensions.WriteAt(element, i+1, i);
            }
    }
    public static void writeLetterY (int letterHeight, string element) {
        MyExtensions.WriteAt(element, letterHeight/2, -1);
            MyExtensions.WriteAt(element, letterHeight/2+1, -1);
            for (int i = 0; i < letterHeight; i++) {
                MyExtensions.WriteAt(element, 0, i);
                MyExtensions.WriteAt(element, letterHeight - i, i);
                MyExtensions.WriteAt(element, letterHeight, i);
            }
    }
    public static void writeLetterB (int letterHeight, string element) {
        for (int i = 0; i < letterHeight; i++) {
                if (i == 0) {
                    for (int j = 0; j <= letterHeight ; j+=2)
                        MyExtensions.WriteAt(element, j, i);
                }
                if (i == letterHeight-1 || i == 3) {
                    for (int j = 0; j < letterHeight ; j+=2)
                        MyExtensions.WriteAt(element, j, i);
                }
                if (i == 4 || i == 6) {
                    MyExtensions.WriteAt(element, 0, i);
                    MyExtensions.WriteAt(element, 7, i);
                }
                if (i == 5) {
                    MyExtensions.WriteAt(element, 0, i);
                    MyExtensions.WriteAt(element, 8, i);
                }
                else MyExtensions.WriteAt(element, 0, i);
            }
    }

}

        