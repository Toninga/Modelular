using UnityEngine;

namespace Modelular.Runtime
{
	public class TorusModel : ModifierModel
	{
		#region Fields

		public DefaultPrimitiveProperties DefaultParameters;
        [Min(0f)]
        public float Radius = 1f;
        [Min(0f)]
        public float Thickness = 0.1f;
        [Min(3f)]
        public int RadialSubdiv = 3;
        [Min(3f)]
        public int ThicknessSubdiv = 3;
        [Range(0f, 1f)]
        public float Arc = 1f;
        public EAxis Axis;
        public bool Caps = true;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private DefaultPrimitiveProperties _defaultParameters;
        private float _radius;
        private float _thickness;
        private int _radialSubdiv;
        private int _thicknessSubdiv;
        private float _arc;
        private EAxis _axis;
        private bool _caps;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public TorusModel()
		{
			underlyingModifier = new Modelular.Runtime.Torus();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.Torus);
			target.DefaultParameters = DefaultParameters;
            target.Radius = Radius;
            target.Thickness = Thickness;
            target.RadialSubdiv = RadialSubdiv;
            target.ThicknessSubdiv = ThicknessSubdiv;
            target.Arc = Arc;
            target.Axis = Axis;
            target.Caps = Caps;
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
                 _thickness != Thickness ||
                 _radialSubdiv != RadialSubdiv ||
                 _thicknessSubdiv != ThicknessSubdiv ||
                 _arc != Arc ||
                 _axis != Axis ||
                 _caps != Caps ||
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
             _thickness = Thickness;
             _radialSubdiv = RadialSubdiv;
             _thicknessSubdiv = ThicknessSubdiv;
             _arc = Arc;
             _axis = Axis;
             _caps = Caps;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
