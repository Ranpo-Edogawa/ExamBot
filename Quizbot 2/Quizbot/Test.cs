using System.Text.Json;
using IronPdf;
//using Newtonsoft.Json;
namespace Quizbot
{
    public class Test
    {
        public string Question { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string CorrectAnswer { get; set; }
    }


    public static class Testlar
    {

        public static string GetTest()
        {
            var stringTest = File.ReadAllText("D:\\New folder (2)\\Quizbot 2\\Quizbot\\Testlar.json");
            var tests = JsonSerializer.Deserialize<List<Test>>(stringTest);


            return GeneratePdfFromTests(tests);
        }

        public static string Get()
        {
            try
            {
                string filePath = "D:\\New folder (2)\\Quizbot 2\\Quizbot\\Testlar.json";

                var stringTest = System.IO.File.ReadAllText(filePath);
                var tests = JsonSerializer.Deserialize<List<Test>>(stringTest) ?? new List<Test>();

                return GeneratePdfFromTests(tests);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "D:\\New folder (2)\\Quizbot 2\\Quizbot\\Testlar.json";
            }
        }

        public static string GeneratePdfFromTests(List<Test> tests)
        {
            try
            {
                IronPdf.License.IsValidLicense("IRONSUITE.UMARHON3005.GMAIL.COM.1795-9E975A51C5-BENURKYJKWPRKSOE-HGJFC6XQ2GDL-27NMQDDQWTQK-EAWOPY6BCQG6-JVWA4AYFAR5B-WSZOPVHMVVSN-O5WVWH-TLPRILOO2ZSLUA-DEPLOYMENT.TRIAL-QL6SWD.TRIAL.EXPIRES.03.MAR.2024");
                if (tests != null && tests.Count > 0)
                {
                    // Generate a PDF document using IronPdf
                    var htmlContent = BuildHtmlContent(tests);
                    var pdf = HtmlToPdf(htmlContent);

                    // Save the PDF file
                    var pdfFilePath = SavePdf(pdf);

                    Console.WriteLine($"PDF file generated successfully at: {pdfFilePath}");

                    return pdfFilePath;
                }
                else
                {
                    Console.WriteLine("No tests found.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        private static string BuildHtmlContent(List<Test> tests)
        {
            // HTML content building logic based on Test class properties
            var htmlContent = "<html><body><h1>Quiz Questions</h1><ul>";

            for (int i = 0; i < tests.Count; i++)
            {
                htmlContent += $"<li><strong>Index:</strong> {i + 1}</li>";
                htmlContent += $"<li><strong>Question:</strong> {tests[i].Question}</li>";
                htmlContent += $"<li><strong>Option A:</strong> {tests[i].A}</li>";
                htmlContent += $"<li><strong>Option B:</strong> {tests[i].B}</li>";
                htmlContent += $"<li><strong>Option C:</strong> {tests[i].C}</li>";
                htmlContent += $"<li><strong>Option D:</strong> {tests[i].D}</li>";
                htmlContent += $"<li><strong>Correct Answer:</strong> {tests[i].CorrectAnswer}</li>";
                htmlContent += "<br>";
            }

            htmlContent += "</ul></body></html>";

            return htmlContent;
        }

        private static PdfDocument HtmlToPdf(string htmlContent)
        {
            var renderer = new IronPdf.HtmlToPdf();
            return renderer.RenderHtmlAsPdf(htmlContent);
        }
        private static string SavePdf(PdfDocument pdf)
        {
            var pdfFilePath = "D:\\New folder (2)\\Quizbot 2\\Quizbot\\QuizPdf.pdf";
            pdf.SaveAs(pdfFilePath);
            return pdfFilePath;
        }



        public static async void Update(List<string> list, int index)
        {
            Delete(index);
            Create(list);
        }


        public static async void Create(List<string> list)
        {
            try
            {
                string filePath = "D:\\New folder (2)\\Quizbot 2\\Quizbot\\Testlar.json";

                string stringTest = await System.IO.File.ReadAllTextAsync(filePath);
                var tests = JsonSerializer.Deserialize<List<Test>>(stringTest) ?? new List<Test>();

                Test test = new Test
                {
                    Question = list[0],
                    A = list[1],
                    B = list[2],
                    C = list[3],
                    D = list[4],
                    CorrectAnswer = list[5]
                };

                tests.Add(test);

                string json1 = JsonSerializer.Serialize(tests);
                await System.IO.File.WriteAllTextAsync(filePath, json1);

                Console.WriteLine("Quiz added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        public static void Delete(int index)
        {
            try
            {
                string filePath = "D:\\New folder (2)\\Quizbot 2\\Quizbot\\Testlar.json";

                string stringTest = File.ReadAllText(filePath);
                List<Test> tests = JsonSerializer.Deserialize<List<Test>>(stringTest) ?? new List<Test>();

                if (index >= 0 && index < tests.Count)
                {
                    tests.RemoveAt(index);

                    string json1 = JsonSerializer.Serialize(tests);
                    File.WriteAllText(filePath, json1);

                    Console.WriteLine("Quiz removed successfully!");
                }
                else
                {
                    Console.WriteLine("Invalid index. No quiz removed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }



        public static List<Test> GetBiologiya()
        {
            string stringTest = System.IO.File.ReadAllText("D:\\New folder (2)\\Quizbot 2\\Quizbot\\General.json");
            var tests = JsonSerializer.Deserialize<List<Test>>(stringTest);

            return tests;
        }
    }
}