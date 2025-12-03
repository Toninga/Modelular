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


        #endregion

        #region Methods


        public override StackElement Bake(StackElement previousResult)
        {
            StackElement obj = new StackElement();
            obj.AddPolygons(Make(Size), DefaultParameters.OutputSelectionGroup);
            (this as IPrimitive).ApplyDefaultParameters(obj);
            


            previousResult.Merge(obj);
            return previousResult;
        }

        protected List<Polygon> Make(Vector3 size)
        {
            size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
            List<Polygon> result = new List<Polygon>();
            Vector2 upSize = new Vector2(size.x, size.z);
            Vector2 frontSize = new Vector2(size.x, size.y);
            Vector2 leftSize = new Vector2(size.y, size.z);
            
            result.Add(Vector3.up * size.y / 2 + Polygon.Quad(upSize));
            result.Add(Vector3.forward * size.z / 2 + Quaternion.Euler(90,0,0) * Polygon.Quad(frontSize));
            result.Add(Vector3.forward * size.z / 2 - Quaternion.Euler(-90,0,0) * Polygon.Quad(frontSize));
            result.Add(Vector3.left * size.x / 2 + Quaternion.Euler(0,0,90) * Polygon.Quad(leftSize));
            result.Add(Vector3.left * size.x / 2 - Quaternion.Euler(0,0,-90) * Polygon.Quad(leftSize));
            result.Add(Vector3.up * size.y / 2 - Quaternion.Euler(180,0,0) * Polygon.Quad(upSize));

            return result;
        }

        #endregion
    }

}