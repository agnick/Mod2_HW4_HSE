namespace CsvUtils
{
    public struct Requisites
    {
        // Private arrays to store numbers and dates
        private string[] _numbers;
        private string[] _dates;

        // Public properties to access the numbers and dates arrays
        public string[] Number => _numbers;
        public string[] Date => _dates;

        // Default constructor initializing the struct with empty arrays
        public Requisites() : this(Array.Empty<string>(), Array.Empty<string>()) { }

        // Parameterized constructor allowing the initialization of numbers and dates arrays
        public Requisites(string[] numbers, string[] dates)
        {
            // Assign the provided numbers and dates arrays to the private fields
            _numbers = numbers;
            _dates = dates;
        }
    }
}