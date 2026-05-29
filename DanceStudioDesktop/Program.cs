using DanceStudioDesktop;

if (args.Contains("--stat"))
{
    var databasePath = Environment.GetEnvironmentVariable("DANCESTUDIO_DB_PATH") ?? "dancestudio.db";
    var statisztika = new Statisztika(databasePath);
    statisztika.RunStatistics();
}
