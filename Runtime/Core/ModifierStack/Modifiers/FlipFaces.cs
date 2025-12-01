using Modelular.Data;
using Modelular.Editor;
using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Modifiers
{
    [ModelularInterface(20)]
    public class FlipFaces : Modifier
    {
        public override StackElement Bake(StackElement previousResult)
        {
            foreach (var polygon in previousResult.Polygons)
            {
                for (int i = 0; i < polygon.triangles.Count/3; i++)
                {
                    int temp = polygon.triangles[i*3+1];
                    polygon.triangles[i*3+1] = polygon.triangles[i*3+2];
                    polygon.triangles[i*3+2] = temp;

                }
            }
            return previousResult;
        }
    }
}