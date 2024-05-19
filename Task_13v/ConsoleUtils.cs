namespace Task_13v
{
    public static class ConsoleUtils
    {
        /// <summary>
        /// A method that outputs a red - marked error.
        /// </summary>
        /// <param name="errorMessage">Transmitted error message.</param>
        public static void PrintBeautyError(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ResetColor();
        }

        /// <summary>
        /// A method that gives the user the ability to select a sort or filter.
        /// </summary>
        /// <param name="advocates">An array of Advocate objects.</param>
        public static void ChooseSortFilter(CsvUtils.Advocate[] advocates)
        {
            // Entering the menu item number of the user's choice.
            int n;
            while (true)
            {
                // Displaying the main user menu.
                Console.Write("1. Произвести сортировку по значению \"Фамилия\" в алфавитном порядке\r\n2. Произвести сортировку по значению \"Фамилия\" в обратном алфавитном порядке\r\n3. Произвести фильтр по значению \"Приостановление статуса адвоката\"\r\n4. Произвести фильтр по значению \"Возобновление статуса адвоката\"\r\n");
                Console.Write("Укажите номер пункта меню для запуска действия: ");

                // Checking the correctness of the entered data.
                if (int.TryParse(Console.ReadLine(), out n) && n >= 1 && n <= 4)
                {
                    break;
                }

                // Calling a method that outputs a red-marked error.
                PrintBeautyError("Неизвестная команда, повторите ввод.");
            }

            // An array to store the filtering result.
            CsvUtils.Advocate[] selected;

            switch (n)
            {
                case 1:
                    // Classic alphabetical sorting in ascending order.
                    Array.Sort(advocates);
                    // Calling a method that displays the sorting result on the screen.
                    DisplaySortResult(advocates, 4);
                    // Calling a method that allows the user to select the type of saving data to a file.
                    ChooseSaveOption(advocates);
                    break;
                case 2:
                    // Reverse sort in descending order.
                    CsvUtils.Advocate.SetSortDirection(false);
                    Array.Sort(advocates);
                    // Calling a method that displays the sorting result on the screen.
                    DisplaySortResult(advocates, 4);
                    // Calling a method that allows the user to select the type of saving data to a file.
                    ChooseSaveOption(advocates);
                    break;
                case 3:
                    // Calling a method that generates a filtered array of class instances by value.
                    selected = AdvocateUtils.GenerateSelection(advocates, "Приостановление статуса адвоката");
                    // Calling a method that checks the filtering result.
                    if (AdvocateUtils.ProccessSelection(selected))
                    {
                        // Calling a method that displays the filtering result on the screen.
                        DisplaySelectionResult(selected, 7);
                        // Calling a method that allows the user to select the type of saving data to a file.
                        ChooseSaveOption(selected);
                    }
                    break;
                case 4:
                    // Calling a method that generates a filtered array of class instances by value.
                    selected = AdvocateUtils.GenerateSelection(advocates, "Возобновление статуса адвоката");
                    // Calling a method that checks the filtering result.
                    if (AdvocateUtils.ProccessSelection(selected))
                    {
                        // Calling a method that displays the filtering result on the screen.
                        DisplaySelectionResult(selected, 9);
                        // Calling a method that allows the user to select the type of saving data to a file.
                        ChooseSaveOption(selected);
                    }
                    break;
            }
        }

        /// <summary>
        /// Allows the user to choose between displaying the top or bottom records from an array of advocates.
        /// </summary>
        /// <param name="advocates">An array of Advocate objects.</param>
        public static void ChooseTopBottom(CsvUtils.Advocate[] advocates)
        {
            while (true)
            {
                Console.Write("Выберите тип вывода строк (0 - Top, 1 - Bottom): ");

                // Read user input for the selection type.
                int selectionType;
                if (!int.TryParse(Console.ReadLine(), out selectionType))
                {
                    // Calling a method that outputs a red - marked error.
                    PrintBeautyError("Некорректный выбор. Повторите попытку.");
                    continue;
                }

                // Convert the numeric selection to the corresponding RecordSelection enumeration value.
                RecordSelection recordSelection = (RecordSelection)selectionType;
                int n;

                switch (recordSelection)
                {
                    case RecordSelection.Top:
                        // Prompt the user for the number of records to display from the top.
                        n = EnterNumberOfElements(advocates.Length);
                        // Display the selected number of records from the top.
                        DisplayTopRecords(advocates, n);
                        break;
                    case RecordSelection.Bottom:
                        // Prompt the user for the number of records to display from the bottom.
                        n = EnterNumberOfElements(advocates.Length);
                        // Display the selected number of records from the bottom.
                        DisplayBottomRecords(advocates, n);
                        break;
                    default:
                        // Calling a method that outputs a red - marked error.
                        PrintBeautyError("Некорректный выбор. Повторите попытку.");
                        continue;
                }

                break;
            }
        }

        /// <summary>
        /// A method that displays the sorting result on the screen.
        /// </summary>
        /// <param name="advocates">Sorting result.</param>
        /// <param name="index">Array index up to which output is needed.</param>
        private static void DisplaySortResult(CsvUtils.Advocate[] advocates, int index)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Данные успешно отсортированы!\nДля экономии места в консоли будут выведены только некоторые поля и половина строк:");
            Console.ResetColor();
            // Output only some fields and half the rows to save console space.
            foreach (CsvUtils.Advocate advocate in advocates[..((advocates.Length / 2) + 1)])
            {
                // Extract fields from the advocate's ToString representation.
                List<string> fields = CsvUtils.DataProcessing.GetFieldsFromLine(advocate.ToString());
                // Display a subset of fields, limiting to the specified index.
                Console.WriteLine(string.Join(",", fields.ToArray()[..index]) + ",...");
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.ResetColor();
        }

        /// <summary>
        /// A method that displays the filtering result on the screen.
        /// </summary>
        /// <param name="selected">Filtering result.</param>
        /// <param name="index">Array index up to which output is needed.</param>
        private static void DisplaySelectionResult(CsvUtils.Advocate[] selected, int index)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Фильтрация выполнена успешно!\nДля экономии места в консоли будут выведены только некоторые поля:");
            Console.ResetColor();
            // Filtering result is not so huge, printing all lines.
            foreach (CsvUtils.Advocate advocate in selected)
            {
                // Extract fields from the advocate's ToString representation.
                List<string> fields = CsvUtils.DataProcessing.GetFieldsFromLine(advocate.ToString());
                // Display a subset of fields, limiting to the specified index.
                Console.WriteLine(string.Join(",", fields.ToArray()[..index]) + ",...");
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.ResetColor();
        }

        /// <summary>
        /// Enumeration representing options for record selection, indicating whether to choose records from the top or bottom of a collection.
        /// </summary>
        private enum RecordSelection
        {
            Top,
            Bottom
        }

        /// <summary>
        /// Allows the user to choose a save option for the processed advocate data.
        /// </summary>
        /// <param name="advocates">An array of Advocate objects.</param>
        private static void ChooseSaveOption(CsvUtils.Advocate[] advocates)
        {
            // Entering the menu item number of the user's choice.
            while (true)
            {
                // Displaying the user menu.
                Console.WriteLine("1. Создание нового файла и запись\r\n2. Замена содержимого уже существующего файла\r\n3. Добавление сохраняемых данных к содержимому существующего файла");
                Console.Write("Укажите номер пункта меню для запуска действия: ");

                int n;
                if (!int.TryParse(Console.ReadLine(), out n) || n < 1 || n > 3)
                {
                    // Calling a method that outputs a red - marked error.
                    PrintBeautyError("Неизвестная команда, повторите ввод.");
                    continue;
                }

                // Prompt the user to enter the absolute path for saving the result.
                Console.Write("Введите абсолютный путь к файлу для сохранения результата. Пример ввода: MyFile.csv или D:\\testCSV\\MyFile.csv или другой путь (зависит от ОС): ");
                string? nPath = Console.ReadLine();

                string[] concatenatedArray;
                switch (n)
                {
                    case 1:
                        // Concatenate advocate information and create a new file.
                        concatenatedArray = AdvocateUtils.GetСoncatenatedAdvocatesInfoArray(advocates);
                        // Checking for exceptions.
                        try
                        {
                            CsvUtils.CsvProcessing.Write(concatenatedArray, nPath);
                        }
                        catch (Exception ex)
                        {
                            // Calling a method that outputs a red - marked error.
                            PrintBeautyError(ex.Message);
                            continue;
                        }
                        break;
                    case 2:
                        // Concatenate advocate information and replace the content of an existing file.
                        concatenatedArray = AdvocateUtils.GetСoncatenatedAdvocatesInfoArray(advocates);
                        // Checking for exceptions.
                        try
                        {
                            // rewrite to existed file
                            CsvUtils.CsvProcessing.Write(concatenatedArray, nPath, 0);
                        }
                        catch (Exception ex)
                        {
                            // Calling a method that outputs a red - marked error.
                            PrintBeautyError(ex.Message);
                            continue;
                        }
                        break;
                    case 3:
                        // Concatenate advocate information and append to the content of an existing file.
                        string concatenatedString = AdvocateUtils.GetСoncatenatedAdvocatesInfoString(advocates);
                        // Checking for exceptions.
                        try
                        {
                            // add to existed file
                            CsvUtils.CsvProcessing.Write(concatenatedString, nPath);
                        }
                        catch (Exception ex)
                        {
                            // Calling a method that outputs a red - marked error.
                            PrintBeautyError(ex.Message);
                            continue;
                        }
                        break;
                    default:
                        // Calling a method that outputs a red - marked error.
                        PrintBeautyError("Неизвестная команда, повторите ввод.");
                        continue;
                }

                break;
            }
        }

        /// <summary>
        /// Prompts the user to enter the number of records to be displayed.
        /// </summary>
        /// <param name="advocatesLength">The total number of available records.</param>
        /// <returns>The user-specified number of records to be displayed.</returns>
        private static int EnterNumberOfElements(int advocatesLength)
        {
            int n;

            while (true)
            {
                // Display a recommendation for stable console performance.
                Console.Write($"Рекомендуется выводить до 9000 записей для стабильной работы консоли!\nВведите количество записей 0 < N <= {advocatesLength}, которые будут выведены: ");

                if (int.TryParse(Console.ReadLine(), out n) && n > 0 && n <= advocatesLength)
                {
                    break;
                }

                // Calling a method that outputs a red - marked error.
                PrintBeautyError("Неккоректные данные, повторите ввод.");
            }

            return n;
        }

        /// <summary>
        /// Displays the top records of Advocate objects based on the specified count.
        /// </summary>
        /// <param name="advocates">An array of Advocate objects.</param>
        /// <param name="n">The number of top records to display.</param>
        private static void DisplayTopRecords(CsvUtils.Advocate[] advocates, int n)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Для экономии места в консоли будут выведены только первые 5 полей строки.");
            Console.ResetColor();

            // Display the header for the top records section.
            Console.WriteLine($"Top {n} записей:");

            // Iterate through the top records and display a subset of fields for each record.
            for (int i = 0; i < n; i++)
            {
                List<string> fields = CsvUtils.DataProcessing.GetFieldsFromLine(advocates[i].ToString());
                Console.WriteLine(string.Join(",", fields.ToArray()[..5]) + ",...");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.ResetColor();
        }

        /// <summary>
        /// Displays the bottom records of Advocate objects based on the specified count.
        /// </summary>
        /// <param name="advocates">An array of Advocate objects.</param>
        /// <param name="n">The number of bottom records to display.</param>
        private static void DisplayBottomRecords(CsvUtils.Advocate[] advocates, int n)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Для экономии места в консоли будут выведены только первые 5 полей строки.");
            Console.ResetColor();

            // Display the header for the bottom records section.
            Console.WriteLine($"Bottom {n} записей:");

            // Iterate through the bottom records and display a subset of fields for each record.
            for (int i = advocates.Length - n; i < advocates.Length; i++)
            {
                List<string> fields = CsvUtils.DataProcessing.GetFieldsFromLine(advocates[i].ToString());
                Console.WriteLine(string.Join(",", fields.ToArray()[..5]) + ",...");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.ResetColor();
        }
    }
}
