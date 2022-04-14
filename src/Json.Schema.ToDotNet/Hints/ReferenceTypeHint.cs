using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Json.Schema.ToDotNet.Hints
{
    public class ReferenceTypeHint : CodeGenHint
    {
        public ReferenceTypeHint(string typeName)
        {
            TypeName = typeName;
        }

        public string TypeName { get; }
    }
}
