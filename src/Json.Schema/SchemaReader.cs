// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Json.Schema
{
    public static class SchemaReader
    {
        public static JsonSchema ReadSchema(TextReader reader, string filePath)
        {
            return ReadSchema(reader.ReadToEnd(), filePath);
        }

        public static async Task<JsonSchema> ReadSchema(Uri schemaUri, string filePath)
        {
            using var httpClient = new HttpClient();
            using var stream = await httpClient.GetStreamAsync(schemaUri);
            using var streamReader = new StreamReader(stream);

            return ReadSchema(streamReader.ReadToEnd(), filePath);
        }

        public static JsonSchema ReadSchema(string jsonText, string filePath)
        {
            SchemaValidationErrorAccumulator.Instance.Clear();

            // Change "$ref" to "$$ref" before we ask Json.NET to deserialize it,
            // because Json.NET treats "$ref" specially.
            jsonText = RefProperty.ConvertFromInput(jsonText);

            var serializer = new JsonSerializer
            {
                ContractResolver = new JsonSchemaContractResolver(),
            };

            JsonSchema schema;
            using (var jsonReader = new JsonTextReader(new StringReader(jsonText)))
            {
                try
                {
                    schema = serializer.Deserialize<JsonSchema>(jsonReader);
                }
                catch (JsonReaderException ex)
                {
                    throw new JsonSyntaxException(filePath, ex);
                }
                finally
                {
                    if (SchemaValidationErrorAccumulator.Instance.HasErrors)
                    {
                        throw SchemaValidationErrorAccumulator.Instance.ToException();
                    }
                }
            }

            return schema;
        }
    }
}
