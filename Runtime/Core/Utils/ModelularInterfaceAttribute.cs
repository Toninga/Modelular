

using System;

namespace Modelular.Runtime
{
    [System.AttributeUsage(AttributeTargets.Class)]
    public class ModelularInterfaceAttribute : Attribute
    {
        public int Priority { get; set; }
        public string ItemName { get; set; }
        public ModelularInterfaceAttribute(string itemName="", int priority=1000)
        {
            ItemName = itemName;
            Priority = priority;
        }
    }
}