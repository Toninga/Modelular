
namespace Modelular.Modifiers
{

    public abstract class Modifier
    {
        public abstract StackElement Bake(StackElement previousResult);
    }

}