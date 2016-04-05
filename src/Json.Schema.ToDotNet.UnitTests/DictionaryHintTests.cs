﻿// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using FluentAssertions;
using Microsoft.Json.Schema.UnitTests;
using Xunit;

namespace Microsoft.Json.Schema.ToDotNet.UnitTests
{
    public class DictionaryHintTests
    {
        private readonly TestFileSystem _testFileSystem;
        private readonly DataModelGeneratorSettings _settings;

        public DictionaryHintTests()
        {
            _testFileSystem = new TestFileSystem();
            _settings = TestSettings.MakeSettings();
        }

        public static readonly object[] TestCases = new object[]
        {
            new object[]
            {
@"{
  ""type"": ""object"",
  ""description"": ""Class with property bag."",
  ""properties"": {
    ""properties"": {
      ""description"": ""Set of key-value pairs."",
      ""type"": ""object""
    }
  }
}",

@"{
  ""C.Properties"": [
    {
      ""$type"": ""Microsoft.Json.Schema.ToDotNet.DictionaryHint, Microsoft.Json.Schema.ToDotNet""
    }
  ]
}",

@"using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace N
{
    /// <summary>
    /// Class with property bag.
    /// </summary>
    [DataContract]
    [GeneratedCode(""Microsoft.Json.Schema.ToDotNet"", ""0.6.0.0"")]
    public sealed class C : IEquatable<C>
    {
        /// <summary>
        /// Set of key-value pairs.
        /// </summary>
        [DataMember(Name = ""properties"", IsRequired = false, EmitDefaultValue = false)]
        public IDictionary<string, string> Properties { get; set; }

        public override bool Equals(object other)
        {
            return Equals(other as C);
        }

        public override int GetHashCode()
        {
            int result = 17;
            unchecked
            {
                if (Properties != null)
                {
                    // Use xor for dictionaries to be order-independent.
                    int xor_0 = 0;
                    foreach (var value_0 in Properties)
                    {
                        xor_0 ^= value_0.Key.GetHashCode();
                        if (value_0.Value != null)
                        {
                            xor_0 ^= value_0.Value.GetHashCode();
                        }
                    }

                    result = (result * 31) + xor_0;
                }
            }

            return result;
        }

        public bool Equals(C other)
        {
            if (other == null)
            {
                return false;
            }

            if (!Object.ReferenceEquals(Properties, other.Properties))
            {
                if (Properties == null || other.Properties == null || Properties.Count != other.Properties.Count)
                {
                    return false;
                }

                foreach (var value_0 in Properties)
                {
                    var value_1;
                    if (!other.Properties.TryGetValue(value_0.Key, out value_1))
                    {
                        return false;
                    }

                    if (value_0.Value != value_1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}"
            },

            new object[]
            {
@"{
  ""type"": ""object"",
  ""properties"": {
    ""dictionaryOfD"": {
      ""type"": ""object"",
      ""additionalProperties"": {
        ""$ref"": ""#/definitions/d""
      }
    }
  },
  ""definitions"": {
    ""d"": {
      ""type"": ""object""
    }
  }
}",

@"{
  ""C.DictionaryOfD"": [
    {
      ""$type"": ""Microsoft.Json.Schema.ToDotNet.DictionaryHint, Microsoft.Json.Schema.ToDotNet""
    }
  ]
}",

@"using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace N
{
    [DataContract]
    [GeneratedCode(""Microsoft.Json.Schema.ToDotNet"", ""0.6.0.0"")]
    public sealed class C : IEquatable<C>
    {
        [DataMember(Name = ""dictionaryOfD"", IsRequired = false, EmitDefaultValue = false)]
        public IDictionary<string, D> DictionaryOfD { get; set; }

        public override bool Equals(object other)
        {
            return Equals(other as C);
        }

        public override int GetHashCode()
        {
            int result = 17;
            unchecked
            {
                if (DictionaryOfD != null)
                {
                    // Use xor for dictionaries to be order-independent.
                    int xor_0 = 0;
                    foreach (var value_0 in DictionaryOfD)
                    {
                        xor_0 ^= value_0.Key.GetHashCode();
                        if (value_0.Value != null)
                        {
                            xor_0 ^= value_0.Value.GetHashCode();
                        }
                    }

                    result = (result * 31) + xor_0;
                }
            }

            return result;
        }

        public bool Equals(C other)
        {
            if (other == null)
            {
                return false;
            }

            if (!Object.ReferenceEquals(DictionaryOfD, other.DictionaryOfD))
            {
                if (DictionaryOfD == null || other.DictionaryOfD == null || DictionaryOfD.Count != other.DictionaryOfD.Count)
                {
                    return false;
                }

                foreach (var value_0 in DictionaryOfD)
                {
                    var value_1;
                    if (!other.DictionaryOfD.TryGetValue(value_0.Key, out value_1))
                    {
                        return false;
                    }

                    if (value_0.Value != value_1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}"
            }
        };

        [Theory(DisplayName = "DictionaryHint generates dictionary")]
        [MemberData(nameof(TestCases))]
        public void GeneratesDictionary(
            string schemaText,
            string hintsText,
            string expected)
        {
            _settings.HintDictionary = HintDictionary.Deserialize(hintsText);
            _settings.GenerateOverrides = true;
            var generator = new DataModelGenerator(_settings, _testFileSystem.FileSystem);

            JsonSchema schema = SchemaReader.ReadSchema(schemaText);

            string actual = generator.Generate(schema);

            TestUtil.WriteTestResultFiles(expected, actual, nameof(GeneratesDictionary));

            actual.Should().Be(expected);
        }
    }
}
