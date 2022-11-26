using DynamicViewWithDynamicObject.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicViewWithDynamicObject.ExtensiveMethods
{
    public static class ExtensiveMethods
    {
        public static string CreateClass(this CustomObject a,string className)
        {
            var sb = new StringBuilder();
            sb.Append("public class ");
            sb.Append(className);
            sb.Append("{");
            sb.AppendLine();
            foreach (var item in a.dictionary)
            {
                sb.Append("    ");
                sb.Append(item.Value.GetType());
                sb.Append(" ");
                sb.Append(item.Key);
                sb.Append(" ");
                sb.Append("{ get; set; }");
                sb.AppendLine();
            }
            sb.Append("}");
            sb.AppendLine();
            return sb.ToString();
        }
        public static Type GetPropertyType(this KeyValuePair<string,object> propful)
        {
            return propful.Value.GetType();
        }
    }
    public class HelperMethods
    {
        public static void WriteClassToCs(string classDefinition,string className)
        {
            string BaseClassPath = Path.Combine(Environment.CurrentDirectory, "../../../Models/BaseClass.cs");

            var str = new StreamReader(BaseClassPath).ReadToEnd();
            var xx = str.Split('{');
            var uspace = xx[0]+"{\n"+ classDefinition + xx[1];
            var tree = CSharpSyntaxTree.ParseText(uspace);
            var root = tree.GetRoot()
                .NormalizeWhitespace();
            var ret = root.ToFullString();
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, $"../../../Models/{className}.cs"), ret);

        }
    }
}
