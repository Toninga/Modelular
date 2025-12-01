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
            //[PathMatching]
        }
    }
}
