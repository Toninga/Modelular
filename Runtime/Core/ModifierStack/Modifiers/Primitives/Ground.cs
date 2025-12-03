using System.IO;
using UnityEditor;
using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Primitives/Ground", 0)]
    public class Ground : Modifier
    {
        [ModelularDefaultValue("DefaultPrimitiveProperties.Default()")]
        public DefaultPrimitiveProperties DefaultParameters { get; set; } = DefaultPrimitiveProperties.Default();
        [ModelularDefaultValue("Vector2.one * 100f")]
        public Vector2 Size { get; set;} = Vector2.one * 100f;
        #region Properties



        #endregion


        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            StackElement ground = new();
            var quad = new Quad();
            quad.Size = Size;
            quad.Bake(ground);

            var uv = new UVTilingOffset();
            uv.Tiling = new Vector2(Size.x / 2, Size.y / 2);
            uv.Offset = new Vector2(-Size.x / 4, -Size.y / 4);
            uv.Bake(ground);

            var tex = new SetTexture();
            tex.Texture = AssetDatabase.LoadAssetAtPath(Path.Combine(Hierarchy.TexturesPath, "T_ModelularChecker.png"), typeof(Texture2D)) as Texture2D;
            tex.Bake(ground);

            previousResult.Merge(ground);
            return previousResult;
        }

        

        #endregion
    }
}