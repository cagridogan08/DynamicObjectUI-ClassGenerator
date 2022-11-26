using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicWithMVVMTest.Models
{
    public class DynamicModule:DynamicObject
    {
        public static MyDynObject GetDynamicObject(Dictionary<string, object> properties)
        {
            return new MyDynObject();
        }

        public sealed class MyDynObject : DynamicObject
        {
            private readonly Dictionary<string, object> _properties;

            public MyDynObject(Dictionary<string, object> properties)
            {
                _properties = properties;
            }

            public MyDynObject()
            {
                _properties = new();
            }
            public override IEnumerable<string> GetDynamicMemberNames()
            {
                return _properties.Keys;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object? result)
            {
                if (_properties.ContainsKey(binder.Name))
                {
                    result = _properties[binder.Name];
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public override bool TrySetMember(SetMemberBinder binder,[NotNull] object value)
            {
                if (_properties.ContainsKey(binder.Name))
                {
                    _properties[binder.Name] = value;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
