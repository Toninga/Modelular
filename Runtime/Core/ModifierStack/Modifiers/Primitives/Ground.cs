using System.IO;
using UnityEditor;
using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Primitives/Ground", 0)]
    public class Ground : Modifier, IPrimitiveModifier
    {
        [ModelularDefaultValue("Color.white")]
        public Color Color {get; set;} = Color.white;
        public string OutputSelectionGroup {get; set;}
        [ModelularDefaultValue("Vector2.one * 100f")]
        public Vector2 Size { get; set;} = Vector2.one * 100f;
        #region Properties



        #endregion


        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            var quad = new Quad();
            quad.Size = Size;
            quad.Color = Color;
            quad.OutputSelectionGroup = OutputSelectionGroup;
            quad.Bake(previousResult);

            var uv = new UVTilingOffset();
            uv.Tiling = new Vector2(Size.x / 2, Size.y / 2);
            uv.Offset = new Vector2(-Size.x / 4, -Size.y / 4);
            uv.Bake(previousResult);

            var tex = new SetTexture();
            tex.Texture = AssetDatabase.LoadAssetAtPath(Path.Combine(Hierarchy.TexturesPath, "T_ModelularChecker.png"), typeof(Texture2D)) as Texture2D;
            tex.Bake(previousResult);
            return previousResult;
        }

        

        #endregion
    }
}