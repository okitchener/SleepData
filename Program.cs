// ask for input
Console.WriteLine("Enter 1 to create data file.");
Console.WriteLine("Enter 2 to parse data.");
Console.WriteLine("Enter anything else to quit.");
// input response
string? resp = Console.ReadLine();

if (resp == "1")
{
    // create data file

    // ask a question
    Console.WriteLine("How many weeks of data is needed?");
    // input the response (convert to int)
    int weeks = Convert.ToInt32(Console.ReadLine());
    // determine start and end date
    DateTime today = DateTime.Now;
    // we want full weeks sunday - saturday
    DateTime dataEndDate = today.AddDays(-(int)today.DayOfWeek);
    // subtract # of weeks from endDate to get startDate
    DateTime dataDate = dataEndDate.AddDays(-(weeks * 7));
    // random number generator
    Random rnd = new();
    // create file
    StreamWriter sw = new("data.txt");

    // loop for the desired # of weeks
    while (dataDate < dataEndDate)
    {
        // 7 days in a week
        int[] hours = new int[7];
        for (int i = 0; i < hours.Length; i++)
        {
            // generate random number of hours slept between 4-12 (inclusive)
            hours[i] = rnd.Next(4, 13);
        }
        // M/d/yyyy,#|#|#|#|#|#|#
        // Console.WriteLine($"{dataDate:M/d/yy},{string.Join("|", hours)}");
        sw.WriteLine($"{dataDate:M/d/yyyy},{string.Join("|", hours)}");
        // add 1 week to date
        dataDate = dataDate.AddDays(7);
    }
    sw.Close();
}//
else if (resp == "2")
{
    if (!File.Exists("data.txt"))
    {
        Console.WriteLine("Data file not found. Please create data first.");
        return;
    }

    // Read all lines from the file
    string[] lines = File.ReadAllLines("data.txt");

    foreach (string line in lines)
    {
        // Each line: date,h1|h2|h3|h4|h5|h6|h7
        var parts = line.Split(',');

        if (parts.Length != 2)
        {
            Console.WriteLine("Invalid data format.");
            continue;
        }

        // Parse the start date of the week
        if (!DateTime.TryParse(parts[0], out DateTime weekStart))
        {
            Console.WriteLine("Invalid date format.");
            continue;
        }

        // Parse hours slept each night (Did get a little help from google here)
        string[] hourStrings = parts[1].Split('|');
        if (hourStrings.Length != 7)
        {
            Console.WriteLine("Invalid hours data.");
            continue;
        }

        int[] hours = new int[7];
        for (int i = 0; i < 7; i++)
        {
            if (!int.TryParse(hourStrings[i], out hours[i]))
            {
                Console.WriteLine("Invalid hour value.");
                break;
            }
        }

        // Extra Credit Here Calculate total and average hours for the week
        int total = hours.Sum();
        double average = total / 7.0;

        // Print the weekly report with exact formatting
        Console.WriteLine($"Week of {weekStart:MMM, dd, yyyy}");
        Console.WriteLine(" Su Mo Tu We Th Fr Sa Tot Avg");
        Console.WriteLine(" --  --  --  --  --  --  --  ---  ---");

        // Format each hour with width 3, right-aligned, with spaces between,
        // Then total width 4, average width 4 with 1 decimal place
        // Example: "  7   4   10   6   9  11   7   48  6.9"
        Console.WriteLine(
            $" {hours[0],2}  {hours[1],2}  {hours[2],2}  {hours[3],2}  {hours[4],2}  {hours[5],2}  {hours[6],2}  {total,3} {average,4:F1}");

        Console.WriteLine();
    }
}
