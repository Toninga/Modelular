

using Modelular.Data;
using System;

namespace Modelular.Selection
{

    public class Selector
    {
        #region Properties
        public ESelectionTarget SelectionType { get; private set; }
        public ESelectionOperand Operand { get; set; }
        public int SelectionID { get; set; }

        #endregion

        #region Fields
        private Func<Vertex, bool> _vertexFilter = (v) => false;
        private Func<Polygon, bool> _polygonFilter = (p) => false;

        #endregion

        #region Methods
        
        public Selector(Func<Vertex, bool> predicate, ESelectionOperand operand=ESelectionOperand.Union)
        {
            Operand = operand;
            _vertexFilter = predicate;
            SelectionType = ESelectionTarget.Vertex;
        }
        
        public Selector(Func<Polygon, bool> predicate, ESelectionOperand operand = ESelectionOperand.Union)
        {
            Operand = operand;
            _polygonFilter = predicate;
            SelectionType = ESelectionTarget.Polygon;
        }

        /// <summary>
        /// Checks if a polygon matches the selection.
        /// If the selection is vertex-based, the check is true if at least one vertex of the polygon passes the test
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool Compare(Polygon p)
        {
            bool result = false;
            switch (SelectionType)
            {
                case ESelectionTarget.Vertex:
                    foreach(var v in p.vertices)
                        if (_vertexFilter(v))
                            result = true;
                    break;
                case ESelectionTarget.Polygon:
                    if (_polygonFilter(p))
                        result = true;
                    break;

                case ESelectionTarget.None:
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// Checks if the vertex matches the selection.
        /// If the selection is polygon-based, it will be ignored.
        /// Pass-in the owning polygon to allow polygon-based selection to occur.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool Compare(Vertex v)
        {
            bool result = false;

            switch (SelectionType)
            {
                case ESelectionTarget.Vertex:
                    if (_vertexFilter(v))
                        result = true;
                    break;
                case ESelectionTarget.Polygon:
                    break;

                case ESelectionTarget.None:
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Checks if the vertex matches the selection.
        /// If the selection is polygon-based, it will return true if the owning polygon passes the test.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="owningPolygon"></param>
        /// <returns></returns>
        public bool Compare(Vertex v, Polygon owningPolygon)
        {
            bool result = false;

            switch (SelectionType)
            {
                case ESelectionTarget.Vertex:
                    if (_vertexFilter(v))
                        result = true;
                    break;
                case ESelectionTarget.Polygon:
                    if (_polygonFilter(owningPolygon))
                        result = true;
                    break;

                case ESelectionTarget.None:
                default:
                    break;
            }

            return result;
        }

        #endregion
    }
}