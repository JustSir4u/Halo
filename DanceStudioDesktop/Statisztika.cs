using Microsoft.Data.Sqlite;

namespace DanceStudioDesktop;

public class Statisztika
{
    private readonly List<Course> courses = new();

    public Statisztika(string databasePath)
    {
        BeolvasCourses(databasePath);
    }

    private void BeolvasCourses(string databasePath)
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
            Console.Error.WriteLine($"Hiba az adatbázis elérésekor: {ex.Message}");
            Environment.Exit(1);
        }
    }

    public void FeladatokVegrehajtasa()
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
        var longestCourse = courses.OrderByDescending(c => c.Length).FirstOrDefault();

        if (longestCourse is null)
        {
            Console.WriteLine("Nincs kurzus adat.");
            return;
        }

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

        var course = courses.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (course is null)
        {
            Console.WriteLine("Nincs ilyen kurzus");
            return;
        }

        Console.WriteLine($"A kurzus oktatója: {course.Instructor}");
    }
}
