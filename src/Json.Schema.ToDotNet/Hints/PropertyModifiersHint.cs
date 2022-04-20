// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.Json.Schema.ToDotNet.Hints
{
    /// <summary>
    /// Represents a code generation hint that tells the code generator to declare a
    /// property with the specified modifiers instead of the default <code>public</code>
    /// modifier.
    /// </summary>
    public class PropertyModifiersHint : CodeGenHint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyModifiersHint"/> class.
        /// </summary>
        /// <param name="modifiers">
        /// The property modifiers.
        /// </param>
        public PropertyModifiersHint(IEnumerable<string> modifiers, bool onlyGet, bool noAccessor)
        {
            Modifiers = modifiers.Select(TokenFromModifierName).ToList();
            OnlyGet = onlyGet;
            NoAccessor = noAccessor;
        }

        private SyntaxToken TokenFromModifierName(string modifierName)
        {
            var kind = modifierName switch
            {
                "public" => SyntaxKind.PublicKeyword,
                "internal" => SyntaxKind.InternalKeyword,
                "protected" => SyntaxKind.PrivateKeyword,
                "private" => SyntaxKind.PrivateKeyword,
                "override" => SyntaxKind.OverrideKeyword,
                "virtual" => SyntaxKind.VirtualKeyword,
                _ => throw new ArgumentException(
                        string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ErrorInvalidModifier,
                        modifierName)),
            };
            return SyntaxFactory.Token(kind);
        }

        /// <summary>
        /// Gets the property modifiers.
        /// </summary>
        public IList<SyntaxToken> Modifiers { get; }
        public bool OnlyGet { get; }
        public bool NoAccessor { get; }
    }
}
