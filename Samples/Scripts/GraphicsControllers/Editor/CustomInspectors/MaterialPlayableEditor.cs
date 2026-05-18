using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MaterialPlayable))]
public class MaterialPlayableEditor : Editor
{
    string stopDrawingPropertiesBefore = "TargetMaterialScope";
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        serializedObject.UpdateIfRequiredOrScript();
        SerializedProperty iterator = serializedObject.GetIterator();
        bool enterChildren = true;
        while (iterator.NextVisible(enterChildren))
        {
            if (iterator.name == stopDrawingPropertiesBefore)
            {
                break;
            }

            using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
            {
                
                EditorGUILayout.PropertyField(iterator, true);
            }

            enterChildren = false;
        }

        DrawRemainingProperties();

        serializedObject.ApplyModifiedProperties();
        EditorGUI.EndChangeCheck();
    }

    void DrawRemainingProperties()
    {
        MaterialPlayable data = (MaterialPlayable)target;

        Draw("TargetMaterialScope");

        if (data.TargetMaterialScope == MaterialPlayable.ETargetMaterialScope.SharedMaterial)
            Draw("_sharedMaterial");
        if (data.TargetMaterialScope == MaterialPlayable.ETargetMaterialScope.SpecificRendererOnly)
            Draw("_targetRenderer");

        Draw("TargetMaterialProperty");
        Draw("_targetMaterialPropertyName");
        if (data.TargetMaterialProperty == MaterialPlayable.ETargetMaterialProperty.Float)
        {
            Draw("_floatRange");
        }
        if (data.TargetMaterialProperty == MaterialPlayable.ETargetMaterialProperty.Vector)
        {
            Draw("_startVector");
            Draw("_endVector");
        }
        if (data.TargetMaterialProperty == MaterialPlayable.ETargetMaterialProperty.Color)
        {
            Draw("_startColor");
            Draw("_endColor");
        }
    }

    void Draw(string propertyName)
    {
        var prop = serializedObject.FindProperty(propertyName);
        if (prop == null)
            return;
        EditorGUILayout.PropertyField(prop, true);
    }
}
