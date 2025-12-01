

using System;

namespace Modelular.Editor
{
    [System.AttributeUsage(AttributeTargets.Property)]
    public class ModelularDefaultValueAttribute : Attribute
    {
        public string DefaultValue { get; set; }
        public ModelularDefaultValueAttribute(string defaultValue)
        {
            DefaultValue = defaultValue;
        }
    }
}