namespace CsvUtils
{
    public static class CsvProcessing
    {
        // Static field that stores the path to the file.
        private static string _fPath = string.Empty;
        // Static readonly field that store the first line of the file.
        private readonly static string _firstLine = "Реестровый номер,Фамилия,Имя,Отчество,Присвоение статуса Реквизиты решения о присвоении статуса адвоката,Реквизиты распоряжения территориального органа Минюста России о внесении сведений об адвокате в реестр,Приостановление статуса Реквизиты решения совета адвокатской палаты о приостоновлении статуса адвоката,Реквизиты распоряжения территориального органа Минюста России о внесении  в реестр сведений о приостоновлении статуса адвоката,Возобновление статуса Реквизиты решения совета адвокатской палаты о возобновлении статуса адвоката,Реквизиты распоряжения территориального органа Минюста России о внесении  в реестр сведений о возобновлении,Прекращение статуса Реквизиты решения совета адвокатской палаты о прекращении статуса адвоката,Реквизиты распоряжения территориального органа Минюста России о внесении  в реестр сведений о прекращении статуса,Исключение сведений об адвокате из реестра Реквизиты уведомления совета адвокатской палаты,Реквизиты распоряжения территориального органа Минюста России об исключении сведений об адвокате,Внесение изменений Содержание изменений,Реквизиты распоряжения территориального органа Минюста России о внесении в реестр изменений,";
        // Static readonly field that store the number of fields per file line.
        private readonly static int _fieldsInLine = 16;

        // Assignment property and getting the file path.
        public static string FPath
        {
            get { return _fPath; }
            set
            {
                // Сhecking that the passed path to the file is not null and not empty.
                if (!string.IsNullOrEmpty(value))
                {
                    _fPath = value;
                }
                else
                {
                    // To reset.
                    _fPath = string.Empty;
                }
            }
        }

        /// <summary>
        /// A method that reads a csv file and checks its compliance with the variant.
        /// </summary>
        /// <returns>An array of strings read from a file.</returns>
        /// <exception cref="ArgumentException">The exception that the method throws when the file path is incorrectly specified.</exception>
        /// <exception cref="IOException">The exception that the method throws when the file is opened incorrectly and the structure is written.</exception>
        /// <exception cref="Exception">The exception that a method throws when unexpected errors occur.</exception> 
        public static string[] Read()
        {
            string[] rowData;

            // Сhecking that the file exists.
            if (!File.Exists(_fPath))
            {
                throw new ArgumentException("Файл с таким названием не существует.");
            }

            // Сhecking for exceptions.
            try
            {
                // Reading data from a file.
                rowData = File.ReadAllLines(_fPath);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Введено некорректное название файла.");
            }
            catch (IOException ex)
            {
                throw new IOException("Возникла ошибка при открытии файла и записи структуры.");
            }
            catch (Exception ex)
            {
                throw new Exception("Возникла непредвиденная ошибка.");
            }

            // Calling a method that checks the structure of the transferred file.
            CheckFileStructure(rowData);
            // Calling a method that adds or removes extra fields from file lines (why this is done is explained in the method).
            string[] result = DataProcessing.GetFixedRowData(rowData, _fieldsInLine);

            return result;
        }

        /// <summary>
        /// A method that checks the structure of the transmitted file.
        /// </summary>
        /// <param name="rowData">An array of strings read from a file.</param>
        /// <exception cref="ArgumentException">An exception that is thrown when the file structure is violated.</exception>
        private static void CheckFileStructure(string[] rowData)
        {
            // Checking that the transferred file is not empty.
            if (rowData.Length == 0 || rowData is null)
            {
                throw new ArgumentException("Передан пустой файл.");
            }

            // Checking that the first line of the transferred file corresponds to the first line of the variant file.
            if (rowData[0] != _firstLine)
            {
                throw new ArgumentException("Заголовки переданного csv файла не соответствуют заголовкам варианта.");
            }
        }

