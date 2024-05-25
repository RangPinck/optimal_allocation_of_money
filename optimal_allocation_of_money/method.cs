using System.ComponentModel.DataAnnotations;
using System.Text;

namespace optimal_allocation_of_money
{
    class method
    {
        string pathIn { get; set; }
        string pathOut { get; set; }
        int? CostStep;
        List<List<int?>> matrix = new List<List<int?>>();
        //0 - статусы; i - сумма процентов; i+1 - индексация максимальной суммы (1 - сумма максимальная; null - нет); i+2 - запись значений
        List<List<int?>> StoryOptimaise = new List<List<int?>>();
        List<int?> moneyInBank = new List<int?>();
        int? finction { get; set; }

        public method(string pathIn, string pathOut)
        {
            this.pathIn = pathIn;
            this.pathOut = pathOut;

            GetDate();

            CreateStoryListMarcup();

            CreateStory();

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
                Console.WriteLine($"{i + 1}\t=\t{moneyInBank[i]}");
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

        void CreateStory()
        {
            if (StoryOptimaise.Count == 1)
            {
                SumProcentFirst();
            }
            else
            {
                SumProcent();
            }

            GetMaxInCategory();

            GetCostContributionInCategory();
        }

        void SumProcentFirst()
        {
            int CountSteps = 1;

            List<int?> temp = new List<int?>();

            int IndexLastBank = (int)matrix[0][matrix[0].Count - 1];

            for (int i = 0; i < StoryOptimaise[0].Count; i++)
            {
                if (StoryOptimaise[0][i] == 0) CountSteps++;

                temp.Add(
                    GetProcentContribution(IndexLastBank - 1, 1 + StoryOptimaise[0][i] / CostStep) +
                    GetProcentContribution(IndexLastBank, 1 + (matrix[CountSteps][0] - StoryOptimaise[0][i]) / CostStep)
                    );
            }

            StoryOptimaise.Add(new List<int?>(temp));
        }

        int? GetProcentContribution(int? indexBanc, int? costContribution)
        => matrix[index: (int)costContribution][index: (int)indexBanc];

        void SumProcent()
        {

        }

        void GetMaxInCategory()
        {
            List<int?> temp = new List<int?>();

            int? maxInCategory = 0;
            int indexStartCategory = 0;
            int CountElementsInCategory = 2;

            for (int i = 0; i < matrix.Count - 2; i++)
            {
                maxInCategory = StoryOptimaise[StoryOptimaise.Count - 1].GetRange(indexStartCategory, CountElementsInCategory).Max();

                for (int j = indexStartCategory; j < indexStartCategory+CountElementsInCategory; j++)
                {
                    if (StoryOptimaise[StoryOptimaise.Count - 1][j] == maxInCategory)
                    {
                        temp.Add(1);
                        continue;
                    }
                    temp.Add(null);
                }

                indexStartCategory += CountElementsInCategory;
                CountElementsInCategory++;
            }

            StoryOptimaise.Add(new List<int?>(temp));
        }

        void GetCostContributionInCategory()
        {

        }
    }
}
