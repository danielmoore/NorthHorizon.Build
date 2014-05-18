using System;

namespace DemoProject
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class SourceHashAttribute : Attribute
    {
        public SourceHashAttribute(byte[] hash) { }
        public SourceHashAttribute(string hahs) { }
    }
}