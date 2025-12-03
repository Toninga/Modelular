using System;

namespace Modelular.Runtime
{
    [System.Serializable]
    public struct SelectorParameters
    {

        public string OutputSelectionGroup;
        public ESelectionOperand SelectionOperand;


        public static bool operator == ( SelectorParameters left, SelectorParameters right ) => left.Equals ( right );
        public static bool operator != ( SelectorParameters left, SelectorParameters right ) { return !left.Equals ( right ); }
        public override bool Equals(object obj)
        {
            return obj is SelectorParameters parameters &&
                   OutputSelectionGroup == parameters.OutputSelectionGroup &&
                   SelectionOperand == parameters.SelectionOperand;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(OutputSelectionGroup, SelectionOperand);
        }
    }
}