namespace Quiz;

public class Questions
{
    private readonly string Question;
    public readonly string? Response;
    private readonly Dictionary<int,string>? MultipleResponse;
    private int goodResponse = -1;

    public Questions(string question, Dictionary<int,string> response,int good)
    {
        this.Question = question;
        this.MultipleResponse = response;
        this.goodResponse = good;
    } 
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
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Faux");
        Console.WriteLine(qu.Question + $"\n La réponse est {qu.Response}");
        Console.ResetColor();
        return false;
       
    }

    public static bool AskQCM(Questions qu)
    {
        Console.WriteLine(qu.Question);
        foreach (var re in qu.MultipleResponse)
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
        if (number == qu.goodResponse)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Vrai");
            Console.ResetColor();
            return true;
        }
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Faux");
        Console.WriteLine(qu.Question + $"\n La réponse est {qu.MultipleResponse[qu.goodResponse]}");
        Console.ResetColor();
        return false;
    }
   
}