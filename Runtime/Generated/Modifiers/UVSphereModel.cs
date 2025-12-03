using UnityEngine;

namespace Modelular.Runtime
{
	public class UVSphereModel : ModifierModel
	{
		#region Fields

		public DefaultPrimitiveProperties DefaultParameters = DefaultPrimitiveProperties.Default();
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
		private DefaultPrimitiveProperties _defaultParameters;
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
			target.DefaultParameters = DefaultParameters;
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
			 _defaultParameters != DefaultParameters ||
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
			 _defaultParameters = DefaultParameters;
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
