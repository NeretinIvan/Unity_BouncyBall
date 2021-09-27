using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//TODO: fix shifting references on adding new enum
[CustomEditor(typeof(UI_Controller))]
public class EnumNameArrayAttribute : Editor
{
    private SerializedProperty screensArrayProperty;
    private SerializedObject mainScript;
    private UI_Controller ui_Controller;

    public override void OnInspectorGUI()
    {
        Init();
        mainScript.Update();

        for (int i = 0; i < ui_Controller.screens.Length; i++)
        {
            string enumName = System.Enum.GetName(typeof(UI_Controller.Screens), i);
            if (NameIsEqualToEnum(enumName, UI_Controller.Screens.none))
            {
                continue;
            }
            SetArrayField(i, enumName);
        }

        mainScript.ApplyModifiedProperties();
    }

    private void SetArrayField(int index, string caption)
    {
        ui_Controller.screens[index] = EditorGUILayout.ObjectField(caption, ui_Controller.screens[index], typeof(UnityEngine.Object), true) as GameObject;
    }

    private bool NameIsEqualToEnum(string name, UI_Controller.Screens screensEnum)
    {
        return name == System.Enum.GetName(typeof(UI_Controller.Screens), screensEnum);
    }

    private void Init()
    {
        ui_Controller = (UI_Controller)target;
        mainScript = new SerializedObject(ui_Controller);
        screensArrayProperty = mainScript.FindProperty(nameof(ui_Controller.screens));   
    }
}
