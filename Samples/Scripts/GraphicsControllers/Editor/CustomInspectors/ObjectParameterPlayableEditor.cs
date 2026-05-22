using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectParameterPlayable))]
public class ObjectParameterPlayableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var data = (ObjectParameterPlayable)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Add parameter from component"))
        {
            GenericMenu menu = new GenericMenu();

            bool selectedObject = Selection.activeGameObject != null && Selection.activeGameObject != data.gameObject;

            AddMenuItemsFromGameObject
                (
                ref menu, 
                data.gameObject, 
                data.AnimatedParameters,
                data.name + " (current)", 
                data.GetSupportedTypes()
                );

            if (selectedObject)
                AddMenuItemsFromGameObject
                    (
                    ref menu, 
                    Selection.activeGameObject, 
                    data.AnimatedParameters, 
                    Selection.activeGameObject.name + " (selected)", 
                    data.GetSupportedTypes()
                    );

            menu.ShowAsContext();
        }
    }
    /// <summary>
    /// Takes a gameobject, finds all the parameters of it's components, and adds the corresponding buttons to the menu,
    /// which in turn will add the parameter reference to the targetParameterList
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="go"></param>
    /// <param name="targetParameterList"></param>
    /// <param name="group"></param>
    void AddMenuItemsFromGameObject(ref GenericMenu menu, GameObject go, List<ObjectParameterPlayable.AnimatedParameter> targetParameterList, string group="", IEnumerable<Type> supportedParameterTypes=null)
    {
        IEnumerable<UnityEngine.Object> sourceObjects = go.GetComponents<Component>();

        var configurableMembers = GetConfigurableParametersFromObjects(sourceObjects, supportedParameterTypes);

        foreach (var menuItem in GetMenuItemsContent(configurableMembers))
        {
            menu.AddItem(new GUIContent(group + ((group != "") ? "/" : "") + menuItem.label), menuItem.selected, () => targetParameterList.Add(new(menuItem.param.Name, menuItem.param.Target, menuItem.param.Member)));
        }
    }
    /// <summary>
    /// Retrieves all the data necessary to make a menu item from the available target members
    /// </summary>
    /// <param name="members"></param>
    /// <param name="alreadyAddedMembers"></param>
    /// <returns></returns>
    private List<(string label, bool selected, ObjectParameterPlayable.AnimatedParameter param)> GetMenuItemsContent(Dictionary<UnityEngine.Object, List<MemberInfo>> members, IEnumerable<MemberInfo> alreadyAddedMembers=null)
    {
        List<(string label, bool selected, ObjectParameterPlayable.AnimatedParameter param)> result = new();
        Dictionary<Type, int> typeOccurenceCount = new();
        foreach (var kvp in members)
        {
            Type parentType = kvp.Key.GetType();
            if (!typeOccurenceCount.ContainsKey(parentType))
                typeOccurenceCount[parentType] = 0;
            typeOccurenceCount[parentType]++;
            foreach (var member in kvp.Value)
            {
                // Assumes the members are either Fields or Properties. If they are anything else, it breaks.
                Type fieldType = member is FieldInfo ? (member as FieldInfo).FieldType : (member as PropertyInfo).PropertyType;
                var itemName = parentType.Name + " (" + (typeOccurenceCount[parentType]-1) + ")/" + member.Name + " (" + PrettyTypeName(fieldType) + ")";

                bool isContained;
                if (alreadyAddedMembers == null)
                    isContained = false;
                else
                    isContained = alreadyAddedMembers.Where(x => x == member).Count() > 0;

                result.Add((itemName, isContained, new(member.Name, kvp.Key, member)));
            }
        }

        return result;
    }
    /// <summary>
    /// Retrieves all the valid parameters from the given objects.
    /// </summary>
    /// <param name="sourceObjects"></param>
    /// <param name="supportedParameterTypes"></param>
    /// <returns></returns>
    private Dictionary<UnityEngine.Object, List<MemberInfo>> GetConfigurableParametersFromObjects(IEnumerable<UnityEngine.Object> sourceObjects, IEnumerable<Type> supportedParameterTypes = null)
    {
        Dictionary<UnityEngine.Object, List<MemberInfo>> result = new();
        foreach (var obj in sourceObjects)
        {
            if (!result.ContainsKey(obj))
                result.Add(obj, new List<MemberInfo>());

            result[obj].AddRange(GetConfigurableParameters(obj.GetType(), supportedParameterTypes));
        }
        return result;
    }
    /// <summary>
    /// Scans all the fields and properties for the given type, and returns every one of them that can be publicly set.
    /// </summary>
    /// <param name="targetType"></param>
    /// <param name="supportedParameterTypes">The list of field/property types that should be added to the resulting list. Other member types will be ignored.
    /// If supportedParameterTypes is null, it will be ignored and all fields/properties that can be set will be included.</param>
    /// <returns></returns>
    private List<MemberInfo> GetConfigurableParameters(Type targetType, IEnumerable<Type> supportedParameterTypes=null)
    {
        var fields = targetType.GetFields();
        var ppties = targetType.GetProperties();

        List<MemberInfo> result = new();
        for (int i = 0; i < fields.Length; i++)
        {
            // Adding a parameter which's type isn't supported is not allowed
            if (supportedParameterTypes != null && !supportedParameterTypes.Contains(fields[i].FieldType)) continue;

            result.Add(fields[i]);
        }
        for (int i = 0; i < ppties.Length; i++)
        {
            // Adding a parameter which's type isn't supported is not allowed
            if (supportedParameterTypes != null && !supportedParameterTypes.Contains(ppties[i].PropertyType)) continue;

            // Adding a parameter that cannot be modified (has no setter) is not allowed
            bool hasSetter = false;
            foreach (MethodInfo accessor in ppties[i].GetAccessors())
            {
                if (accessor.ReturnType == typeof(void))
                {
                    hasSetter = true;
                    break;
                }
            }
            if (!hasSetter) continue;

            result.Add(ppties[i]);
        }

        return result;
    }

    string PrettyTypeName(Type type)
    {
        if (type == null) return "Null";
        if (type == typeof(float)) return "float";
        if (type == typeof(int)) return "int";
        if (type == typeof(bool)) return "bool";
        return type.Name;
    }
}
