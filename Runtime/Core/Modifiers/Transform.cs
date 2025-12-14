using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Transform", -1)]
    public class Transform : Modifier, IModifier
    {
        #region Parameters
        public string TargetSelectionGroup { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        [ModelularDefaultValue("Vector3.one")]
        public Vector3 Scale { get; set; } = Vector3.one;
        #endregion

        public override StackElement Bake(StackElement previousResult)
        {
            var rotator = Matrix4x4.Rotate(Quaternion.Euler(Rotation));
            var scaler = Matrix4x4.Scale(Scale);
            if (Scale == Vector3.zero)
                scaler = Matrix4x4.identity;

            previousResult.ReplaceVertices((v) => (rotator * scaler * v) + Position, TargetSelectionGroup);
            return previousResult;
        }

        public void Setup(STransform transform)
        {
            Position = transform.Position;
            Rotation = transform.Rotation;
            Scale = transform.Scale;
        }

        public static List<Polygon> Apply(List<Polygon> l, STransform transform)
        {
            StackElement se = new();
            se.AddPolygons(l);
            Transform t = new();
            t.Setup(transform);
            t.Bake(se);
            return se.Polygons;
        }
    }
}