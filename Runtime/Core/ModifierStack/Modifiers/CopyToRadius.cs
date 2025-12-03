using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Copy/Copy to radius", 60)]
    public class CopyToRadius : Modifier, IModifier,  ISelector
    {
        // TODO : Add a mode with a fixed angle offset
        #region Properties
        public string TargetSelectionGroup { get; set; }
        public SelectorParameters OutputParameters { get; set; }
        [ModelularDefaultValue("8")]
        public int Count { get; set; } = 8;
        [ModelularDefaultValue("1f")]
        public float Radius { get; set; } = 1f;
        [ModelularDefaultValue("1f")]
        [Range(0f, 1f)]
        public float Arc01 { get; set; } = 1f;

        [ModelularDefaultValue("true")]
        public bool FaceNormal { get; set; } = true; // Not implemented
        [ModelularDefaultValue("EAxis.Y")]
        public EAxis Axis { get; set; } = EAxis.Y;

        #endregion


        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            int evc = ExpectedVertexCount(previousResult);
            if (!IgnoreVertexLimits)
                GlobalSettings.DetectVertexCountLimitations(evc);

            var radial = MakeRadialLayout(previousResult.GetPolygons(TargetSelectionGroup));
            previousResult.RemovePolygons(TargetSelectionGroup);
            previousResult.AddPolygons(radial, OutputParameters.OutputSelectionGroup);
            return previousResult;
        }

        private int ExpectedVertexCount(StackElement previousResult)
        {
            return previousResult.Vertices.Count * Count;
        }

        private List<Polygon> MakeRadialLayout(List<Polygon> mesh)
        {
            List<Polygon> result = new List<Polygon>();

            Arc01 = Mathf.Clamp01(Arc01);

            for (int i = 0; i < Count; i++)
            {
                float angle = i / (float)Count * 2 * Mathf.PI * Arc01;

                List<Polygon> newObject = new List<Polygon>();
                foreach (var p  in mesh)
                {
                    Polygon newPoly = new(p);
                    newPoly.ReplaceVertices((v) => new Vertex(v, Offset(v.position, angle, Radius), Offset(v.normal, angle,0f)));
                    newObject.Add(newPoly);
                }
                result.AddRange( newObject );
            }

            return result;
        }

        private Vector3 Offset(Vector3 p, float angle, float radius)
        {
            Vector3 result;
            Vector3 dir = AxisUtility.GetAxisDirection(Axis);
            Vector3 right = new Vector3(dir.y, dir.z, dir.x);
            Quaternion rotator = Quaternion.Euler(dir * angle / Mathf.PI * 180);
            if (FaceNormal)
            {
                p += right * radius;
                result = rotator * p;
            }
            else
            {
                result = p + rotator * (right * radius);
            }
                return result;

        }

        #endregion
    }
}