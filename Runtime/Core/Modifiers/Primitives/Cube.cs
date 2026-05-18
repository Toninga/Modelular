using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Primitives/Cube", 0)]
    public class Cube : Modifier, IPrimitive
    {
        #region Parameters
        [ModelularDefaultValue("DefaultPrimitiveProperties.Default()")]
        public DefaultPrimitiveProperties DefaultParameters { get; set; } = DefaultPrimitiveProperties.Default();

        [ModelularDefaultValue("Vector3.one")]
        public Vector3 Size {  get; set; }
        [ModelularDefaultValue("Vector3Int.zero")]
        public Vector3Int Subdivisions {  get; set; }


        #endregion

        #region Methods


        public override StackElement Bake(StackElement previousResult)
        {
            StackElement obj = new StackElement();
            obj.AddPolygons(Make(Size, Subdivisions), DefaultParameters.OutputSelectionGroup);
            (this as IPrimitive).ApplyDefaultParameters(obj);
            


            previousResult.Merge(obj);
            return previousResult;
        }

        protected List<Polygon> Make(Vector3 size, Vector3Int subdiv)
        {
            size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
            List<Polygon> result = new List<Polygon>();
            Vector2 upSize = new Vector2(size.x, size.z);
            Vector2 frontSize = new Vector2(size.x, size.y);
            Vector2 leftSize = new Vector2(size.z, size.y);
            subdiv = new Vector3Int(Mathf.Max(0, subdiv.x), Mathf.Max(0, subdiv.y), Mathf.Max(0, subdiv.z));
            Vector2Int upSubdiv = new(subdiv.x, subdiv.z);
            Vector2Int frontSubdiv = new(subdiv.x, subdiv.y);
            Vector2Int leftSubdiv = new(subdiv.z, subdiv.y);

            Vector3 up = Vector3.up * size.y / 2;
            Vector3 fw = Vector3.forward * size.z / 2;
            Vector3 lf = Vector3.left * size.x / 2;
            
            result.Add(MeshUtility.MakeGrid(upSize, upSubdiv, Vector3.up) + up);
            result.Add(MeshUtility.MakeGrid(upSize, upSubdiv, -Vector3.up) - up);
            result.Add(MeshUtility.MakeGrid(frontSize, frontSubdiv, Vector3.forward) + fw);
            result.Add(MeshUtility.MakeGrid(frontSize, frontSubdiv, -Vector3.forward) - fw);
            result.Add(MeshUtility.MakeGrid(leftSize, leftSubdiv, Vector3.left) + lf);
            result.Add(MeshUtility.MakeGrid(leftSize, leftSubdiv, -Vector3.left) - lf);

            return result;
        }

        #endregion
    }

}