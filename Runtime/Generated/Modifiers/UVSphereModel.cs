using UnityEngine;

namespace Modelular.Runtime
{
	public class UVSphereModel : ModifierModel
	{
		#region Fields

		public Color Color = Color.white;
        public string OutputSelectionGroup;
        public float Radius = 0.5f;
        [Min(1f)]
        public int HorizontalSubdivisions = 8;
        [Min(3f)]
        public int VerticalSubdivisions = 16;
        public string TargetSelectionSet;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private Color _color;
        private string _outputSelectionGroup;
        private float _radius;
        private int _horizontalSubdivisions;
        private int _verticalSubdivisions;
        private string _targetSelectionSet;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public UVSphereModel()
		{
			underlyingModifier = new Modelular.Runtime.UVSphere();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.UVSphere);
			target.Color = Color;
            target.OutputSelectionGroup = OutputSelectionGroup;
            target.Radius = Radius;
            target.HorizontalSubdivisions = HorizontalSubdivisions;
            target.VerticalSubdivisions = VerticalSubdivisions;
            target.TargetSelectionSet = TargetSelectionSet;
            target.IgnoreVertexLimits = IgnoreVertexLimits;
            //[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			 _color != Color ||
                 _outputSelectionGroup != OutputSelectionGroup ||
                 _radius != Radius ||
                 _horizontalSubdivisions != HorizontalSubdivisions ||
                 _verticalSubdivisions != VerticalSubdivisions ||
                 _targetSelectionSet != TargetSelectionSet ||
                 _ignoreVertexLimits != IgnoreVertexLimits ||
                //[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			 _color = Color;
             _outputSelectionGroup = OutputSelectionGroup;
             _radius = Radius;
             _horizontalSubdivisions = HorizontalSubdivisions;
             _verticalSubdivisions = VerticalSubdivisions;
             _targetSelectionSet = TargetSelectionSet;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
