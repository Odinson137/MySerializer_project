
using MySerialization_project;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;

var model = new Test
{
    Salary = new Test2()
    {
        Salary = 143,
    },
    Height = 123,
    Name = "Hello",
    LastName = "Hello",
};

Stopwatch stopwatch = new Stopwatch();

stopwatch.Start();

var str = Selializer.Serialize(new List<Test>() { model }).ToString();

stopwatch.Stop();

Console.WriteLine($"Время выполнения моего кода: {stopwatch.ElapsedMilliseconds} миллисекунд.");

stopwatch.Start();

var str2 = JsonConvert.SerializeObject(model);

stopwatch.Stop();

Console.WriteLine($"Время выполнения внешней библиотеки: {stopwatch.ElapsedMilliseconds} миллисекунд.");

stopwatch.Start();

stopwatch.Stop();

Console.WriteLine($"Время выполнения внешней библиотеки: {stopwatch.ElapsedMilliseconds} миллисекунд.");


Console.WriteLine(str);
Console.WriteLine(str2);


class Test
{
    public bool IsActive { get; set; } = true;
    public string Name { get; set; }
    public string LastName { get; set; }
    public int Height { get; set; }
    public Test2 Salary { get; set; }
}

class Test2
{
    public Test3 Age { get; set; }
    public int Salary { get; set; }
}

class Test3
{
    public int Age { get; set; }
}