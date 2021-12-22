namespace Quiz;

public class Questions
{
    public readonly string Question;
    public readonly string Response;

    public Questions(string question, string response)
    {
        this.Question = question;
        this.Response = response;
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
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Faux");
            Console.WriteLine(qu.Question + $"\n La réponse est {qu.Response}");
            Console.ResetColor();
            return false;
        }
    }

}