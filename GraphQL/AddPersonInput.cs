namespace GraphQL;

public class AddPersonInput
{
    public string Name { get; }
    public int Age { get; }

    public AddPersonInput(
        string name,
        int age)
    {
        Name = name;
        Age = age;
    }
}