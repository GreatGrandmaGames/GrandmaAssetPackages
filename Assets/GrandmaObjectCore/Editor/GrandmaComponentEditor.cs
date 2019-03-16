using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Grandma
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GrandmaComponent), true)]
    public class GrandmaComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var comp = (GrandmaComponent)target;

            EditorGUILayout.LabelField("Data Settings", EditorStyles.boldLabel);
            comp.initialDataMode = (GrandmaComponent.InitialDataMode)EditorGUILayout.EnumPopup("Initial Data Mode", comp.initialDataMode);

            switch (comp.initialDataMode)
            {
                case GrandmaComponent.InitialDataMode.Provide:
                    comp.initialData = EditorGUILayout.ObjectField(comp.initialData, typeof(GrandmaComponentData), false) as GrandmaComponentData;
                    break;
                case GrandmaComponent.InitialDataMode.Named:
                    comp.dataClassName = EditorGUILayout.TextField("Data Class Name", comp.dataClassName);
                    comp.appendNameSpace = EditorGUILayout.Toggle("Append 'Grandma.' to name", comp.appendNameSpace);
                    break;
            }

            base.OnInspectorGUI();
        }
    }
}
