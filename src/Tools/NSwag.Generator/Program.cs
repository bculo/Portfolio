using NSwag;
using NSwag.CodeGeneration.CSharp;

var filePath = Path.Combine("OpenApi", "keycloak-23.json");
var serviceName = "KeycloakAdminApiClient";
var nameSpace = "Keycloak.Admin.Client";

if (!File.Exists(filePath))
{
    throw new FileNotFoundException();
}

var json = await File.ReadAllTextAsync(filePath);
var document = await OpenApiDocument.FromJsonAsync(json);

var settings = new CSharpClientGeneratorSettings
{
    ClassName = serviceName,
    CSharpGeneratorSettings =
    {
        Namespace = nameSpace,
    },
    UseBaseUrl = false
};

var generator = new CSharpClientGenerator(document, settings);
var code = generator.GenerateFile();

await File.WriteAllTextAsync("../NSwagGenerated.cs", code);