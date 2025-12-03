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
            Undo.RecordObject(data, "Edit modular mesh");
            bool requestRebake = false;
            DrawModifierParameters(data);

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
                requestRebake = true;
            }
            // SAVE BUTTON
            if (GUILayout.Button("Save file"))
            {
                _requireSave = true;
            }
            GUILayout.EndHorizontal();


            GUILayout.Space(15);

            GUILayout.BeginHorizontal();
            // ADD MODIFIER button
            GUILayout.ExpandWidth(true);
            if (GUILayout.Button("Add new modifier"))
            {
                GenericMenu menu = new GenericMenu();

                foreach (var kvp in ModifierModelList.ModifierPaths)
                {
                    menu.AddItem(new GUIContent(kvp.Key), false, () => AddModifier(data, kvp.Value));
                }

                menu.ShowAsContext();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Delete selected modifier"))
            {
                if (_selectedModifier < data.Modifiers.Count)
                {
                    data.Modifiers.RemoveAt(_selectedModifier);
                    if (_selectedModifier == data.Modifiers.Count)
                        _selectedModifier = data.Modifiers.Count - 1;
                }
            }
            if (GUILayout.Button("Move up"))
            {
                if (_selectedModifier < data.Modifiers.Count && _selectedModifier > 0)
                {
                    data.Modifiers.Insert(_selectedModifier - 1, data.Modifiers[_selectedModifier]);
                    data.Modifiers.RemoveAt(_selectedModifier + 1);
                    _selectedModifier--;
                    requestRebake = true;
                }
            }
            if (GUILayout.Button("Move down"))
            {
                if (_selectedModifier < data.Modifiers.Count - 1)
                {
                    data.Modifiers.Insert(_selectedModifier + 2, data.Modifiers[_selectedModifier]);
                    data.Modifiers.RemoveAt(_selectedModifier);
                    _selectedModifier++;
                    requestRebake = true;
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
                    name = i.ToString() + ": " + name;

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


            if (requestRebake)
                data.ApplyModifierStack();

            if (_requireSave)
            {
                _requireSave = false;
                data.ApplyModifierStack();
                data.Save();
            }
        }

        private void DrawModifierParameters(ModularMesh data)
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

                // Draw icon
                if (EditorGUIUtility.GetIconForObject(modifier))
                {
                    var icon = EditorGUIUtility.GetIconForObject(modifier);
                    if (icon != null)
                    {
                        Rect position = new Rect(6, EditorGUIUtility.singleLineHeight + 3, 24, 24);
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

                // Automatic property layout
                obj.UpdateIfRequiredOrScript();
                SerializedProperty iterator = obj.GetIterator();
                bool enterChildren = true;
                EditorGUI.indentLevel++;
                while (iterator.NextVisible(enterChildren))
                {
                    if ("m_Script" != iterator.propertyPath)
                    {
                        EditorGUILayout.PropertyField(iterator, true);
                    }

                    enterChildren = false;
                }

                EditorGUI.indentLevel--;
                obj.ApplyModifiedProperties();
            }
            GUILayout.EndScrollView();
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
            var mod = data.MakeNewModifier(modifierType);
            if (_selectedModifier >= 0 && _selectedModifier < data.Modifiers.Count)
            {
                data.Modifiers.Insert(_selectedModifier +1, mod);
                _selectedModifier = _selectedModifier + 1;
            }
            else
            {
                data.Modifiers.Add(mod);
                _selectedModifier = data.Modifiers.Count - 1;
            }
            data.ApplyModifierStack();
            
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