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
            Console.WriteLine("Vrai");
            return true;
        }
        else
        {
            Console.WriteLine("Faux");
            return false;
        }
    }

}