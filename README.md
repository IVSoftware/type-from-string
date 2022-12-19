Here's one possible scheme that, when tested with the four types mentioned by gunr2171, seems to work. It relies on a `UnderlyingTypeName` to serialize the full type name and meets the requirement that the `MyClass` cannot be generic.

The `UnderlyingItem` property itself is ignored by the json serializer. The getter for this property queries the loaded assemblies to find one containing the target type. If found, the json deserializer uses the specific type. Otherwise the default Json.Net deserializer is invoked.

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

***

**Testbench**

[![four types][1]][1]

**string[]**

    MyClass testRoundTripIn, testRoundTripOut;
    string json;
    Type returnedType;

    testRoundTripIn = new MyClass
    {
        UnderlyingItem = new string[] { "A", "B", "C"},
    };
    json = JsonConvert.SerializeObject(testRoundTripIn, Formatting.Indented);

    testRoundTripOut = JsonConvert.DeserializeObject<MyClass>(json)!;

    returnedType = testRoundTripOut.UnderlyingItem.GetType();
    if (typeof(string[]).Equals(returnedType))
    {
        Console.WriteLine($"MyClass.UnderlyingItem deserialized as {returnedType.FullName}");
    }
    else
    {
        Console.WriteLine($"Error string[] Deserialized as {returnedType.Name}");
    }

**List<string>**

    testRoundTripIn = new MyClass
    {
        UnderlyingItem = new List<string> { "A", "B", "C"},
    };
    json = JsonConvert.SerializeObject(testRoundTripIn, Formatting.Indented);

    testRoundTripOut = JsonConvert.DeserializeObject<MyClass>(json)!;

    returnedType = testRoundTripOut.UnderlyingItem.GetType();
    if (typeof(List<string>).Equals(returnedType))
    {
        Console.WriteLine($"MyClass.UnderlyingItem deserialized as {returnedType.Name}");
    }
    else
    {
        Console.WriteLine($"Error List<string> deserialized as {returnedType.FullName}");
    }

**double**

    testRoundTripIn = new MyClass
    {
        UnderlyingItem = Math.PI,
    };
    json = JsonConvert.SerializeObject(testRoundTripIn, Formatting.Indented);

    testRoundTripOut = JsonConvert.DeserializeObject<MyClass>(json)!;

    returnedType = testRoundTripOut.UnderlyingItem.GetType();
    if (typeof(double).Equals(returnedType))
    {
        Console.WriteLine($"MyClass.UnderlyingItem deserialized as {returnedType.Name}");
    }
    else
    {
        Console.WriteLine($"Error double deserialized as {returnedType.FullName}");
    }

**decimal**

    testRoundTripIn = new MyClass
    {
        UnderlyingItem = 1.23m,
    };
    json = JsonConvert.SerializeObject(testRoundTripIn, Formatting.Indented);

    testRoundTripOut = JsonConvert.DeserializeObject<MyClass>(json)!;

    returnedType = testRoundTripOut.UnderlyingItem.GetType();
    if (typeof(decimal).Equals(returnedType))
    {
        Console.WriteLine($"MyClass.UnderlyingItem deserialized as {returnedType.Name}");
    }
    else
    {
        Console.WriteLine($"Error decimal deserialized as {returnedType.FullName}");
    }


  [1]: https://i.stack.imgur.com/QItdj.png