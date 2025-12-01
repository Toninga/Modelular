using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Transform", -1)]
    public class Transform : Modifier
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; } = Vector3.one;

        public override StackElement Bake(StackElement previousResult)
        {
            var mat = MakeMatrix(Rotation, Scale);
            foreach (var polygon in previousResult.Polygons)
            {
                polygon.ReplaceVertices((v) => (mat * v) + Position);
            }
            return previousResult;
        }

        private Matrix4x4 MakeMatrix(Vector3 rotation, Vector3 scale)
        {
            Matrix4x4 result = Matrix4x4.identity;
            var rotator = Matrix4x4.Rotate(Quaternion.Euler(rotation));
            var scaler = Matrix4x4.Scale(scale);
            result = rotator * scaler;
            return result;
        }
    }
}