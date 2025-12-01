

using UnityEngine;

namespace Modelular.Modifiers.Primitives
{
    public interface IPrimitiveModifier
    {
        public Color Color { get; set; }
        public string OutputSelectionGroup { get; set; }
    }
}