using System.Collections.Generic;

namespace Modelular.Data
{

    public struct MeshData
    {
        public IEnumerable<SubmeshData> Submeshes => SubmeshesByID.Values;
        public Dictionary<int, SubmeshData> SubmeshesByID { get; set; }

        #region Methods

        public SubmeshData GetSubmeshByID(int ID)
        {
            if (SubmeshesByID.ContainsKey(ID))
                return SubmeshesByID[ID];
            return default;
        }


        public MeshData(SubmeshData[] submeshes)
        {
            this.SubmeshesByID = new();
            foreach (SubmeshData submesh in submeshes)
            {
                SubmeshesByID[submesh.ID] = submesh;
            }
        }
        #endregion
    }
}