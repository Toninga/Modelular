using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Primitives/Cube", 0)]
    public class Cube : Modifier, IPrimitiveModifier
    {
        #region Parameters
        [ModelularDefaultValue("Color.white")]
        public Color Color { get; set; }
        public string OutputSelectionGroup { get; set; }
        [ModelularDefaultValue("Vector3.one")]
        public Vector3 Size {  get; set; }


        #endregion

        #region Methods


        public override StackElement Bake(StackElement previousResult)
        {
            previousResult.AddPolygons(Make(Size), OutputSelectionGroup);
            var mod = new SetColor();
            mod.Color = Color;
            mod.Bake(previousResult);
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