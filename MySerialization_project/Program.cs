using MySerialization_project;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq.Expressions;
using static fastJSON.Reflection;

var model = new Test
{
    Salarys = new List<Test2>()
    {
        new()
        {
            Age = new Test3()
            {
                Age = 234,
            }, 
            Salary = 23,
        },
        new()
        {
            Age = new Test3()
            {
                Age = 234,
            },
            Salary = 23,
        }
    },
    Height = 123,
    Name = "Hello",
    LastName = "Hello",
};

string content = string.Empty;
var list = new List<Test>();

for (int i = 0; i < 1; i++)
{
    list.Add(new Test
    {
        Salarys = new List<Test2>()
    {
        new()
        {
            Age = new Test3()
            {
                Age = i+234,
            },
            Salary = i+23,
        }
    },
        Height = i+54,
        Name = "Hello",
        LastName = "Hello",
    });
}


CountTime("Мой", Serializer.Serialize, list);
CountTime("Newtonsoft", JsonConvert.SerializeObject, list);
CountTime("FastJson", fastJSON.JSON.ToJSON, list);

void CountTime<T>(string str, Func<T, string> func, T list)
{
    Stopwatch stopwatch = new Stopwatch();

    stopwatch.Start();

    var result = func(list);
    //if (content == string.Empty)
    //{
    //    content = result;
    //}
    //else
    //{
    //    if (content != result)
    //    {
    //        throw new Exception("Not match");
    //    }
    //}

    stopwatch.Stop();

    Console.WriteLine($"Время выполнения {str} кода: {stopwatch.ElapsedMilliseconds} миллисекунд.");
    Console.WriteLine(result);

    var obj = Serializer.Deserialize<Test>(result);
}




class Test
{
    public bool IsActive { get; set; } = true;
    public string Name { get; set; }
    public string LastName { get; set; }
    public int Height { get; set; }
    public ICollection<Test2> Salarys { get; set; }
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