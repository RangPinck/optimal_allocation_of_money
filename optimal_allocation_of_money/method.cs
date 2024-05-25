using System.Text;

namespace optimal_allocation_of_money
{
    class method
    {
        string pathIn{ get; set; }
        string pathOut { get; set; }
        int? CostStep;
        List<List<int?>> matrix = new List<List<int?>>();
        //
        List<List<int?>> StoryOptimaise = new List<List<int?>>();
        List<int?>moneyInBank = new List<int?>();
        int? finction{ get; set; }


        public method(string pathIn, string pathOut)
        {
            this.pathIn = pathIn;
            this.pathOut = pathOut;

            GetDate();

            CreateStoryListMarcup();

            PostData();
        }

        void GetDate()
        {
            using StreamReader sr = new StreamReader(pathIn);
            string line;
            List<int?> temp = new List<int?>();
            while ((line = sr.ReadLine()) != null)
            {
                foreach (var item in line.Split('\t'))
                {
                    if (item == "")
                    {
                        temp.Add(null);
                        continue;
                    }

                    temp.Add(int.Parse(item));
                }
                matrix.Add(new List<int?>(temp));
                temp.Clear();
            }
            CostStep = matrix[2][0] - matrix[1][0];
        }

        void PostData()
        {
            using StreamWriter sw = new StreamWriter(pathOut, false, Encoding.UTF8);
            sw.WriteLine("Банки:");
            for (int i = 0; i < moneyInBank.Count; i++)
            {
                Console.WriteLine($"{i+1}\t=\t{moneyInBank[i]}");
            }
            sw.Write($"Максимальная прибыль = {finction}%");
        }

        void CreateStoryListMarcup()
        {
            List<int?> temp = new List<int?>();
            for (int i = 2; i < matrix.Count(); i++)
            {
                for (int? j = 0; j <= matrix[i][0]; j += CostStep)
                {
                    temp.Add(j);
                }
            }
            StoryOptimaise.Add(new List<int?>(temp));
        }
    }
}
