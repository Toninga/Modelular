

namespace Modelular.Runtime
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