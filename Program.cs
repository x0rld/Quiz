// ReSharper disable All
namespace Quiz
{
    static class Quiz
    {
        static void Main()
        {
            Console.WriteLine("Taper le mot de passe administrateur");
            var password = Console.ReadLine();
            if (password == null)
            {
                Console.WriteLine("Erreur stdin fermé");
                System.Environment.Exit(-1);
            }
            if (password == "admin")
            {
                AddQestion("../../../questions.txt");
            }
            List<Questions> questionsList = new List<Questions>();
            try
            {
                questionsList = LoadQuestions("../../../questions.txt");
                if (questionsList.Count == 0)
                {
                    Console.WriteLine("le fichier est vide");
                }
                StartQuiz(questionsList);
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("file not found!");   
            }  
            catch (System.UnauthorizedAccessException)
            {
                Console.WriteLine("permissions denied!");   
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.Message);   
            }
        }

        private static void StartQuiz(List<Questions> questionsList)
        {
            var res = 0;
            foreach (var qu in questionsList)
            {
                if (qu.Response != null)
                {
                    res += Questions.AskQuestions(qu) ? 1 : 0 ;                 
                }
                else
                {
                    res += Questions.AskQcm(qu) ? 1 : 0 ;
                }
            }
            Console.WriteLine($"vous avez eu {res} bonne réponses sur {questionsList.Count} !");
        }

        private static List<Questions>  LoadQuestions(string path)
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
            return questionsList;
        }

        private static void AddQestion(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("fichier non trouvé");
            }
            Console.WriteLine("Taper la questions à ajouter");
            var question = Console.ReadLine();
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
    }
}
