using UnityEngine;

namespace Modelular.Runtime
{
	public class SetColorModel : ModifierModel
	{
		#region Fields

		public string TargetSelectionGroup;
        public Color Color = Color.white;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private string _targetSelectionGroup;
        private Color _color;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public SetColorModel()
		{
			underlyingModifier = new Modelular.Runtime.SetColor();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.SetColor);
			target.TargetSelectionGroup = TargetSelectionGroup;
            target.Color = Color;
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
                 _color != Color ||
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
             _color = Color;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
