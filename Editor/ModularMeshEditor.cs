using Modelular.Runtime;
using System.Net;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;

namespace Modelular.Editor
{

    [CustomEditor(typeof(ModularMesh))]
    public class ModularMeshEditor : UnityEditor.Editor
    {
        private string[] _tabs = { "General", "Modifiers", "Visualization" };
        private int _tabIndex = 1;
        private bool _requireSave;
        private Vector2 _position = Vector2.zero;
        private int _selectedModifier = 0;
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

        }

        private void DrawGeneralTab(ModularMesh data)
        {
            GUILayout.Space(10);
            data.UpdateMode = (ModularMesh.EUpdateMode)EditorGUILayout.EnumPopup("Update mode", data.UpdateMode);
            if (data.UpdateMode == ModularMesh.EUpdateMode.Manual)
                data.IgnoreVertexLimits = EditorGUILayout.Toggle("Ignore vertex limits", data.IgnoreVertexLimits);
        }

        private void DrawModifiersTab(ModularMesh data)
        {

            GUIStyle darkBackground = new GUIStyle();
            darkBackground.normal.background = MakeTex(1, 1, new Color(0.16f, 0.16f, 0.16f, 1f));
            _position = GUILayout.BeginScrollView(_position, darkBackground, GUILayout.Height(200));
            if (data.Modifiers != null && data.Modifiers.Count > _selectedModifier)
            {
                var modifier = data.Modifiers[_selectedModifier];
                SerializedObject obj = new SerializedObject(modifier);

                string name = modifier.GetType().Name;
                name = name.Replace("Model", "");
                GUILayout.Label(name, EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();

                if (EditorGUIUtility.GetIconForObject(modifier))
                {
                    var icon = EditorGUIUtility.GetIconForObject(modifier);
                    if (icon != null)
                    {
                        Rect position = new Rect(6,EditorGUIUtility.singleLineHeight+  3,24,24);
                        if (icon.alphaIsTransparency)
                        {
                            GUI.DrawTexture(position, icon);
                            //EditorGUI.DrawTextureTransparent(position, icon);
                        }
                        else
                        {
                            EditorGUI.DrawPreviewTexture(position, icon);
                        }

                        EditorGUILayout.Space(26);
                    }
                }
                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();

                obj.UpdateIfRequiredOrScript();
                SerializedProperty iterator = obj.GetIterator();
                bool enterChildren = true;
                while (iterator.NextVisible(enterChildren))
                {
                    if ("m_Script" != iterator.propertyPath)
                    {
                        EditorGUILayout.PropertyField(iterator, true);
                    }

                    enterChildren = false;
                }

                obj.ApplyModifiedProperties();
            }
            GUILayout.EndScrollView();

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

            GUILayout.BeginHorizontal();
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
            if (GUILayout.Button("Delete selected modifier"))
            {
                if (_selectedModifier < data.Modifiers.Count)
                {
                    data.Modifiers.RemoveAt(_selectedModifier);
                }
            }

            GUILayout.EndHorizontal();


            GUIStyle greyBackground = new GUIStyle();
            greyBackground.normal.background = MakeTex(1, 1, new Color(0.18f, 0.18f, 0.18f, 1f));
            GUILayout.BeginVertical(greyBackground);
            for (int i = 0; i < data.Modifiers.Count; i++)
            {
                var modifier = data.Modifiers[i];
                if (modifier != null)
                {
                    string name = modifier.GetType().Name;
                    name = name.Replace("Model", "");

                    GUIStyle style;
                    if (_selectedModifier == i)
                    {
                        style = new GUIStyle(EditorStyles.boldLabel);
                        style.normal.textColor = new Color(1f, 0.76f, 0f, 1f);
                        style.hover.textColor = new Color(1f, 0.85f, 0.5f, 1f);
                    }
                    else
                    {
                        style = new GUIStyle(EditorStyles.label);
                        style.normal.textColor = Color.white;
                    }

                    if (GUILayout.Button(name, style))
                    {
                        _selectedModifier = i;
                    }
                }
            }
            GUILayout.EndVertical();

            // Modifier list
            /*
            SerializedObject obj = new SerializedObject(data);
            EditorGUILayout.PropertyField(obj.FindProperty("Modifiers"));
            obj.ApplyModifiedProperties();
            */
        }

        private void DrawVisualizationTab(ModularMesh data)
        {
            GUILayout.Space(5);

            GUILayout.Label("Visualization can impact editor performances");

            GUILayout.Space(10);

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
            GUILayout.Space(10);
        }

        private void AddModifier(ModularMesh data, System.Type modifierType)
        {
            data.AddModifier(modifierType);
            data.ApplyModifierStack();
            _selectedModifier = data.Modifiers.Count - 1;
        }
        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }


        // #########################################################################################################
        // #####################################        NAUGHTY ATTRIBUTES      ####################################
        // #########################################################################################################

        
    }

}