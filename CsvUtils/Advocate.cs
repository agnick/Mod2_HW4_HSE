namespace CsvUtils
{
    public class Advocate : IComparable<Advocate>
    {
        // Field storing an array of string fields.
        private string[] _data;
        // Field storing an array of the Requisites structure.
        private Requisites[] _requisites;
        // Field storing: "1. Реестровый номер".
        private string _registrationNumber;
        // Field storing: "2. Фамилия".
        private string _lastName;
        // Field storing: "3. Имя".
        private string _name;
        // Field storing: "4. Отчество".
        private string _surName;
        // Field storing a sort direction: 1 for forward, -1 for reverse.
        private static int _sortDirection = 1;
        // Static readonly field that store the number of fields in line.
        private readonly static int fieldsInLine = 16;

        // Properties for reading class fields.
        public string[] Data => _data;
        public Requisites[] Requisites => _requisites;
        public string RegistrationNumber => _registrationNumber;
        public string LastName => _lastName;
        public string Name => _name;
        public string SurName => _surName;

        // Empty constructor with no parameters.
        public Advocate()
        {
            _data = Array.Empty<string>();
            _requisites = Array.Empty<Requisites>();
            _registrationNumber = string.Empty;
            _lastName = string.Empty;
            _name = string.Empty;
            _surName = string.Empty;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="info">Passed array of string fields.</param>
        /// <exception cref="ArgumentException">Exception that is thrown when data is invalid.</exception>
        public Advocate(string[] info)
        {
            // Checking that the passed array of fields is not null and its length corresponds to the number of fields in the string.
            if (info == null || info.Length != fieldsInLine)
            {
                throw new ArgumentException("Ошибка в конструкторе.");
            }

            // Storing an array of string fields.
            _data = info;
            // Assigning to class fields the corresponding data from the passed array of fields. Only those fields that do not contain "Реквизиты".
            _registrationNumber = info[0];
            _lastName = info[1];
            _name = info[2];
            _surName = info[3];

            // Adding other fields to the requisites field that contain "Реквизиты" (all fields after "Отчество").
            // Using the abstract List data type to easily store and add new fields.
            List<Requisites> req = new List<Requisites>();
            // Loop through all other fields after "Отчество".
            for (int i = 4; i < info.Length; i++)
            {
                // Checking that the field is not empty and not equal to null.
                if (string.IsNullOrEmpty(info[i]))
                {
                    // Adding an empty Requisites structure to a list.
                    req.Add(new Requisites());
                }

                // Calling a special method that processes complex strings containing numbers and dates ("Реквизиты"). Why this is done is explained in the method.
                string[] complexInfo = DataProcessing.GetNumbersAndDates(info[i]);
                // Using the abstract List data type to easily store and add new numbers.
                List<string> numbers = new List<string>();
                // Using the abstract List data type to easily store and add new dates.
                List<string> dates = new List<string>();
                // Loop through all elements of an array.
                foreach (string el in complexInfo)
                {
                    // If the element contains '№', then it is a number; if not, then it is a date.
                    if (el.Contains('№'))
                    {
                        numbers.Add(el.Trim());
                    }
                    else
                    {
                        dates.Add(el.Trim());
                    }
                }

                // Adding a new structure to the list of structures.
                req.Add(new Requisites(numbers.ToArray(), dates.ToArray()));
            }

            // Assigning the created array of structures to the _requisites field.
            _requisites = req.ToArray();
        }

        /// <summary>
        /// Required Icomparable interface method for comparing classes by the LastName field.
        /// </summary>
        /// <param name="other">Other Advocate class.</param>
        /// <returns>Comparison result.</returns>
        public int CompareTo(Advocate other)
        {
            // If the object being compared is null, then the current one is greater.
            if (other == null)
            {
                return 1;
            }

            // Comparing by last name.
            // If both objects have empty last names, we consider them equal.
            if (string.IsNullOrEmpty(this.LastName) && string.IsNullOrEmpty(other.LastName))
            {
                return 0;
            }

            // The empty last name of the current object is greater than the non-empty last name of another object.
            if (string.IsNullOrEmpty(this.LastName))
            {
                return 1;
            }

            // The non-empty last name of the current object is less than the empty last name of another object.
            if (string.IsNullOrEmpty(other.LastName))
            {
                return -1;
            }

            // Use CompareTo to get the value to compare.
            int result = this.LastName.CompareTo(other.LastName);

            // Multiply by -1 if reverse sorting is needed.
            return result * _sortDirection;
        }

        /// <summary>
        /// Method for setting sort direction: 1 - default; -1 - reverse.
        /// </summary>
        /// <param name="ascending">True = default; false = reverse.</param>
        public static void SetSortDirection(bool ascending)
        {
            // Assigning to a sort destination field.
            _sortDirection = ascending ? 1 : -1;
        }

        /// <summary>
        /// An override method that returns a string by concatenating fields.
        /// </summary>
        /// <returns>Concatenated string.</returns>
        public override string ToString()
        {
            // Concatenating fields by ','.
            string result = string.Join(",", _data) + ",";
            return result;
        }
    }
}