using Microsoft.Data.Sqlite;

namespace DanceStudioDesktop;

public class Statisztika
{
    private readonly List<Course> courses = new();

    public Statisztika(string databasePath)
    {
        BeolvasKurzusok(databasePath);
    }

    private void BeolvasKurzusok(string databasePath)
    {
        var connectionString = new SqliteConnectionStringBuilder { DataSource = databasePath }.ToString();

        try
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT id, name, type, length, instructor FROM courses";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                courses.Add(new Course(
                    reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetInt32(3),
                    reader.GetString(4)));
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Hiba az adatbázis elérésekor: {ex}");
            Environment.Exit(1);
        }
    }

    public void RunStatistics()
    {
        CsoportosKurzusokSzama();
        LeghosszabbKurzusAdatai();
        OktatoKurzusAlapjan();
    }

    private void CsoportosKurzusokSzama()
    {
        var count = courses.Count(c =>
            c.Type.Equals("csoportos", StringComparison.OrdinalIgnoreCase) ||
            c.Type.Equals("group", StringComparison.OrdinalIgnoreCase));

        Console.WriteLine($"Csoportos kurzusok száma: {count}");
    }

    private void LeghosszabbKurzusAdatai()
    {
        if (courses.Count == 0)
        {
            Console.WriteLine("Nincs kurzus adat.");
            return;
        }

        var maxLength = courses.Max(c => c.Length);
        var longestCourse = courses
            .Where(c => c.Length == maxLength)
            .OrderBy(c => c.Id)
            .First();

        Console.WriteLine("Leghosszabb kurzus adatai:");
        Console.WriteLine($"Id: {longestCourse.Id}");
        Console.WriteLine($"Név: {longestCourse.Name}");
        Console.WriteLine($"Típus: {longestCourse.Type}");
        Console.WriteLine($"Hossz: {longestCourse.Length}");
        Console.WriteLine($"Oktató: {longestCourse.Instructor}");
    }

    private void OktatoKurzusAlapjan()
    {
        Console.Write("Adjon meg egy kurzus nevet: ");
        var name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Nincs ilyen kurzus");
            return;
        }

        var course = courses.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (course is null)
        {
            Console.WriteLine("Nincs ilyen kurzus");
            return;
        }

        Console.WriteLine($"A kurzus oktatója: {course.Instructor}");
    }
}
