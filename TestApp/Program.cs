using TestApp;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine(@"Command: 
                1: Db2Class
                2: Add all images
                3: Change image path");
        var c = Console.ReadKey();
        if (c.KeyChar == '1')
        {
            ClassGenerator.Generate();
        }
        else if (c.KeyChar == '2')
        {
            Console.WriteLine("\r\nEnter project csproj file path:");
            string filePath = Console.ReadLine();
            ClassGenerator.AddImages(filePath);
        }
        else if (c.KeyChar == '3')
        {
            Console.WriteLine("\r\nEnter solution folder:");
            string filePath = Console.ReadLine();
            ClassGenerator.AddImages(filePath);
        }
        else
        {

        }
    }
}