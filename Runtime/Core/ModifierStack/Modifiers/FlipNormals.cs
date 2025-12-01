using Modelular.Data;
using Modelular.Editor;
using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Modifiers
{
    [ModelularInterface(20)]
    public class FlipNormals : Modifier
    {
        public override StackElement Bake(StackElement previousResult)
        {
            previousResult.ReplaceVertices((vert) => vert.Flipped());
            return previousResult;
        }
    }
}