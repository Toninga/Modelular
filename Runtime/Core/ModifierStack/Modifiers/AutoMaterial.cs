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
            int count = previousResult.GetSubmeshesID().Count;

            // Generate as many default materials as there are submeshes
            List<Material> updatedMaterials = GenerateMaterials(count);
            if (TargetRenderer != null)
            {
                // Make sure that only materials that are not already set in the renderer are replaced
                for (int i = 0; i < count; i++)
                {
                    if (TargetRenderer.sharedMaterials.Length-1 >= i && TargetRenderer.sharedMaterials[i] != null)
                        updatedMaterials[i] = TargetRenderer.sharedMaterials[i];
                }
            }
            previousResult.Materials = updatedMaterials;
            
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
            // Find the current render pipeline
            // NOTE : This can be upgraded with a shader supporting any SRP regardless of it's sub type
            string path;
            switch (Shader.globalRenderPipeline)
            {
                case "UniversalPipeline":
                    path = "Modellular/URPStandard";
                    break;
                case "HDRenderPipeline":
                    path = "Modellular/HDRPStandard";
                    break;
                case "":
                    path = "Modellular/BuiltInStandard";
                    break;
                default:
                    path = "Modellular/BuiltInStandard";
                    break;

            }
            //Debug.Log("AutoMaterial : Current pipeline is '" + Shader.globalRenderPipeline + "' ; material path is '" + path + "'");
            // Find the adequate material, or standard if no srp is found
            Shader shader = Shader.Find(path);
            if (shader == null)
                throw new System.ArgumentException("No shader with the specified path could be found : " + path);
            Material newMat = new Material(shader);
            return newMat;
        }


        #endregion
    }
}