using Modelular.Data;
using Modelular.Editor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Modifiers
{
    [ModelularInterface(60)]
    public class CopyToRadius : Modifier
    {
        // TODO : Add a mode with a fixed angle offset
        #region Properties
        [ModelularDefaultValue("8")]
        public int Count { get; set; } = 8;
        [ModelularDefaultValue("1f")]
        public float Radius { get; set; } = 1f;
        [ModelularDefaultValue("1f")]
        public float Arc01 { get; set; } = 1f;

        [ModelularDefaultValue("true")]
        public bool FaceNormal { get; set; } = true; // Not implemented

        #endregion


        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            var radial = MakeRadialLayout(previousResult.Polygons);
            previousResult.ReplacePolygons(radial);
            return previousResult;
        }

        private List<Polygon> MakeRadialLayout(List<Polygon> mesh)
        {
            List<Polygon> result = new List<Polygon>();

            

            for (int i = 0; i < Count; i++)
            {
                Arc01 = Mathf.Clamp01(Arc01);
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
            p.x += radius;
            return new Vector3(
            (Mathf.Cos(angle) * p.x - (Mathf.Sin(angle) * p.z)),
            p.y,
            (Mathf.Sin(angle) * p.x + Mathf.Cos(angle) * p.z)
            );
        }

        #endregion
    }
}