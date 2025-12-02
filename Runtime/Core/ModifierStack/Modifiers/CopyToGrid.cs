using System.Collections.Generic;
using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Copy/Copy to grid", 60)]
    public class CopyToGrid : Modifier
    {
        #region Properties
        [ModelularDefaultValue("Vector3Int.one * 2")]
        public Vector3Int Count {  get; set; } = Vector3Int.one * 2;
        [ModelularDefaultValue("Vector3.one")]
        public Vector3 Distance { get; set; } = Vector3.one;
        [ModelularDefaultValue("ECenterMode.Centered")]
        public ECenterMode CenterMode { get; set; } = ECenterMode.Centered;
        [ModelularDefaultValue("EGridLayoutMode.FixedSize")]
        public EGridLayoutMode GridLayout { get; set; } = EGridLayoutMode.FixedSize;
        #endregion


        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            int evc = ExpectedVertexCount(previousResult);
            if (!IgnoreMaximumAllowedVertexCount)
                GlobalSettings.DetectVertexCountLimitations(evc);

            var grid = MakeGrid(previousResult.Polygons);
            previousResult.ReplacePolygons(grid);
            return previousResult;
        }

        public int ExpectedVertexCount(StackElement previousResult)
        {
            int previousCount = previousResult.Vertices.Count;
            return previousCount * Count.x * Count.y * Count.z;
        }

        private List<Polygon> MakeGrid(List<Polygon> mesh)
        {
            List<Polygon> grid = new List<Polygon>();

            Vector3 offset = Vector3.one;
            Vector3 BB = Vector3.one;

            if (GridLayout == EGridLayoutMode.Offset)
            {
                offset = new Vector3(Distance.x, Distance.y, Distance.z);
                BB = new Vector3(Distance.x * (Count.x - 1), Distance.y * (Count.y - 1), Distance.z * (Count.z - 1));
            }
            if (GridLayout == EGridLayoutMode.FixedSize)
            {
                offset = new Vector3(
                    (Count.x - 1 == 0) ? 0 : Distance.x / (Count.x - 1),
                    (Count.y - 1 == 0) ? 0 : Distance.y / (Count.y - 1),
                    (Count.z - 1 == 0) ? 0 : Distance.z / (Count.z - 1)
                    );
                BB = Distance;
            }
            var halfBB = BB / 2;
            for (int x = 0; x < Count.x; x++)
            {
                for (int z = 0; z < Count.z; z++)
                {
                    for (int y  = 0; y < Count.y; y++)
                    {
                        List<Polygon> newElm = new List<Polygon>();
                        foreach(var p in mesh)
                        {
                            Polygon newPoly = new Polygon(p);
                            // Offset computed from the corner of the grid
                            Vector3 o = new Vector3(x * offset.x, y * offset.y, z * offset.z);

                            if (CenterMode == ECenterMode.Centered)
                                o = o - new Vector3(
                                    Count.x - 1 == 0 ? 0 : halfBB.x,
                                    Count.y - 1 == 0 ? 0 : halfBB.y,
                                    Count.z - 1 == 0 ? 0 : halfBB.z);

                            newPoly.ReplaceVertices((v) => new Vertex(v, v.position + o));
                            newElm.Add(newPoly);
                        }
                        grid.AddRange(newElm);
                    }
                }
            }
            return grid;
        }


        #endregion

        
    }
    public enum EGridLayoutMode
    {
        Default = 0,
        FixedSize,
        Offset
    }

    public enum ECenterMode
    {
        Default = 0,
        Centered,
        Corner,
    }
}