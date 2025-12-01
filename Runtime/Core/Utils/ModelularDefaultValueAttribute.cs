

using System;

namespace Modelular.Runtime
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