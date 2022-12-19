
#if false

var typeName= "IV.Control.Tree.SomeClass";
var assy = typeof(MainActivity).Assembly;
var type = Type.GetType($"{full.qualified.typeName}");

#endif
using Newtonsoft.Json;
using System.Reflection;

Console.Title = "Test MyClass";

MyClass testRoundTripIn, testRoundTripOut;
string json;
Type returnedType;

#region  t e s t    s t r i n g [ ]
testRoundTripIn = new MyClass
{
    UnderlyingItem = new string[] { "A", "B", "C"},
};
json = JsonConvert.SerializeObject(testRoundTripIn, Formatting.Indented);

testRoundTripOut = JsonConvert.DeserializeObject<MyClass>(json);

returnedType = testRoundTripOut.UnderlyingItem.GetType();
if (typeof(string[]).Equals(returnedType))
{
    Console.WriteLine($"MyClass.UnderlyingItem deserialized as {returnedType.FullName}");
}
else
{
    Console.WriteLine($"Error string[] Deserialized as {returnedType.Name}");
}
#endregion  t e s t    s t r i n g [ ]

#region  t e s t    L i s t < s t r i n g >
testRoundTripIn = new MyClass
{
    UnderlyingItem = new List<string> { "A", "B", "C"},
};
json = JsonConvert.SerializeObject(testRoundTripIn, Formatting.Indented);

testRoundTripOut = JsonConvert.DeserializeObject<MyClass>(json);

returnedType = testRoundTripOut.UnderlyingItem.GetType();
if (typeof(List<string>).Equals(returnedType))
{
    Console.WriteLine($"MyClass.UnderlyingItem deserialized as {returnedType.FullName}");
}
else
{
    Console.WriteLine($"Error List<string> deserialized as {returnedType.FullName}");
}
#endregion  t e s t    L i s t < s t r i n g >

#region  t e s t    d o u b l e
testRoundTripIn = new MyClass
{
    UnderlyingItem = Math.PI,
};
json = JsonConvert.SerializeObject(testRoundTripIn, Formatting.Indented);

testRoundTripOut = JsonConvert.DeserializeObject<MyClass>(json);

returnedType = testRoundTripOut.UnderlyingItem.GetType();
if (typeof(double).Equals(returnedType))
{
    Console.WriteLine($"MyClass.UnderlyingItem deserialized as {returnedType.Name}");
}
else
{
    Console.WriteLine($"Error double deserialized as {returnedType.FullName}");
}
#endregion  t e s t    d o u b l e

#region  t e s t    d e c i m a l
testRoundTripIn = new MyClass
{
    UnderlyingItem = 1.23m,
};
json = JsonConvert.SerializeObject(testRoundTripIn, Formatting.Indented);

testRoundTripOut = JsonConvert.DeserializeObject<MyClass>(json);

returnedType = testRoundTripOut.UnderlyingItem.GetType();
if (typeof(decimal).Equals(returnedType))
{
    Console.WriteLine($"MyClass.UnderlyingItem deserialized as {returnedType.Name}");
}
else
{
    Console.WriteLine($"Error decimal deserialized as {returnedType.FullName}");
}
#endregion  t e s t     d e c i m a l

Console.ReadKey();
    public class MyClass
    {
        public int Counter { get; set; }
        public string UnderlyingItemString { get; set; }
        public string UnderlyingTypeName { get; set; }
        [JsonIgnore]
        public object UnderlyingItem
        {
            get
            {
                if (string.IsNullOrWhiteSpace(UnderlyingItemString))
                {
                    return null;
                }
                else
                {
                    Type type = typeof(object);
                    Assembly assy = null;
                    if (!string.IsNullOrEmpty(UnderlyingTypeName))
                        assy =
                            AppDomain
                            .CurrentDomain
                            .GetAssemblies()
                            .FirstOrDefault(_ => _.GetType(UnderlyingTypeName) != null);
                    if (assy == null)
                    {
                        return JsonConvert.DeserializeObject(UnderlyingItemString);
                    }
                    else
                    {
                        type = assy.GetType(UnderlyingTypeName);
                        return JsonConvert.DeserializeObject(UnderlyingItemString, type);
                    }
                }
            }
            set
            {
                if (!Equals(_UnderlyingItem, value))
                {
                    _UnderlyingItem = value;
                    UnderlyingItemString = JsonConvert.SerializeObject(value);
                    UnderlyingTypeName = _UnderlyingItem.GetType().FullName;
                }
            }
        }
        private object _UnderlyingItem = new object();
    }
