using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UI_Controller))]
public class EnumNameArrayAttribute : Editor
{
    //TODO: fix shifting references on adding new enum
    public override void OnInspectorGUI()
    {
        UI_Controller ui_Controller = (UI_Controller) target;
        SerializedObject serializedObject = new SerializedObject(ui_Controller);
        serializedObject.Update();
        SerializedProperty screensArrayProperty = serializedObject.FindProperty(nameof(ui_Controller.screens));
  
        for (int i = 0; i < ui_Controller.screens.Length; i++)
        {
            string enumName = System.Enum.GetName(typeof(UI_Controller.Screens), i);
            if (enumName == System.Enum.GetName(typeof(UI_Controller.Screens), UI_Controller.Screens.none))
            {
                continue;
            }
            SerializedProperty currentObject = screensArrayProperty.GetArrayElementAtIndex(i);
            currentObject.objectReferenceValue = EditorGUILayout.ObjectField(enumName, currentObject.objectReferenceValue, typeof(UnityEngine.Object), true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