        /// <summary>
        /// A method that creates a new file and adds info in.
        /// </summary>
        /// <param name="lines">Lines that need to be written to the file.</param>
        /// <param name="nPath">The path to the file.</param>
        /// <exception cref="ArgumentException">The exception that the method throws when the file path is incorrectly specified.</exception>
        /// <exception cref="IOException">The exception that the method throws when the file is opened incorrectly and the structure is written.</exception>
        /// <exception cref="Exception">The exception that a method throws when unexpected errors occur.</exception>
        public static void Write(string[] lines, string nPath)
        {
            // Checking if the file extension is csv.
            if (!nPath.Contains(".csv"))
            {
                throw new ArgumentException("Введено некорректное название для файла.");
            }

            // Checking that a file with the same name exists.
            if (File.Exists(nPath))
            {
                throw new ArgumentException("Файла с таким названием уже существует.");
            }

            // Сhecking for exceptions.
            try
            {
                // Adding headers as the first line in the file. All data that was in the file is deleted.
                File.WriteAllText(nPath, _firstLine + "\n");
                // Adding all lines after the header to a file.
                File.AppendAllLines(nPath, lines);
                // Successful save message.
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Данные успешно записаны в файл.");
                Console.ResetColor();
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Введено некорректное название для файла.");
            }
            catch (IOException ex)
            {
                throw new IOException("Возникла ошибка при открытии файла и записи структуры.");
            }
            catch (Exception ex)
            {
                throw new Exception("Возникла непредвиденная ошибка.");
            }
        }

        /// <summary>
        ///A method that rewrites lines with data to an existing file csv file.
        /// </summary>
        /// <param name="lines">Lines that need to be written to the file.</param>
        /// <param name="nPath">The path to the file.</param>
        /// <exception cref="ArgumentException">The exception that the method throws when the file path is incorrectly specified.</exception>
        /// <exception cref="IOException">The exception that the method throws when the file is opened incorrectly and the structure is written.</exception>
        /// <exception cref="Exception">The exception that a method throws when unexpected errors occur.</exception>
        public static void Write(string[] lines, string nPath, int _)
        {
            // Checking if the file extension is csv.
            if (!nPath.Contains(".csv"))
            {
                throw new ArgumentException("Введено некорректное название для файла.");
            }

            // Checking that a file with the same name exists.
            if (!File.Exists(nPath))
            {
                throw new ArgumentException("Файла с таким названием не существует.");
            }

            // Сhecking for exceptions.
            try
            {
                // Adding headers as the first line in the file. All data that was in the file is deleted.
                File.WriteAllText(nPath, _firstLine + "\n");
                // Adding all lines after the header to a file.
                File.AppendAllLines(nPath, lines);
                // Successful save message.
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Данные успешно записаны в файл.");
                Console.ResetColor();
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Введено некорректное название для файла.");
            }
            catch (IOException ex)
            {
                throw new IOException("Возникла ошибка при открытии файла и записи структуры.");
            }
            catch (Exception ex)
            {
                throw new Exception("Возникла непредвиденная ошибка.");
            }
        }

        /// <summary>
        /// A method that adds a line with data to an existing file csv file.
        /// </summary>
        /// <param name="line">The line to be added to the csv file.</param>
        /// <param name="nPath">The path to the file.</param>
        /// <exception cref="ArgumentException">The exception that the method throws when the file path is incorrectly specified or file does not exist.</exception>
        /// <exception cref="IOException">The exception that the method throws when the file is opened incorrectly and the structure is written.</exception>
        /// <exception cref="Exception">The exception that a method throws when unexpected errors occur.</exception>
        public static void Write(string line, string nPath)
        {
            // Checking if the file extension is csv.
            if (!nPath.Contains(".csv"))
            {
                throw new ArgumentException("Введено некорректное название для файла.");
            }

            // Checking that a file with the same name exists.
            if (!File.Exists(nPath))
            {
                throw new ArgumentException("Файла с таким названием не существует.");
            }

            // Сhecking for exceptions.
            try
            {
                // Adding a line with data to a file.
                File.AppendAllText(nPath, line);
                // Successful save message.
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Данные успешно записаны в файл.");
                Console.ResetColor();
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Введено некорректное название для файла.");
            }
            catch (IOException ex)
            {
                throw new IOException("Возникла ошибка при открытии файла и записи структуры.");
            }
            catch (Exception ex)
            {
                throw new Exception("Возникла непредвиденная ошибка.");
            }
        }
    }
}