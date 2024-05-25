namespace optimal_allocation_of_money
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Добро пожаловать! Это программа для расчёта оптимального вклада в банк.");
            Console.WriteLine("Данные о количестве вклада и сумме будут взяты из файла программы.");
            method met = new method("input.txt", "output.txt");

        }
    }
}
