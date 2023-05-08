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

}

        