using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace YurtBursu.Api.Swagger
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParams = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile));

            if (!fileParams.Any()) return;

            // Ensure RequestBody exists
            operation.RequestBody ??= new OpenApiRequestBody { Content = new Dictionary<string, OpenApiMediaType>() };

            // Ensure multipart/form-data media type exists
            if (!operation.RequestBody.Content.TryGetValue("multipart/form-data", out var uploadMediaType))
            {
                uploadMediaType = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>()
                    }
                };
                operation.RequestBody.Content["multipart/form-data"] = uploadMediaType;
            }

            // Ensure Schema and Properties exist
            uploadMediaType.Schema ??= new OpenApiSchema { Type = "object", Properties = new Dictionary<string, OpenApiSchema>() };
            uploadMediaType.Schema.Properties ??= new Dictionary<string, OpenApiSchema>();

            foreach (var param in fileParams)
            {
                var name = param.Name ?? "file";
                if (!uploadMediaType.Schema.Properties.ContainsKey(name))
                {
                    uploadMediaType.Schema.Properties[name] = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    };
                }
            }
        }
    }
}
