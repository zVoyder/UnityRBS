namespace RBS.Editor.Utility
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public static class RBSEditorUtility
    {
        public static void UpdateProperties(this SerializedObject target, Action onUpdate = null)
        {
            target.Update();
            onUpdate?.Invoke();
            target.ApplyModifiedProperties();
        }

        public static void DrawObjectField<T>(SerializedProperty property, Action<T> callback) where T : UnityEngine.Object
        {
            EditorGUILayout.PropertyField(property);
            if (property.objectReferenceValue is T value)
                callback?.Invoke(value);
        }

        public static void DrawVector3ListField(ref List<Vector3> vectors, string label = "")
        {
            if(vectors == null)
                throw new ArgumentNullException(nameof(vectors));
            
            for (int i = 0; i < vectors.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                vectors[i] = EditorGUILayout.Vector3Field(label + " " + i, vectors[i]);

                if (GUILayout.Button("x", GUILayout.Width(20)))
                    vectors.RemoveAt(i);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
                vectors.Add(Vector3.zero);
            if (GUILayout.Button("-") && vectors.Count > 0)
                vectors.RemoveAt(vectors.Count - 1);
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawFoldout(ref bool isShowed, string label, Action callback)
        {
            isShowed = EditorGUILayout.BeginFoldoutHeaderGroup(isShowed, label);
            if (isShowed) callback?.Invoke();
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        public static void DrawDisabledGroup(Action callback, bool isDisabled = true)
        {
            EditorGUI.BeginDisabledGroup(isDisabled);
            callback?.Invoke();
            EditorGUI.EndDisabledGroup();
        }

        public static void DrawHorizontal(Action callback, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(options);
            callback?.Invoke();
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawVertical(Action callback, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(options);
            callback?.Invoke();
            EditorGUILayout.EndVertical();
        }

        public static void DrawBox(Color color, Action callback)
        {
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.normal.background = CreateTexture(600, 1, color);
            EditorGUILayout.BeginVertical(boxStyle);
            callback?.Invoke();
            EditorGUILayout.EndVertical();
        }

        public static void DrawHeaderLabel(string label)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        }

        public static void DrawSpace(int space = 4)
        {
            EditorGUILayout.Space(space);
        }

        public static void DrawIndented(int indentLevel, Action callback)
        {
            EditorGUI.indentLevel += indentLevel;
            callback?.Invoke();
            EditorGUI.indentLevel -= indentLevel;
        }

        public static Texture2D CreateTexture(int width, int height, Color color)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = color;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        public static void DrawSelectionGrid(EditorWindow window, string[] toolNames, ref int selectedIndex, int buttonsLength = 125)
        {
            GUIStyle toolbarButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            int xCount = (int)(window.position.width / buttonsLength);
            xCount = Mathf.Clamp(xCount, 1, toolNames.Length);
            selectedIndex = GUILayout.SelectionGrid(selectedIndex, toolNames, xCount, toolbarButtonStyle /*, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)*/);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            DrawSpace(toolNames.Length * 15 / xCount);
        }
    }
}