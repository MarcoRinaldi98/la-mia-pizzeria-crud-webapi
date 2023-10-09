using System.Text.RegularExpressions;

namespace la_mia_pizzeria_static.CustomLoggers
{
    public class CustomFileLogger : ICustomLogger
    {
        public void WriteLog(string message)
        {
            // File che terrà conto delle LOG
            File.AppendAllText("C:\\Users\\Marco\\source\\repos\\la-mia-pizzeria-crud-mvc\\la-mia-pizzeria-static\\my-log.txt", $"{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} LOG: {message}\n");
        }
    }
}
