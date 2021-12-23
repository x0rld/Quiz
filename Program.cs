// ReSharper disable All
namespace Quiz
{
     class Quiz
    {
        static async Task Main()
        {
            var questionsFile = "../../../questions.txt";
            List<Questions>? questionsList = null;
            try
            {
                ConsoleColor color;
                questionsList = QuestionManager.LoadQuestions(questionsFile);
                if (questionsList == null) throw new ArgumentNullException("Le fichier est vide");
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Fichier non trouvé!");   
            }  
            catch (System.UnauthorizedAccessException)
            {
                Console.WriteLine("Impossible d'accéder au fichiers, manque de permissions!");   
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.Message);   
            }
            
            Console.WriteLine("1: Administration\n" +
                              "2: Lancer le quiz\n" +
                              "3: Quitter le programme\n" +
                              "(veuillez répondre avec le numéro)");
            switch (Console.ReadLine()?.Trim())
            {
                case null:
                    Console.WriteLine("Erreur stdin fermé");
                    Environment.Exit(-1);
                    break;
                    
                case "1":
                    Console.WriteLine("Taper le mot de passe administrateur");
                    var password = Console.ReadLine();
                    if (password == null)
                    {
                        Console.WriteLine("Erreur stdin fermé");
                        System.Environment.Exit(-1);
                    }

                    QuestionManager? questionManager = null;
                    try
                    {
                       questionManager = QuestionManager.AdminLogin(password, questionsList, questionsFile);

                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        Environment.Exit(-2);
                    }
                    Console.WriteLine("1: Ajouter une question\n" +
                                      "2: Supprimer une question\n" +
                                      "3: Retour en arrière");
                    var chose = Console.ReadLine();
                    if (password == null)
                    {
                        Console.WriteLine("Erreur stdin fermé");
                        System.Environment.Exit(-1);
                    }

                    switch (chose)
                    {
                        case "1":
                            questionManager?.AddQestion();
                            break;
                        case "2":
                            await questionManager?.DeleteQuestion()!;
                            break;
                        case "3":
                            break;
                    }
                    
                    break;
                
                case "2":
                    StartQuiz(questionsList);
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
            }
        }

        private static void StartQuiz(List<Questions>? questionsList)
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
    }
}
