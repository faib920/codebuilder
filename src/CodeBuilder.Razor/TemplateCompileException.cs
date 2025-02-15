using System;

namespace CodeBuilder.Razor
{
    public class TemplateCompileException : Exception
    {
        public TemplateCompileException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
