namespace Quiz;

public class QuestionManager
{
    private string path;
    private List<Questions>? quList;

    public QuestionManager(string path, List<Questions>? questionsList)
    {
        this.path = path;
        if (questionsList == null)
        {
            throw new ArgumentException();
        }

        quList = questionsList;
    }
        public void AddQestion()
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("fichier non trouvé");
            }
            Console.WriteLine("Taper la questions à ajouter");
            var question = Console.ReadLine()?.Trim();
            if (question == null)
            {
                Console.WriteLine("Erreur stdin fermé");
                System.Environment.Exit(-1);
            }
            File.AppendAllText(path,  question);
            Console.WriteLine("Taper la réponse à cette question");
            var response = Console.ReadLine();
            if (response == null)
            {
                Console.WriteLine("Erreur stdin fermé");
                System.Environment.Exit(-1);
            }
            File.AppendAllText(path,  $",{response}");
            Console.WriteLine("Nouvelle question enregistrée");
        }

        public async Task DeleteQuestion()
        {
            var startIndex = 0;
            foreach (var qu in quList!)
            {
                Console.WriteLine("Question: " + startIndex);
                startIndex++;
                if (qu.Response == null)
                {
                    Console.WriteLine(qu.Question);
                    foreach (var re in qu.MultipleResponse!)
                    {
                        Console.WriteLine($"{re.Key}: {re.Value}");
                    }
                }
                else
                {
                    Console.WriteLine(qu.Question);
                    Console.WriteLine(qu.Response);
                }
            }
            Console.WriteLine("Quelle questions voulez vous supprimer ? (n pour aucune)");
            var response = Console.ReadLine();
            if (response == null)
            {
                Console.WriteLine("Erreur stdin fermé");
                Environment.Exit(-1);
            }

            var r = response.ToCharArray()[0];
            if ( r == 'n' )
            {
                return;
            }

            var index = Int32.Parse(response);
            quList.RemoveAt(index);
            await using StreamWriter file = new StreamWriter(path);
            foreach (var qu in quList)
            {
                if (qu.Response == null)
                {
                    var toWrite = String.Empty;
                    startIndex = 0;
                    toWrite = toWrite.Insert(startIndex,qu.Question+ ",");
                    startIndex+= qu.Question.Length +1;
                    toWrite = toWrite.Insert(startIndex,qu.GoodResponse.ToString() + ",");
                    startIndex+=2;
                    foreach (var q in qu.MultipleResponse!)
                    {
                        toWrite = toWrite.Insert(startIndex,q.Value + ",");
                        startIndex += q.Value.Length +1 ;
                    }

                    if (toWrite[toWrite.Length-1] == ',')
                    {
                        toWrite = toWrite.Remove(toWrite.Length - 1);
                    }
                    await file.WriteAsync(toWrite + "\n");
                }
                else
                {
                    await file.WriteLineAsync(qu.Question + "," + qu.Response);
                }
            }
            Console.WriteLine("Question supprimée");
        }
        public static List<Questions>?  LoadQuestions(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("fichier non trouvé");
            }
            
            var questionsList = new List<Questions>();
            foreach (string line in File.ReadLines(path))
            {
                var data = line.Split(',');
                if (data.Length <2)
                {
                    throw new Exception("Le fichier doit avoir les lignes sous forme questions,réponse " +
                                        "ou questions,numero_réponse,réponses,réponses ");
                }

                if (data.Length == 2)
                {
                    questionsList.Add(new Questions(data[0],data[1]));   
                }
                else
                {
                    var number = data[1].ToCharArray()[0] - '0';
                    var dict = new Dictionary<int, string>();
                    for (int i = 2; i < data.Length; i++)
                    {
                        dict.Add(i -1,data[i]);   
                    }
                    
                    questionsList.Add(new Questions(data[0],dict,number));   
                }
            }

            if (questionsList.Count == 0)
            {
                return null;
            }
            return questionsList;
        }
}