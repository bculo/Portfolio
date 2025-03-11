using System.Security.Cryptography;

using (RSA rsa = RSA.Create())
{
    // Set the key size (e.g., 2048 or 4096)
    rsa.KeySize = 2048;

    // Export the public and private keys
    string publicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());
    string privateKey = Convert.ToBase64String(rsa.ExportPkcs8PrivateKey());

    Console.WriteLine("Public Key:");
    Console.WriteLine(publicKey);
    Console.WriteLine("\nPrivate Key:");
    Console.WriteLine(privateKey);
}