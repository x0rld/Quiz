// ReSharper disable All
namespace Quiz
{
    static class Quiz
    {
        static void Main()
        {
            List<Questions> questionsList = new List<Questions>();
            try
            {
                questionsList = LoadQuestions("../../../questions.txt");
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
                res += Questions.AskQuestions(qu) ? 1 : 0 ;
            }
            Console.WriteLine($"vous avez eu {res} bonne réponses sur {questionsList.Count} !");
        }

        private static List<Questions>  LoadQuestions(string path)
        {
            var questionsList = new List<Questions>();
            foreach (string line in File.ReadLines(path))
            {
                var data = line.Split(',');
                if (data.Length <2)
                {
                    throw new Exception("Le fichier doit avoir les lignes sous forme questions,réponse ");
                }
                questionsList.Add(new Questions(data[0],data[1]));
            }
            return questionsList;
        } 
    }
}
