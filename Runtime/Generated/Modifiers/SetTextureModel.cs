using UnityEngine;

namespace Modelular.Runtime
{
	public class SetTextureModel : ModifierModel
	{
		#region Fields

		public int MaterialIndex;
        public string PropertyName = "_MainTex";
        public Texture2D Texture;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private int _materialIndex;
        private string _propertyName;
        private Texture2D _texture;
        //[ReplicatedField]

		#endregion
		public SetTextureModel()
		{
			underlyingModifier = new Modelular.Runtime.SetTexture();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.SetTexture);
			target.MaterialIndex = MaterialIndex;
            target.PropertyName = PropertyName;
            target.Texture = Texture;
            //[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			 _materialIndex != MaterialIndex ||
                 _propertyName != PropertyName ||
                 _texture != Texture ||
                //[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			 _materialIndex = MaterialIndex;
             _propertyName = PropertyName;
             _texture = Texture;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
