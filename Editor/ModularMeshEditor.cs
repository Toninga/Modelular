using Modelular.Runtime;
using UnityEditor;
using UnityEngine;

namespace Modelular.Editor
{

    [CustomEditor(typeof(ModularMesh))]
    public class ModularMeshEditor : UnityEditor.Editor
    {
        private string[] _tabs = { "General", "Modifiers", "Visualization" };
        private int _tabIndex = 1;
        private bool _requireSave;
        public override void OnInspectorGUI()
        {

            EditorGUI.BeginChangeCheck();

            ModularMesh data = (ModularMesh)target;

            _tabIndex = GUILayout.Toolbar(_tabIndex, _tabs);

            GUILayout.Space(15);

            switch (_tabIndex)
            {
                case 0:
                    DrawGeneralTab(data);
                    break;
                case 1:
                    DrawModifiersTab(data);
                    break;
                case 2:
                    DrawVisualizationTab(data);
                    break;
            }

            // Modifier list
            SerializedObject obj = new SerializedObject(data);
            EditorGUILayout.PropertyField(obj.FindProperty("modifiers"));


            GUILayout.Space(15);

            GUIStyle categoryStyle = EditorStyles.boldLabel;

        

        

            obj.ApplyModifiedProperties();
        }

        private void DrawGeneralTab(ModularMesh data)
        {
            GUILayout.Space(10);
            data.updateMode = (ModularMesh.EUpdateMode)EditorGUILayout.EnumPopup("Update mode", data.updateMode);
        }

        private void DrawModifiersTab(ModularMesh data)
        {
            GUILayout.Label("Last baked " + Mathf.Round((float)(EditorApplication.timeSinceStartup - data.LastBakingTime)) + "s ago", EditorStyles.boldLabel);


            // ###################################### BUTTONS
            GUILayout.BeginHorizontal();
            // REFRESH BUTTON
            if (GUILayout.Button("Refresh"))
            {
                data.ApplyModifierStack();
            }
            // CLEAR STACK BUTTON
            if (GUILayout.Button("Clear stack"))
            {
                data.ClearModifierStack();
                data.ApplyModifierStack();
            }
            // SAVE BUTTON
            if (GUILayout.Button("Save file"))
            {
                _requireSave = true;
            }
            GUILayout.EndHorizontal();

            if (_requireSave)
            {
                _requireSave = false;
                data.ApplyModifierStack();
                data.Save();
            }

            GUILayout.Space(15);

            // ADD MODIFIER button
            GUILayout.ExpandWidth(true);
            Undo.RecordObject(data, "Add modifier");
            if (GUILayout.Button("Add new modifier"))
            {
                GenericMenu menu = new GenericMenu();

                foreach (var kvp in ModifierModelList.ModifierPaths)
                {
                    menu.AddItem(new GUIContent(kvp.Key), false, () => AddModifier(data, kvp.Value));
                }

                menu.ShowAsContext();
            
            }
        }

        private void DrawVisualizationTab(ModularMesh data)
        {
            GUILayout.Space(5);


            data.ShowVertices = GUILayout.Toggle(data.ShowVertices, "Show vertices");
            if (data.ShowVertices)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                data.ShowVertexNumber = GUILayout.Toggle(data.ShowVertexNumber, "Show vertex number");
                if (data.ShowVertexNumber)
                {
                    data.VertexNumberDistance = EditorGUILayout.Slider("Distance", data.VertexNumberDistance, 0f, 0.1f);
                }
                data.VertexDisplaySize = EditorGUILayout.Slider("Size", data.VertexDisplaySize, 0f, 1f);
                data.VertexDisplayMode = (EColorCoding)EditorGUILayout.EnumPopup("Color coding", data.VertexDisplayMode);
                if (data.VertexDisplayMode == EColorCoding.Custom)
                    data.VertexColor = EditorGUILayout.ColorField("Color", data.VertexColor);
                GUILayout.EndVertical();
            }

            data.ShowFaces = GUILayout.Toggle(data.ShowFaces, "Show faces");
            if (data.ShowFaces)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                data.FaceDisplayMode = (EColorCoding)EditorGUILayout.EnumPopup("Color coding", data.FaceDisplayMode);
                if (data.FaceDisplayMode == EColorCoding.Custom)
                    data.FaceColor = EditorGUILayout.ColorField("Color", data.FaceColor);
                GUILayout.EndVertical();
            }
            data.ShowNormals = GUILayout.Toggle(data.ShowNormals, "Show normals");
            if (data.ShowNormals)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                data.NormalDisplaySize = EditorGUILayout.Slider("Length", data.NormalDisplaySize, 0f, 1f);
                data.NormalDisplayMode = (EColorCoding)EditorGUILayout.EnumPopup("Color coding", data.NormalDisplayMode);
                if (data.NormalDisplayMode == EColorCoding.Custom)
                    data.NormalColor = EditorGUILayout.ColorField("Color", data.NormalColor);

                GUILayout.EndVertical();
            
            }
        }

        private void AddModifier(ModularMesh data, System.Type modifierType)
        {
            data.AddModifier(modifierType);
            data.ApplyModifierStack();
        }
    }

}