using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Runtime
{
    public class AutoMaterial : Modifier
    {
        #region Properties
        public MeshRenderer TargetRenderer { get; set; }
        #endregion


        #region Methods
        public AutoMaterial() { }
        public AutoMaterial(MeshRenderer targetRenderer)
        {
            TargetRenderer = targetRenderer;
        }
        public override StackElement Bake(StackElement previousResult)
        {
            if (TargetRenderer == null && previousResult.Stack != null && previousResult.Stack.Owner != null)
            {
                TargetRenderer = previousResult.Stack.Owner.MeshRenderer;
            }

            int count = previousResult.GetSubmeshesID().Count;

            // Generate as many default materials as there are submeshes
            Material defaultMat = MakeDefaultMaterial();
            previousResult.UpdateMaterials();
            if (TargetRenderer != null)
            {
                // Make sure that only materials that are not already set in the renderer are replaced
                for (int i = 0; i < count; i++)
                {
                    if (previousResult.Materials.Count > i && previousResult.Materials[i] != null)
                        continue;

                    if (previousResult.Materials.Count <= i)
                        previousResult.Materials.Add(defaultMat);
                    else
                        previousResult.Materials[i] = defaultMat;
                }
                TargetRenderer.sharedMaterials = previousResult.Materials.ToArray();
            }

            
            return previousResult;
        }

        private List<Material> GenerateMaterials(int count)
        {
            List<Material> materials = new List<Material>();
            Material mat = MakeDefaultMaterial();
            for (int i = 0; i < count; i++)
                materials.Add(mat);
            return materials;
        }

        private Material MakeDefaultMaterial()
        {
            string path = "Modelular/Standard";

            Shader shader = Shader.Find(path);
            if (shader == null)
                throw new System.ArgumentException("No shader with the specified path could be found : " + path);
            Material newMat = new Material(shader);
            return newMat;
        }


        #endregion
    }
}