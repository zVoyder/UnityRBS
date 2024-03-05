namespace RBS.Editor.Utility
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public static class RBSEditorUtility
    {
        /// <summary>
        /// Callback to update the serialized object and apply the modified properties.
        /// </summary>
        /// <param name="target"> The target serialized object. </param>
        /// <param name="onUpdate"> The callback to update the serialized object. </param>
        public static void UpdateProperties(this SerializedObject target, Action onUpdate = null)
        {
            target.Update();
            onUpdate?.Invoke();
            target.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draws an object field with a callback.
        /// </summary>
        /// <param name="property"> The serialized property to draw. </param>
        /// <param name="callback"> The callback to invoke when the object reference is changed. </param>
        /// <typeparam name="T"> The type of the object reference. </typeparam>
        public static void DrawObjectField<T>(SerializedProperty property, Action<T> callback) where T : UnityEngine.Object
        {
            EditorGUILayout.PropertyField(property);
            if (property.objectReferenceValue is T value)
                callback?.Invoke(value);
        }

        /// <summary>
        /// Draws a list of Vector3 fields with a callback.
        /// </summary>
        /// <param name="vectors"> The list of Vector3 values to draw. </param>
        /// <param name="label"> The label to display for each Vector3 field. </param>
        /// <exception cref="ArgumentNullException"> Thrown when the list of Vector3 values is null. </exception>
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

        /// <summary>
        /// Draws a foldout group with a callback.
        /// </summary>
        /// <param name="isShowed"> The state of the foldout group. </param>
        /// <param name="label"> The label to display for the foldout group. </param>
        /// <param name="callback"> The callback to invoke when the foldout group is opened. </param>
        public static void DrawFoldout(ref bool isShowed, string label, Action callback)
        {
            isShowed = EditorGUILayout.BeginFoldoutHeaderGroup(isShowed, label);
            if (isShowed) callback?.Invoke();
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        /// <summary>
        /// Draws a disabled group with a callback.
        /// </summary>
        /// <param name="callback"> The callback to invoke inside the group. </param>
        /// <param name="isDisabled"> The state of the group. </param>
        public static void DrawDisabledGroup(Action callback, bool isDisabled = true)
        {
            EditorGUI.BeginDisabledGroup(isDisabled);
            callback?.Invoke();
            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// Draws a horizontal group with a callback.
        /// </summary>
        /// <param name="callback"> The callback to invoke inside the group. </param>
        /// <param name="options"> The layout options for the group. </param>
        public static void DrawHorizontal(Action callback, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(options);
            callback?.Invoke();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws a vertical group with a callback.
        /// </summary>
        /// <param name="callback"> The callback to invoke inside the group. </param>
        /// <param name="options"> The layout options for the group. </param>
        public static void DrawVertical(Action callback, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(options);
            callback?.Invoke();
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Draws a box with a callback.
        /// </summary>
        /// <param name="color"> The color of the box. </param>
        /// <param name="callback"> The callback to invoke inside the box. </param>
        public static void DrawBox(Color color, Action callback)
        {
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.normal.background = CreateTexture(600, 1, color);
            EditorGUILayout.BeginVertical(boxStyle);
            callback?.Invoke();
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Draws a header label.
        /// </summary>
        /// <param name="label"> The label to display. </param>
        public static void DrawHeaderLabel(string label)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        }

        /// <summary>
        /// Draws a space.
        /// </summary>
        /// <param name="space"> The space to draw. </param>
        public static void DrawSpace(int space = 4)
        {
            EditorGUILayout.Space(space);
        }

        /// <summary>
        /// Draws an indented group with a callback.
        /// </summary>
        /// <param name="indentLevel"> The level of indentation for the group. </param>
        /// <param name="callback"> The callback to invoke inside the group. </param>
        public static void DrawIndented(int indentLevel, Action callback)
        {
            EditorGUI.indentLevel += indentLevel;
            callback?.Invoke();
            EditorGUI.indentLevel -= indentLevel;
        }

        /// <summary>
        /// Creates a texture with a specified width, height, and color.
        /// </summary>
        /// <param name="width"> The width of the texture. </param>
        /// <param name="height"> The height of the texture. </param>
        /// <param name="color"> The color of the texture. </param>
        /// <returns> The created texture. </returns>
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

        /// <summary>
        /// Draws an auto-resizable selection grid in an editor window.
        /// </summary>
        /// <param name="window"> The editor window to draw the selection grid. </param>
        /// <param name="labels"> The labels to display for each button. </param>
        /// <param name="selectedIndex"> The index of the selected button. </param>
        /// <param name="buttonsLength"> The length of the buttons. </param>
        public static void DrawSelectionGrid(EditorWindow window, string[] labels, ref int selectedIndex, int buttonsLength = 125)
        {
            GUIStyle toolbarButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            int xCount = (int)(window.position.width / buttonsLength);
            xCount = Mathf.Clamp(xCount, 1, labels.Length);
            selectedIndex = GUILayout.SelectionGrid(selectedIndex, labels, xCount, toolbarButtonStyle /*, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)*/);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            DrawSpace(labels.Length * 15 / xCount);
        }
    }
}