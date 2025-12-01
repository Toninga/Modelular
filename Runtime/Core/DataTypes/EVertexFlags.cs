using System;

namespace Modelular.Runtime
{

    [Flags] public enum EVertexFlags
    {
        None = 0,
        Position = 1,
        Normal = 2,
        Color = 4,
        Submesh = 8,
        UV0 = 16,
        UV1 = 32,
        UV2 = 64,
        UV3 = 128,
    }


}