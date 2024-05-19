using System.Text.RegularExpressions;

namespace CsvUtils
{
    public static class DataProcessing
    {
        /// <summary>
        /// A method that adds or removes fields from a string in accordance with the passed number of fields in the string.
        /// </summary>
        /// <param name="rowData">The passed array of file fields.</param>
        /// <param name="fieldsInLine">The passed number of fields in the line.</param>
        /// <returns>Augmented lines array.</returns>
        /// <exception cref="ArgumentException">The exception that the method throws when the passed array is empty or null or when fieldsInLine <= 0.</exception>
        public static string[] GetFixedRowData(string[] rowData, int fieldsInLine)
        {
            // Checking that the transferred array is not empty.
            if (rowData.Length == 0 || rowData is null)
            {
                throw new ArgumentException("Передан пустой массив.");
            }

            // Checking that the number of header fields is greater than zero.
            if (fieldsInLine <= 0)
            {
                throw new ArgumentException("Количество полей заголовка меньше нуля или равно нулю.");
            }

            // Using the abstract List data type to easily store and add new rows.
            List<string> fixedRowData = new List<string>();
            // Loop through an array of rows.
            foreach (string row in rowData)
            {
                /* 
                    Calling a method that divides a string into fields according to certain rules (described in the method). 
                    Using the abstract List data type to easily store and add new fields.
                */
                List<string> fields = GetFieldsFromLine(row);
                // Number of fields in list.
                int fieldsNumber = fields.Count;

                /* 
                    The file header contains 16 fields, but the next lines of the file have a different number of fields (18, somewhere more or less).
                    It was decided to add or remove the last fields of the rows (they do not affect sorting and filters, so there will be no data loss). 
                    So that the number of fields in the row corresponds to the number of header fields (16 fields).
                    Example (18 fileds) -> (16 fields):
                    77/1,Мирзоев,Гасан,Борисович,№ от 08.11.1993,№1109-а от 10.09.2002,№72 от 18.03.2003 ,№39-а от 01.04.2003          ,№1370 от 23.11.2003 ,№01-а от 08.01.2004 ,,,,,,,,приост.ст.,
                    After processing -> ...
                    77/1,Мирзоев,Гасан,Борисович,№ от 08.11.1993,№1109-а от 10.09.2002,№72 от 18.03.2003 ,№39-а от 01.04.2003          ,№1370 от 23.11.2003 ,№01-а от 08.01.2004 ,,,,,,,
                    Similar to the situation when empty fields are added to the end.
                    Example (14 fileds) -> (16 fields).
                */

                // Separate handling of situations when the number of fields in a line is less or more than necessary.
                if (fieldsNumber < fieldsInLine)
                {
                    // Difference between number of fields in row and header fields.
                    int diff = fieldsInLine - fieldsNumber;
                    // Adding empty fields to a row.
                    for (int i = 0; i < diff; i++)
                    {
                        fields.Add("");
                    }
                }
                else if (fieldsNumber > fieldsInLine)
                {
                    // Difference between header fields and number of fields in row.
                    int diff = fieldsNumber - fieldsInLine;
                    // Removing extra fields from a row.
                    for (int i = 0; i < diff; i++)
                    {
                        fields.RemoveAt(fields.Count - 1);
                    }
                }

                // Adding corrected lines to list (reduce to original format with commas).
                fixedRowData.Add(string.Join(",", fields.ToArray()) + ",");
            }

            return fixedRowData.ToArray();
        }

        /// <summary>
        /// A method that divides a string into fields according to certain rules.
        /// </summary>
        /// <param name="line">Passed string.</param>
        /// <returns>List consisting of fields in a row.</returns>
        /// <exception cref="ArgumentException">The exception that the method throws when the passed string is empty or equal to null.</exception>
        public static List<string> GetFieldsFromLine(string line)
        {
            // Checking that the string is not empty and not equal to null.
            if (string.IsNullOrEmpty(line))
            {
                throw new ArgumentException("Передана пустая строка.");
            }

            // Using the abstract List data type to easily store and add new fields.
            List<string> fields = new List<string>();
            // A string intended to store the current word without a comma.
            string word = "";
            // A boolean variable that determines whether the character is in quotes.
            bool inQuotes = false;

            /*
                The file contains many strange lines, so the following solutions were taken to normalize the data.    
                It is possible that a comma is in quotation marks and is not a separator; this is processed separately.
                Example:
                ...,"КА ""Орлов, Вербицкий, Добровольский и партнеры""",...
                Almost all such lines are in the last fields of the line, so they are highly likely to be deleted by the method above.
            */

            // Loop through all characters of a string.
            foreach (char ch in line)
            {
                // Changing state when the character is a quote. In other cases, the line is either filled in or nulled and added as a new field to the list.
                if (ch == '\"')
                {
                    inQuotes = !inQuotes;
                }
                else if (!inQuotes && ch == ',')
                {
                    fields.Add(word);
                    word = "";
                }
                else
                {
                    word += ch;
                }
            }

            return fields;
        }

        /// <summary>
        /// A method that processes complex strings containing numbers and dates ("Реквизиты").
        /// </summary>
        /// <param name="str">Passed string.</param>
        /// <returns>An array containing numbers and dates.</returns>
        public static string[] GetNumbersAndDates(string str)
        {
            /* 
                Since the file contains fields containing several numbers and dates at once.
                It is necessary to use regular expressions to handle such situations.
                Examples:
                field - №6 от 13.02.2003 №9 от 25.09.2007 ---> [№6, 13.02.2003, №9, 25.09.2007].
                field - №6 от 13.02.2003 ---> [№6, 13.02.2003].
                field - № от 13.02.2003 ---> [№, 13.02.2003].
                field - №3-а от 08.11.1993 №2-а от 08.11.1993 ---> [№3-а, 08.11.1993, №2-а, 08.11.1993].
                ...
            */

            // Using a regular expression to search for "№" and dates.
            MatchCollection matches;
            if (str.Contains('-'))
            {
                matches = Regex.Matches(str, @"№\d+-[а-яА-Я]?|\d{2}\.\d{2}\.\d{4}");
            }
            else
            {
                matches = Regex.Matches(str, @"№\d*(?:\D|$)|\d{2}\.\d{2}\.\d{4}");
            }

            // Converting the result to an array of strings.
            string[] parts = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                parts[i] = matches[i].Value;
            }

            return parts;
        }
    }
}