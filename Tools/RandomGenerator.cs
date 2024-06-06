namespace HomeBankingMindHub.Tools
{
    public class RandomGenerator
    {
        public static string GenerateVIN()
        {
            string numberAccountGenerate = "VIN-";
            Random random = new Random();
            for (int i = 0; i < 8; i++)
            {
                numberAccountGenerate += random.Next(0, 10);
            }
            Console.WriteLine(numberAccountGenerate);
            return numberAccountGenerate;
        }

        public static string GenerateCardNumber()
        {
            string cardNumberGenerate = string.Empty;
            Random random = new Random();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    cardNumberGenerate += random.Next(0, 10);
                }
                if (i != 3)
                    cardNumberGenerate += "-";
            }
            Console.WriteLine(cardNumberGenerate);
            return cardNumberGenerate;
        }

        public static int GenerateCVV()
        {
            int CVVGenerate;
            Random random = new Random();
            CVVGenerate= random.Next(0, 1000);

            Console.WriteLine(CVVGenerate);
            return CVVGenerate;
        }
    }
}
