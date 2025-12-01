

using UnityEngine;

namespace Modelular.Runtime
{
    public interface IPrimitiveModifier
    {
        public Color Color { get; set; }
        public string OutputSelectionGroup { get; set; }
    }
}