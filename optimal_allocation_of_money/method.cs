namespace optimal_allocation_of_money
{
    class method
    {
        string pathIn;
        string pathOut;

        int? CostStep;
        List<List<int?>> matrix = new List<List<int?>>();
        //
        List<int> StoryOptimaise = new List<int>();

        public method(string pathIn, string pathOut)
        {
            this.pathIn = pathIn;
            this.pathOut = pathOut;

            GetDate();

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

            CostStep = matrix[1][2] - matrix[1][1];
        }
    }
}
