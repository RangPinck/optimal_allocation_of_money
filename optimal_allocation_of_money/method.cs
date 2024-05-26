using System.Text;

namespace optimal_allocation_of_money
{
    class method
    {
        string pathIn { get; set; }
        string pathOut { get; set; }
        int? CostStep;
        List<List<int?>> matrix = new List<List<int?>>();
        //0 - статусы; i - сумма процентов; i+1 - индексация максимальной суммы (1 - сумма максимальная; null - нет)
        List<List<int?>> StoryOptimaise = new List<List<int?>>();
        List<int?> moneyInBank = new List<int?>();
        int? function { get; set; }
        int indexWorkingBank { get; set; }
        public method(string pathIn, string pathOut)
        {
            this.pathIn = pathIn;
            this.pathOut = pathOut;
            GetDate();
            CreateStoryListMarcup();
            CreateStory();
            GetResults();
            GetFunction();
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
            indexWorkingBank = matrix[0].Count - 1;
        }
        void PostData()
        {
            using StreamWriter sw = new StreamWriter(pathOut, false, Encoding.UTF8);
            sw.WriteLine("Банки:");
            for (int i = 0; i < moneyInBank.Count; i++)
            {
                sw.WriteLine($"{i + 1}\t=\t{moneyInBank[i]}");
            }
            sw.Write($"Максимальная прибыль = {function}%");
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
        void CreateStory()
        {
            if (StoryOptimaise.Count == 1)
            {
                SumProcentFirst();
                indexWorkingBank -= 2;
            }
            else
            {
                SumProcent();
                indexWorkingBank--;
            }
            if (indexWorkingBank == 0) return;
            CreateStory();
        }
        void SumProcentFirst()
        {
            int CountSteps = 1;

            List<int?> temp = new List<int?>();

            for (int i = 0; i < StoryOptimaise[0].Count; i++)
            {
                if (StoryOptimaise[0][i] == 0) CountSteps++;

                temp.Add(
                    GetProcentContribution(indexWorkingBank - 1, 1 + StoryOptimaise[0][i] / CostStep) +
                    GetProcentContribution(indexWorkingBank, 1 + (matrix[CountSteps][0] - StoryOptimaise[0][i]) / CostStep)
                    );
            }

            StoryOptimaise.Add(new List<int?>(temp));
        }
        int? GetProcentContribution(int? indexBanc, int? costContribution)
        => matrix[index: (int)costContribution][index: (int)indexBanc];
        void SumProcent()
        {
            int CountSteps = 1;

            List<int?> temp = new List<int?>();



            for (int i = 0; i < StoryOptimaise[0].Count; i++)
            {
                if (StoryOptimaise[0][i] == 0) CountSteps++;

                temp.Add(
                    GetProcentContribution(indexWorkingBank, 1 + StoryOptimaise[0][i] / CostStep) +
                    GetProcentContributionFromStory(matrix[CountSteps][0] - StoryOptimaise[0][i])
                    );
            }

            StoryOptimaise.Add(new List<int?>(temp));
        }
        int? GetProcentContributionFromStory(int? contribytionInStory)
        {
            if (contribytionInStory == 0) return 0;
            int? CountElementsInCategory = 1 + contribytionInStory / CostStep;
            int? indexStartCategory = 0;
            for (int i = 2; i < CountElementsInCategory; i++)
            {
                indexStartCategory += i;
            }
            return StoryOptimaise[StoryOptimaise.Count - 1].GetRange((int)indexStartCategory, (int)CountElementsInCategory).Max();
        }
        void GetResults()
        {
            int? CountElementsInCategory = 1 + StoryOptimaise[0].Max() / CostStep;
            int? indexStartCategory = 0;
            int? max = 0;
            List<int?> temp;
            for (int i = StoryOptimaise.Count-1; i > 0; i--)
            {
                for (int j = 2; j < CountElementsInCategory; j++)
                {
                    indexStartCategory += j;
                }
                temp = new List<int?>(StoryOptimaise[i].GetRange((int)indexStartCategory, (int)CountElementsInCategory).ToList());
                max = temp.Max();
                moneyInBank.Add(temp.IndexOf(max)*CostStep);
                CountElementsInCategory = (CountElementsInCategory - temp.IndexOf(max));
                indexStartCategory = 0;
            }
            moneyInBank.Add(matrix[(int)CountElementsInCategory][0]);
        }
        void GetFunction()
        {
            function = 0;
            for (int i = 0; i < moneyInBank.Count; i++)
            {
                function += matrix[(int)(moneyInBank[i] / CostStep + 1)][i+1];
            }
        }
    }
}
