// ReSharper disable All
namespace Quiz;

public class Questions
{
    private readonly string _question;
    public readonly string? Response;
    private readonly Dictionary<int,string>? _multipleResponse;
    private readonly int _goodResponse = -1;

    public Questions(string question, Dictionary<int,string> response,int good)
    {
        this._question = question;
        this._multipleResponse = response;
        this._goodResponse = good;
    } 
    public Questions(string question, string response)
    {
        this._question = question;
        this.Response = response;
    }

    public static bool AskQuestions(Questions qu)
    {
        Console.WriteLine(qu._question);
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
        Console.WriteLine(qu._question + $"\n La réponse est {qu.Response}");
        Console.ResetColor();
        return false;
       
    }

    public static bool AskQcm(Questions qu)
    {
        Console.WriteLine(qu._question);
        foreach (var re in qu._multipleResponse!)
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
        if (number == qu._goodResponse)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Vrai");
            Console.ResetColor();
            return true;
        }
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Faux");
        Console.WriteLine(qu._question + $"\n La réponse est {qu._multipleResponse[qu._goodResponse]}");
        Console.ResetColor();
        return false;
    }
   
}