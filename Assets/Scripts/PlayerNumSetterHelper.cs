using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(PlayerNumSetterHelper))]
public class PlayerNumSetterHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (PlayerNumSetterHelper) target;

        if (GUILayout.Button("Set In children"))
        {
            script.DoIt();
        }
    }
}
#endif

public class PlayerNumSetterHelper : MonoBehaviour
{
    #if UNITY_EDITOR
    [SerializeField] private PlayerNum _newValue;
    
    public void DoIt()
    {
        var componentDatas = GetMonos().Select(GetFieldInfosTuple)
            .Where(data => data.fieldInfos.Any());
        
        foreach (var (component, fieldInfos) in componentDatas)
        {
            foreach (var fieldInfo in fieldInfos)
            {
                Debug.Log($"Setting {component.GetType()}.{fieldInfo.Name} = {_newValue}");
                fieldInfo.SetValue(component, _newValue);
            }
            
            Debug.Log($"Setting {component} dirty");
            EditorUtility.SetDirty(component);
        }
    }

    private static (MonoBehaviour component, IEnumerable<FieldInfo> fieldInfos) GetFieldInfosTuple(MonoBehaviour mono)
    {
        var fieldInfos = mono.GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        var relevantFieldInfos = fieldInfos.Where(info => info.FieldType == typeof(PlayerNum));
        
        return (
            component: mono,
            fieldInfos: relevantFieldInfos
        );
    }

    private IEnumerable<MonoBehaviour> GetMonos()
    {
        return GetComponents<MonoBehaviour>()
            .Union(GetComponentsInChildren<MonoBehaviour>());
    }
    
    #endif
}