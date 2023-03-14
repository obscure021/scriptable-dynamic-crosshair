using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Obscure.SDC.Utilities;

namespace Obscure.SDC {
    [CustomEditor(typeof(Crosshair))]
    public class CrosshairEditor : Editor
    {
        /// <summary>
        /// The main UXML document
        /// </summary>
        VisualTreeAsset uXML;
        /// <summary>
        /// Reference to the target object
        /// </summary>
        Crosshair crosshair => target as Crosshair;
        /// <summary>
        /// Used for debug purposes.
        /// </summary>
        const string documentName = "Crosshair";

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            uXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Obscure045/Scriptable Dynamic Crosshair/Project/Core/Crosshair/Crosshair.uxml");

            // Adding the UI Document
            if (uXML == null)
            {
                Debug.Log("UI Document for " + documentName + " is null");
                return root;
            }

            uXML.CloneTree(root);

            OGU.TitleButtonCallback(root, "Title", crosshair, this, uXML);

            OGU.HeaderPanelsHandler(root, "referenceToggle", "referencePanel", documentName);
            OGU.HeaderPanelsHandler(root, "settingsToggle", "settingsPanel", documentName);
            OGU.HeaderPanelsHandler(root, "debugToggle", "debugPanel", documentName);

            return root;
        }
    }
}

