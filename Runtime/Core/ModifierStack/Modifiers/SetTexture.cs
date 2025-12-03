using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Material/Set Texture")]
    public class SetTexture : Modifier
    {
        #region Properties
        public int MaterialIndex { get; set; }
        [ModelularDefaultValue("\"_MainTex\"")]
        public string PropertyName { get; set; } = "_MainTex";
        public Texture2D Texture { get; set; }


        #endregion

        
        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            var mod = new AutoMaterial();
            mod.Bake(previousResult);
            if (MaterialIndex >= previousResult.Materials.Count)
                return previousResult;

            foreach (var mat in previousResult.Materials)
            {
                if (mat.HasTexture(PropertyName))
                {
                    mat.SetTexture(PropertyName, Texture);
                }
            }

            return previousResult;
        }

        

        #endregion
    }
}