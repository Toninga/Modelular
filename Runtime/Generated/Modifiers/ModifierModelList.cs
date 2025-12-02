using System.Collections.Generic;
using Modelular.Runtime;

namespace Modelular.Editor
{
    public static class ModifierModelList
    {
        public static Dictionary<string, System.Type> ModifierPaths = new();
        static ModifierModelList()
        {
            ModifierPaths["Transform"] = typeof(TransformModel);
            ModifierPaths["Transform"] = typeof(TransformModel);
            ModifierPaths["Primitives/Cube"] = typeof(CubeModel);
            ModifierPaths["Primitives/Cylinder"] = typeof(CylinderModel);
            ModifierPaths["Primitives/Ground"] = typeof(GroundModel);
            ModifierPaths["Primitives/Quad"] = typeof(QuadModel);
            ModifierPaths["Primitives/UV Sphere"] = typeof(UVSphereModel);
            ModifierPaths["Normals/Flip faces"] = typeof(FlipFacesModel);
            ModifierPaths["Normals/Flip normals"] = typeof(FlipNormalsModel);
            ModifierPaths["Color/Set color"] = typeof(SetColorModel);
            ModifierPaths["Utility/Attach modifier stack"] = typeof(AttachModifierStackModel);
            ModifierPaths["Copy/Copy to grid"] = typeof(CopyToGridModel);
            ModifierPaths["Copy/Copy to radius"] = typeof(CopyToRadiusModel);
            ModifierPaths["UV/Tiling and offset"] = typeof(UVTilingOffsetModel);
            ModifierPaths["Color/Color gradient"] = typeof(ColorGradientModel);
            ModifierPaths["Select/Select by color"] = typeof(SelectByColorModel);
            ModifierPaths["Utility/Set Submesh"] = typeof(SetSubmeshModel);
            ModifierPaths["Material/SetTexture"] = typeof(SetTextureModel);
            //[PathMatching]
        }
    }
}

