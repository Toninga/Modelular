using UnityEngine;

namespace Modelular.Runtime
{
	public class SetSubmeshModel : ModifierModel
	{
		#region Fields

		public string TargetSelectionGroup;
        [Min(0f)]
        public int SubmeshID;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private string _targetSelectionGroup;
        private int _submeshID;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public SetSubmeshModel()
		{
			underlyingModifier = new Modelular.Runtime.SetSubmesh();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.SetSubmesh);
			target.TargetSelectionGroup = TargetSelectionGroup;
            target.SubmeshID = SubmeshID;
            target.IgnoreVertexLimits = IgnoreVertexLimits;
            //[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			 _targetSelectionGroup != TargetSelectionGroup ||
                 _submeshID != SubmeshID ||
                 _ignoreVertexLimits != IgnoreVertexLimits ||
                //[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			 _targetSelectionGroup = TargetSelectionGroup;
             _submeshID = SubmeshID;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
