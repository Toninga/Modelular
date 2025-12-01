using UnityEngine;

namespace Modelular.Runtime
{
	public class CylinderModel : ModifierModel
	{
		#region Fields

		public Color Color = Color.white;
        public string OutputSelectionGroup;
        public float Height = 1f;
        public float Radius = 0.5f;
        public int HorizontalSubdivisions;
        public int VerticalSubdivisions = 16;
        public bool GenerateCaps = true;
        public bool GenerateShell = true;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private Color _color;
        private string _outputSelectionGroup;
        private float _height;
        private float _radius;
        private int _horizontalSubdivisions;
        private int _verticalSubdivisions;
        private bool _generateCaps;
        private bool _generateShell;
        //[ReplicatedField]

		#endregion
		public CylinderModel()
		{
			underlyingModifier = new Modelular.Runtime.Cylinder();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.Cylinder);
			target.Color = Color;
            target.OutputSelectionGroup = OutputSelectionGroup;
            target.Height = Height;
            target.Radius = Radius;
            target.HorizontalSubdivisions = HorizontalSubdivisions;
            target.VerticalSubdivisions = VerticalSubdivisions;
            target.GenerateCaps = GenerateCaps;
            target.GenerateShell = GenerateShell;
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
                 _height != Height ||
                 _radius != Radius ||
                 _horizontalSubdivisions != HorizontalSubdivisions ||
                 _verticalSubdivisions != VerticalSubdivisions ||
                 _generateCaps != GenerateCaps ||
                 _generateShell != GenerateShell ||
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
             _height = Height;
             _radius = Radius;
             _horizontalSubdivisions = HorizontalSubdivisions;
             _verticalSubdivisions = VerticalSubdivisions;
             _generateCaps = GenerateCaps;
             _generateShell = GenerateShell;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
