using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Json.Schema.ToDotNet.Hints
{
    public class ArrayTypeHint : CodeGenHint
    {
        public ArrayTypeHint(string itemsTypeName)
        {
            ItemsTypeName = itemsTypeName;
        }

        public string ItemsTypeName { get; }
    }
}
