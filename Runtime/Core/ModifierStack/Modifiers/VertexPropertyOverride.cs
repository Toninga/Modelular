using Modelular.Runtime;

namespace Modelular.Runtime
{
    public class VertexPropertyOverride : Modifier
    {
        #region Parameters
        public bool ApplyOnlyChangedValues { get; set; } = true;
        public Vertex Overrides { get; set; }


        #endregion

        #region Methods


        public override StackElement Bake(StackElement previousResult)
        {
            previousResult.ReplaceVertices((v) => DataProcessor.OverrideVertexProperties(v, Overrides, ApplyOnlyChangedValues));
            return previousResult;
        }

        


        #endregion
    }

}