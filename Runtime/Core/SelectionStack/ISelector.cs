

namespace Modelular.Runtime

{
    public interface ISelector
    {
        public string OutputSelectionGroup { get; set; }
        public ESelectionOperand SelectionOperand { get; set; }
    }
}