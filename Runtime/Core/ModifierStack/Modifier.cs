
namespace Modelular.Runtime
{

    public abstract class Modifier
    {
        public bool IgnoreVertexLimits { get; set; }
        public abstract StackElement Bake(StackElement previousResult);
    }

}