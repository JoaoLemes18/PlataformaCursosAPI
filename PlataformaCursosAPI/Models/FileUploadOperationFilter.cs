using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parameters = context.MethodInfo.GetParameters();

        // Verifica se há algum parâmetro IFormFile
        if (!parameters.Any(p => p.ParameterType == typeof(IFormFile)))
        {
            return;
        }

        var schema = new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>(),
            Required = new HashSet<string>()
        };

        foreach (var param in parameters)
        {
            OpenApiSchema paramSchema;

            if (param.ParameterType == typeof(IFormFile))
            {
                paramSchema = new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                };
            }
            else if (param.ParameterType == typeof(DateTime) || param.ParameterType == typeof(DateTime?))
            {
                paramSchema = new OpenApiSchema
                {
                    Type = "string",
                    Format = "date-time"
                };
            }
            else
            {
                paramSchema = new OpenApiSchema
                {
                    Type = "string"
                };
            }

            schema.Properties.Add(param.Name!, paramSchema);

            if (!param.IsOptional)
            {
                schema.Required.Add(param.Name!);
            }
        }

        operation.RequestBody = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = schema
                }
            }
        };
    }
}
