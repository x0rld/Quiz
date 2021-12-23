// ReSharper disable All
namespace Quiz
{

    public class Questions
    {
        public string Question { get; }
        public  string? Response  { get; }
        public Dictionary<int, string>? MultipleResponse { get; }
        public int GoodResponse  { get; }
        public Questions(string question, Dictionary<int, string> response, int good)
        {
            this.Question = question;
            this.MultipleResponse = response;
            this.GoodResponse = good;
        }

        public Questions(string question, string response)
        {
            this.Question = question;
            this.Response = response;
            this.GoodResponse = -1;
        }

        public static bool AskQuestions(Questions qu)
        {
            Console.WriteLine(qu.Question);
            var resp = Console.ReadLine()?.Trim();
            if (resp == null)
            {
                throw new Exception("erreur d'entrée");
            }

            if (resp == qu.Response)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Vrai");
                Console.ResetColor();
                return true;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Faux");
            Console.WriteLine(qu.Question + $"\n La réponse est {qu.Response}");
            Console.ResetColor();
            return false;

        }

        public static bool AskQcm(Questions qu)
        {
            Console.WriteLine(qu.Question);
            foreach (var re in qu.MultipleResponse!)
            {
                Console.WriteLine($"{re.Key}: {re.Value}");
            }

            Console.WriteLine("Veuillez répondre avec le numéro de la réponse");
            var resp = Console.ReadLine()?.Trim();
            if (resp == null)
            {
                throw new Exception("erreur d'entrée");
            }

            var number = resp.ToCharArray()[0] - '0';
            if (number == qu.GoodResponse)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Vrai");
                Console.ResetColor();
                return true;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Faux");
            Console.WriteLine(qu.Question + $"\n La réponse est {qu.MultipleResponse[qu.GoodResponse]}");
            Console.ResetColor();
            return false;
        }

    }
}