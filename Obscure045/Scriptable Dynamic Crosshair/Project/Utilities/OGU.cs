using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Obscure.SDC.Utilities
{
    using UnityEditor;

    public static class OGU
    {
        /// <summary>
        /// Get the first element of type T with the given name in the given root element
        /// </summary>
        /// <param name="root">The Root `VisualElement`</param>
        /// <param name="name">The Name of the Object</param>
        /// <param name="documentName">Optional: The name of the document, to find errors easily</param>
        /// <typeparam name="T">A VisualElement</typeparam>
        /// <returns>The Object of type T with the given class name</returns>
        public static T GetElement<T>(VisualElement root, string name, string documentName = null) where T : VisualElement, new()
        {
            T element = root.Q<T>(name);
            if (element == null)
            {
                string type = typeof(T).ToString();
                type = type.Substring(type.LastIndexOf('.') + 1);
                Debug.LogError(string.Format("{0} \"{1}\" not found in \"{2}{3}\"!", type, name, (root.name == null || root.name == "" ? "root" : root.name), (documentName != null ? " of " + documentName : "")));
                return new T();
            }
            return element;
        }

        /// <summary>
        /// Toggle the DisplayStyle by the given element
        /// </summary>
        /// <param name="e">the element</param>
        /// <returns>The Opposing DisplayStyle to the Current DisplayStyle</returns>
        public static DisplayStyle ToggleDisplay(VisualElement e)
        {
            StyleEnum<DisplayStyle> currentDisplay = e.style.display;
            if (currentDisplay == DisplayStyle.Flex) {
                return DisplayStyle.None;
            }
            else
            {
                return DisplayStyle.Flex;
            }
        }

        /// <summary>
        /// Toggles the display of an element when a certain button is clicked
        /// </summary>
        /// <param name="parent">the root of both objects</param>
        /// <param name="toggleButtonName">the name of the "certain button"</param>
        /// <param name="panelName">the name of the "certain panel"</param>
        /// <param name="documentName">Optional: the name of the document for debug</param>
        public static void HeaderPanelsHandler(VisualElement parent, string toggleButtonName, string panelName, string documentName = "")
        {
            Button toggle = GetElement<Button>(parent, toggleButtonName, documentName);
            VisualElement panel = GetElement<VisualElement>(parent, panelName, documentName);
            panel.style.display = DisplayStyle.Flex;
            toggle.clicked += () =>
            {
                panel.style.display = ToggleDisplay(panel);
            };
        }

        /// <summary>
        /// Used to add a callback to the title button. Enables it to open a context menu including
        /// the option to open the script, open the editor window, and open the UI document
        /// </summary>
        /// <param name="parent">the root of the object</param>
        /// <param name="buttonName">the name of the button</param>
        /// <param name="monoBehaviour">the MonoBehaviour</param>
        /// <param name="editor">the Editor script for MonoBehaviour</param>
        /// <param name="document">the UI Document also referred as UXML</param>
        public static void TitleButtonCallback(VisualElement parent, string buttonName, MonoBehaviour monoBehaviour, Editor editor, VisualTreeAsset document)
        {   
            // Title Button functionality
            Button titleButton = GetElement<Button>(parent, buttonName, document.name);
            titleButton.clicked += () =>
            {
                // opening a context menu
                GenericMenu menu = new GenericMenu();
                // adding options to the menu
                menu.AddItem(new GUIContent("Open Script"), false, () =>
                {
                    // opening the script
                    string scriptPath = AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(monoBehaviour));
                    AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath));
                });
                menu.AddItem(new GUIContent("Open Editor Script"), false, () =>
                {
                    // opening the editor script
                    string editorPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(editor));
                    AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<MonoScript>(editorPath));
                });
                menu.AddItem(new GUIContent("Open UI Document"), false, () =>
                {
                    // opening the UI document
                    string uiPath = AssetDatabase.GetAssetPath(document);
                    AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uiPath));
                });
                menu.ShowAsContext();
            };
        }
    }
}
