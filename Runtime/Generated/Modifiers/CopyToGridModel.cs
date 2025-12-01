using UnityEngine;

namespace Modelular.Runtime
{
	public class CopyToGridModel : ModifierModel
	{
		#region Fields

		public Vector3Int Count = Vector3Int.one * 2;
        public Vector3 Distance = Vector3.one;
        public ECenterMode CenterMode = ECenterMode.Centered;
        public EGridLayoutMode GridLayout = EGridLayoutMode.FixedSize;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private Vector3Int _count;
        private Vector3 _distance;
        private ECenterMode _centerMode;
        private EGridLayoutMode _gridLayout;
        //[ReplicatedField]

		#endregion
		public CopyToGridModel()
		{
			underlyingModifier = new Modelular.Runtime.CopyToGrid();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.CopyToGrid);
			target.Count = Count;
            target.Distance = Distance;
            target.CenterMode = CenterMode;
            target.GridLayout = GridLayout;
            //[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			 _count != Count ||
                 _distance != Distance ||
                 _centerMode != CenterMode ||
                 _gridLayout != GridLayout ||
                //[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			 _count = Count;
             _distance = Distance;
             _centerMode = CenterMode;
             _gridLayout = GridLayout;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
