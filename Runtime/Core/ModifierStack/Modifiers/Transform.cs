using UnityEngine;
using UnityEngine.UIElements;

namespace Modelular.Runtime
{
    [ModelularInterface("Transform", -1)]
    public class Transform : Modifier
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        [ModelularDefaultValue("Vector3.one")]
        public Vector3 Scale { get; set; } = Vector3.one;

        public override StackElement Bake(StackElement previousResult)
        {
            var rotator = Matrix4x4.Rotate(Quaternion.Euler(Rotation));
            var scaler = Matrix4x4.Scale(Scale);
            if (Scale == Vector3.zero)
                scaler = Matrix4x4.identity;
            foreach (var polygon in previousResult.Polygons)
            {
                polygon.ReplaceVertices((v) => (rotator * scaler * v) + Position);
            }
            return previousResult;
        }

    }
}