// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.Json.Schema.ToDotNet.Hints;

namespace Microsoft.Json.Schema.ToDotNet
{
    internal static class Utilities
    {
        internal static string QualifyNameWithSuffix(string name, string suffix)
        {
            return string.IsNullOrWhiteSpace(suffix)
                ? name
                : name + "." + suffix.Trim();
        }

        internal static string GetHintedClassName(HintDictionary hintDictionary, string className)
        {
            ClassNameHint classNameHint = hintDictionary?.GetHint<ClassNameHint>(className.ToCamelCase());
            if (classNameHint != null)
            {
                className = classNameHint.ClassName;
            }

            // Remove "_" from the class name, and make the name to Pascal.
            var subNames = className.Split('_');
            var formattedName = string.Empty;
            foreach (var subName in subNames)
            {
                formattedName += subName.ToPascalCase();
            }

            return formattedName;
        }
    }
}
