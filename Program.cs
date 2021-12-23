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
                var quList = QuestionManager.LoadQuestions(questionsFile);
                var questionManager = new QuestionManager(questionsFile,quList);
                await questionManager.DeleteQuestion(questionsFile,quList);
                questionManager.AddQestion();
            }
            var questionsList = new List<Questions>();
           
            try
            {
                questionsList = QuestionManager.LoadQuestions(questionsFile);
                if (questionsList == null) throw new ArgumentNullException("Le fichier est vide");
                StartQuiz(questionsList);
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
