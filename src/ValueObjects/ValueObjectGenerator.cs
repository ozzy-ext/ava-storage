using System.Text;

namespace ValueObjects
{
    public static class ValueObjectGenerator
    {
        public static string NonEmptyString()
        {
            const string className = "Class";

            var b = new StringBuilder();

            b.AppendLine($"public partial record {className} : NonEmptyString");
            b.AppendLine("{");
            b.AppendLine($"\tpublic {className} (string value)");
            b.AppendLine("\t{");
            b.AppendLine("\t\tif(!IsValid(value))");
            b.AppendLine("\t\t\tthrow new ValidationException(\"Value has wrong format\");");
            b.AppendLine("\t}");
            b.AppendLine("");
            b.AppendLine("\tpartial public static bool IsValid(string value);");
            b.AppendLine("}");

            return b.ToString();
        }
    }
}
