using System.Collections.Generic;
using System.Linq;

namespace Modelular.Runtime
{

    public struct MeshData
    {
        public IEnumerable<SubmeshData> Submeshes => SubmeshesByID?.Values;
        public Dictionary<int, SubmeshData> SubmeshesByID { get; set; }

        #region Methods

        public SubmeshData GetSubmeshByID(int ID)
        {
            if (SubmeshesByID.ContainsKey(ID))
                return SubmeshesByID[ID];
            return default;
        }

        public MeshData(IEnumerable<SubmeshData> submeshes)
        {
            this.SubmeshesByID = new();
            foreach (SubmeshData submesh in submeshes)
            {
                SubmeshesByID[submesh.ID] = submesh;
            }
        }

        public MeshData(params SubmeshData[] submeshes)
        {
            this.SubmeshesByID = new();
            foreach (SubmeshData submesh in submeshes)
            {
                SubmeshesByID[submesh.ID] = submesh;
            }
        }

        public static MeshData operator +(MeshData a, MeshData b)
        {
            if (a.Submeshes != null)
                if (b.Submeshes != null)
                    return new MeshData(a.Submeshes.Union(b.Submeshes));
                else
                    return a;
            else if (b.Submeshes != null)
                return b;
            return new MeshData();
        }
        #endregion
    }
}