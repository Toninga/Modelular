

using System;

namespace Modelular.Runtime
{
    [System.AttributeUsage(AttributeTargets.Class)]
    public class ModelularInterfaceAttribute : Attribute
    {
        public int Priority { get; set; }
        public ModelularInterfaceAttribute(int priority=1000)
        {
            Priority = priority;
        }
    }
}