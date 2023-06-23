using System.Security.Cryptography;

var key = new byte[32];
using (var generator = RandomNumberGenerator.Create())
    generator.GetBytes(key);
string apiKey = Convert.ToBase64String(key);

Console.WriteLine(apiKey);