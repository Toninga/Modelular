

using System;

namespace Modelular.Editor
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