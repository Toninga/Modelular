using Modelular.Runtime;
using System.Collections.Generic;

namespace Modelular.Runtime
{

    public class SelectionStack
    {
        #region Properties
        public List<Selector> Selectors => _selectors;
        #endregion

        #region Fields
        private List<Selector> _selectors = new();
        private int myInt;
        #endregion

        #region Methods
        public void Merge(SelectionStack other) => _selectors.AddRange(other.Selectors);
        public void AddSelectors(IEnumerable<Selector> selectors) => _selectors.AddRange(selectors);
        public void AddSelector(Selector selector) => _selectors.Add(selector);
        public void ClearSelectors() => _selectors.Clear();
        public void ClearPolygonSelectors()
        {
            List<Selector> result = new List<Selector>();
            foreach (var selector in _selectors)
                if (selector.SelectionType != ESelectionTarget.Polygon)
                    result.Add(selector);
            _selectors = result;
        }
        public void ClearVertexSelectors()
        {
            List<Selector> result = new List<Selector>();
            foreach (var selector in _selectors)
                if (selector.SelectionType != ESelectionTarget.Vertex)
                    result.Add(selector);
            _selectors = result;
        }


        public List<Vertex> GetSelectedVertices(List<Vertex> vertices)
        {
            List<Vertex> result = new List<Vertex>();
            // Not implemented
            return result;
        }
        public bool Contains(Vertex v)
        {
            bool result = false;
            foreach (var s in _selectors)
            {
                switch (s.Operand)
                {
                    case ESelectionOperand.Union:
                        result = result || s.Compare(v);
                        break;
                    case ESelectionOperand.Intersection:
                        result = result && s.Compare(v);
                        break;
                    case ESelectionOperand.Replace:
                        result = s.Compare(v);
                        break;
                    case ESelectionOperand.XOR:
                        result = result ^ s.Compare(v);
                        break;

                    default:
                        break;
                }
            }

            return result;
        }

        public bool Contains(Polygon p)
        {
            bool result = false;
            foreach (var s in _selectors)
            {
                switch (s.Operand)
                {
                    case ESelectionOperand.Union:
                        result = result || s.Compare(p);
                        break;
                    case ESelectionOperand.Intersection:
                        result = result && s.Compare(p);
                        break;
                    case ESelectionOperand.Replace:
                        result = s.Compare(p);
                        break;
                    case ESelectionOperand.XOR:
                        result = result ^ s.Compare(p);
                        break;

                    default:
                        break;
                }
            }

            return result;
        }
        // Implement polygon versions as well

        #endregion
    }
}