/*
   Студент:     Агафонов Никита Максимович    
   Группа:      БПИ234
   Вариант:     13
   Дата:        15.12.2023
*/
namespace Task_13v
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();

                // User input of absolute file path.
                Console.Write("Введите абсолютный путь к файлу с csv-данными: ");
                string? path = Console.ReadLine();

                // Assigning the class field a user-entered file name.
                CsvUtils.CsvProcessing.FPath = path;

                // Declaring an empty reference to an array of strings in a file.
                string[]? rowData = null;

                // Сhecking for exceptions.
                try
                {
                    // Calling a method to validate and read a user-supplied csv file.
                    rowData = CsvUtils.CsvProcessing.Read();
                }
                catch (Exception ex)
                {
                    // Calling a method that outputs a red - marked error.
                    ConsoleUtils.PrintBeautyError(ex.Message);
                }

                // Сhecking that the returned array is not null and not empty.
                if (rowData is not null && rowData.Length != 0)
                {
                    try
                    {
                        // Calling a method that returns an array of class instances and passing a reference to it.
                        CsvUtils.Advocate[] advocates = AdvocateUtils.getAdvocatesArray(rowData[1..]);
                        // Calling a method that gives the user the choice of outputting the top and bottom margins from the file.
                        ConsoleUtils.ChooseTopBottom(advocates);
                        // Calling a method that gives the user the ability to select a sort or filter.
                        ConsoleUtils.ChooseSortFilter(advocates);
                    }
                    catch (Exception ex)
                    {
                        // Calling a method that outputs a red - marked error.
                        ConsoleUtils.PrintBeautyError(ex.Message);
                    }
                }

                // Reseting fpath value.
                CsvUtils.CsvProcessing.FPath = string.Empty;

                // Repeating the solution at the user's request.
                Console.Write("Для выхода из программы нажмите клавишу ESC, для перезапуска программы нажмите любую другую клавишу: ");
                if (Console.ReadKey(true).Key == ConsoleKey.Escape) { break; }
            }
        }

    }
}