// ReSharper disable All
namespace Quiz
{
     class Quiz
    {
        static async Task Main()
        {
            Console.WriteLine("Taper le mot de passe administrateur");
            var password = Console.ReadLine();
            if (password == null)
            {
                Console.WriteLine("Erreur stdin fermé");
                System.Environment.Exit(-1);
            }

            var questionsFile = "../../../questions.txt";
            if (password == "admin")
            {
                var quList = LoadQuestions(questionsFile);
                //AddQestion("../../../questions.txt");
                await DeleteQuestion(questionsFile,quList);
            }
            List<Questions> questionsList = new List<Questions>();
            try
            {
                questionsList = LoadQuestions(questionsFile);
                
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

        private static async Task DeleteQuestion(string path, List<Questions> questionsList)
        {
            var startIndex = 0;
            foreach (var qu in questionsList)
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
                System.Environment.Exit(-1);
            }

            var r = response.ToCharArray()[0];
            if ( r == 'n' )
            {
                return;
            }

            var index = Int32.Parse(response);
            questionsList.RemoveAt(index);
            using StreamWriter file = new StreamWriter(path);
            foreach (var qu in questionsList)
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
    }
}
