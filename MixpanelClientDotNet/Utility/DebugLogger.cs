using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixpanelDotNet
{
    public class DebugLogger
    {
        public static string Write(Exception ex)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Source: {ex.Source}");
            builder.AppendLine($"Message: {ex.Message}");
            builder.AppendLine($"HResult: {ex.HResult}");
            builder.AppendLine($"{ex.StackTrace}");
            if (ex.InnerException != null)
            {
                builder.AppendLine(Write(ex.InnerException));
            }
            string content = builder.ToString();
            Debug.WriteLine(content);
            return content;
        }
    }
}