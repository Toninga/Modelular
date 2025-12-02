
namespace Modelular.Runtime
{

    public abstract class Modifier
    {
        public bool IgnoreMaximumAllowedVertexCount { get; set; }
        public abstract StackElement Bake(StackElement previousResult);
    }

}