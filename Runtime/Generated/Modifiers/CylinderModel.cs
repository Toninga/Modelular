using UnityEngine;

namespace Modelular.Runtime
{
	public class CylinderModel : ModifierModel
	{
		#region Fields

		public DefaultPrimitiveProperties DefaultParameters = DefaultPrimitiveProperties.Default();
        public float Height = 1f;
        public float Radius = 0.5f;
        public int HorizontalSubdivisions;
        public int VerticalSubdivisions = 16;
        public bool GenerateCaps = true;
        public bool GenerateShell = true;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private DefaultPrimitiveProperties _defaultParameters;
        private float _height;
        private float _radius;
        private int _horizontalSubdivisions;
        private int _verticalSubdivisions;
        private bool _generateCaps;
        private bool _generateShell;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public CylinderModel()
		{
			underlyingModifier = new Modelular.Runtime.Cylinder();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.Cylinder);
			target.DefaultParameters = DefaultParameters;
            target.Height = Height;
            target.Radius = Radius;
            target.HorizontalSubdivisions = HorizontalSubdivisions;
            target.VerticalSubdivisions = VerticalSubdivisions;
            target.GenerateCaps = GenerateCaps;
            target.GenerateShell = GenerateShell;
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
                 _height != Height ||
                 _radius != Radius ||
                 _horizontalSubdivisions != HorizontalSubdivisions ||
                 _verticalSubdivisions != VerticalSubdivisions ||
                 _generateCaps != GenerateCaps ||
                 _generateShell != GenerateShell ||
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
             _height = Height;
             _radius = Radius;
             _horizontalSubdivisions = HorizontalSubdivisions;
             _verticalSubdivisions = VerticalSubdivisions;
             _generateCaps = GenerateCaps;
             _generateShell = GenerateShell;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
