namespace Task_13v
{
    public static class AdvocateUtils
    {
        /// <summary>
        /// Converts an array of strings representing CSV rows into an array of Advocate objects.
        /// </summary>
        /// <param name="rowdata">An array of strings representing CSV rows.</param>
        /// <returns>An array of Advocate objects.</returns>
        /// <exception cref="ArgumentException">Thrown if an invalid argument is encountered during processing.</exception>
        public static CsvUtils.Advocate[] getAdvocatesArray(string[] rowdata)
        {
            // Create a List to store Advocate objects
            List<CsvUtils.Advocate> advocates = new List<CsvUtils.Advocate>();

            // Checking for exceptions.
            try
            {
                // Iterate through each CSV row in the provided array
                foreach (string row in rowdata)
                {
                    // Extract fields from the CSV row using a utility method
                    List<string> data = CsvUtils.DataProcessing.GetFieldsFromLine(row);

                    // Check if valid data is obtained from the CSV row
                    if (data != null && data.Count != 0)
                    {
                        // Create an Advocate object from the extracted data and add it to the list
                        CsvUtils.Advocate advocate = new CsvUtils.Advocate(data.ToArray());
                        advocates.Add(advocate);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }

            // Convert the list of Advocate objects to an array and return
            return advocates.ToArray();
        }

        /// <summary>
        /// Generates a selection of Advocate objects based on a specified field value.
        /// </summary>
        /// <param name="advocates">An array of Advocate objects to filter.</param>
        /// <param name="selectValue">The field value for which the selection is made.</param>
        /// <returns>An array of filtered Advocate objects.</returns>
        public static CsvUtils.Advocate[] GenerateSelection(CsvUtils.Advocate[] advocates, string selectValue)
        {
            /* 
                You need to enter specific values of fields: 
                "Приостановление статуса Реквизиты решения совета адвокатской палаты о приостоновлении статуса адвоката" --> Example: №72 от 18.03.2003 (1 line in the file).
                You can also filter by complex strings --> Example: №3507 от 24.12.2009  №11 от 20.10.2010 (334 line in the file).
                or 
                "Возобновление статуса Реквизиты решения совета адвокатской палаты о возобновлении статуса адвоката" --> Example: №1370 от 23.11.2003 (1 line in the file).
                The method does not filter on empty values ​​because there is no point in doing so.
            */

            // Prompt the user to enter a specific value for the selected field to create a filter.
            string? userValue;
            while (true)
            {
                Console.WriteLine($"Введите конкретное значение поля {selectValue} для организации фильтра:");
                userValue = Console.ReadLine();

                // Check that the entered value is not empty and is not null.
                if (!string.IsNullOrEmpty(userValue))
                {
                    break;
                }

                Console.WriteLine("Пустое значение поля, повторите ввод.");
            }

            // Create a list to store the selected Advocate objects.
            List<CsvUtils.Advocate> selectedAdvocates = new List<CsvUtils.Advocate>();

            // Iterate through each Advocate in the provided array.
            foreach (CsvUtils.Advocate advocate in advocates)
            {
                if (selectValue == "Приостановление статуса адвоката")
                {
                    // Calling a method that checks whether a class field matches the passed value.
                    if (EqualityCheck(advocate, 2, userValue))
                    {
                        selectedAdvocates.Add(advocate);
                    }
                }
                else
                {
                    // Calling a method that checks whether a class field matches the passed value.
                    if (EqualityCheck(advocate, 4, userValue))
                    {
                        selectedAdvocates.Add(advocate);
                    }
                }
            }

            // Convert the list of selected Advocate objects to an array and return.
            return selectedAdvocates.ToArray();
        }

        /// <summary>
        /// Method that checks the filtering result.
        /// </summary>
        /// <param name="selected">Filter result.</param>
        /// <returns>Correctness of the filtering result.</returns>
        public static bool ProccessSelection(CsvUtils.Advocate[] selected)
        {
            // Checking that the filtering result is not empty and not equal to null.
            if (selected == null || selected.Length == 0)
            {
                ConsoleUtils.PrintBeautyError("Результат выборки пуст.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// A method that returns a string consisting of all lines of a file separated by a newline delimiter.
        /// </summary>
        /// <param name="advocates">The passed instance of the Advocate class.</param>
        /// <returns>A string consisting of all lines of a file separated by a newline delimiter.</returns>
        public static string GetСoncatenatedAdvocatesInfoString(CsvUtils.Advocate[] advocates)
        {
            // Empty storage string.
            string concatenated = "";

            foreach (CsvUtils.Advocate advocate in advocates)
            {
                // Calling a class method to conveniently turn class fields into a string.
                concatenated += advocate.ToString() + "\n";
            }

            return concatenated;
        }

        /// <summary>
        /// A method that returns a string array consisting of all lines of a file.
        /// </summary>
        /// <param name="advocates">The passed instance of the Advocate class.</param>
        /// <returns>A string array consisting of all lines of a file.</returns>
        public static string[] GetСoncatenatedAdvocatesInfoArray(CsvUtils.Advocate[] advocates)
        {
            // Create a List to store strings. 
            List<string> concatenated = new List<string>();

            foreach (CsvUtils.Advocate advocate in advocates)
            {
                // Calling a class method to conveniently turn class fields into a string. And adding it into a list.
                concatenated.Add(advocate.ToString());
            }

            return concatenated.ToArray();
        }

        /// <summary>
        /// A method that checks whether a class field matches the passed value.
        /// </summary>
        /// <param name="advocate">The passed instance of the Advocate class.</param>
        /// <param name="index">Index of the required field.</param>
        /// <param name="userValue">A specific value for the selected field to create a filter.</param>
        /// <returns></returns>
        private static bool EqualityCheck(CsvUtils.Advocate advocate, int index, string userValue)
        {
            // Checking that a field with such an index exists in the structure.
            if (advocate.Requisites.Length > index)
            {
                // Calling a special method that processes complex strings containing numbers and dates ("Реквизиты"). 
                string[] numbersDates = CsvUtils.DataProcessing.GetNumbersAndDates(userValue);
                /*
                    The method does not check empty arrays not containing dates and numbers and empty strings, since this makes no sense.  
                */
                if (numbersDates.Length != 0 && numbersDates is not null)
                {
                    // Create a Lists to store numbers and dates.
                    List<string> numbers = new List<string>();
                    List<string> dates = new List<string>();
                    // Loop through all elements of the resulting array with numbers and dates (it is not ordered).
                    foreach (string el in numbersDates)
                    {
                        // Checking whether a string is a number or a number.
                        if (el.Contains('№'))
                        {
                            numbers.Add(el.Trim());
                        }
                        else
                        {
                            dates.Add(el.Trim());
                        }
                    }
                    // Comparing the resulting arrays element by element, if they are equal, then the class structure contains such a field.
                    if (advocate.Requisites[index].Number.SequenceEqual(numbers) && advocate.Requisites[index].Date.SequenceEqual(dates))
                    {
                        return true;
                    }
                }
            }

            // If there is no such field in the class structure.
            return false;
        }
    }
}