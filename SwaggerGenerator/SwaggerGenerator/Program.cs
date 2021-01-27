using NSwag;
using NSwag.CodeGeneration.CSharp;
using System;
using System.IO;

namespace SwaggerGenerator
{

    class Program
    {
        public static async void GenerateCode(CSharpClientGeneratorSettings settings, string filePath)
        {
            System.Net.WebClient wclient = new System.Net.WebClient();

            string path = @"C:\apps\CSharpClientGenerator\SwaggerGenerator\SwaggerGenerator\squidex.json";
            string url = @"https://cloud.squidex.io/api/content/ilearn/swagger/v1/swagger.json";
            
            string jsonString = File.ReadAllText(path);
            var document = await OpenApiDocument.FromJsonAsync(wclient.DownloadString(url));

            wclient.Dispose();

            var generator = new CSharpClientGenerator(document, settings);
            var code = generator.GenerateFile();
            File.WriteAllText(filePath, code);
        }
        static void Main(string[] args)
        {
            string baseLocation = @"C:\apps\CSharpClientGenerator\SwaggerGenerator\SwaggerGenerator\";
            var serviceSettings = new CSharpClientGeneratorSettings
            {
                ClassName = "ContentAppService",
                ClientBaseClass = "ContentManagementAppService, IContentAppService",
                GenerateClientClasses = true,
                GenerateClientInterfaces = false,
                GenerateDtoTypes = false,
                CSharpGeneratorSettings =
                {
                    Namespace = "ContentManagement.Content"

                }
            };
            GenerateCode(serviceSettings, baseLocation + "ContentAppService.cs");

            var interfaceSettings = new CSharpClientGeneratorSettings
            {
                ClassName = "IContentAppService",
                ClientBaseClass = "IApplicationService",
                GenerateClientClasses = false,
                GenerateClientInterfaces = true,
                GenerateDtoTypes = false,
                CSharpGeneratorSettings =
                {
                    Namespace = "ContentManagement.Content"

                }
            };
            GenerateCode(interfaceSettings, baseLocation + "IContentAppService.cs");

            var dtoSettings = new CSharpClientGeneratorSettings
            {
                ClassName = "ContentAppServiceDto",
                GenerateClientClasses = false,
                GenerateClientInterfaces = false,
                GenerateDtoTypes = true,
                CSharpGeneratorSettings =
                {
                    Namespace = "ContentManagement.Content"

                }
            };
            GenerateCode(dtoSettings, baseLocation + "ContentAppServiceDto.cs");


            Console.WriteLine("Hello World!");
        }
    }
}
