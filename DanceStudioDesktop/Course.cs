namespace DanceStudioDesktop;

public class Course
{
    public long Id { get; }
    public string Name { get; }
    public string Type { get; }
    public int Length { get; }
    public string Instructor { get; }

    public Course(long id, string name, string type, int length, string instructor)
    {
        Id = id;
        Name = name;
        Type = type;
        Length = length;
        Instructor = instructor;
    }
}
