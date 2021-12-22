namespace Quiz;

public class Questions
{
    public readonly string question;
    public readonly string response;

    public Questions(string question, string response)
    {
        this.question = question;
        this.response = response;
    }

    public static bool AskQuestions(Questions qu)
    {
        Console.WriteLine(qu.question);
        var resp = Console.ReadLine()?.Trim();
        if (resp == null)
        {
            throw new Exception("erreur d'entrée");
        }
        if (resp == qu.response)
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