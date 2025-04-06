#if UNITY_EDITOR

using System;
using Resonant.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace Resonant.Editor
{
    /// <summary>
    /// The editor window for ResonantBehaviours
    /// </summary>
    public class ResonantEditor : EditorWindow
    {
        ResonantBehaviour Behaviour; // The behaviour currently being edited

        VisualElement toolbar; // The toolbar row at the top of the window
        TextElement unsaved; // A small icon that appears when there is unsaved data
        ScrollView scroll; // The scrolling view, containing all the rows of the triggers and reactions
        
        [MenuItem("Window/Resonant/Editor"), MenuItem("Tools/Resonant/Editor")]
        public static void Open()
        {
            GetWindow<ResonantEditor>("Resonant Editor");
        }

        void CreateGUI()
        {
            ResonantEditorUtilities.AddStyleSheet(rootVisualElement.styleSheets, "ResonantEditorStyleSheet");

            if (Behaviour == null) return;

            rootVisualElement.style.backgroundColor = new StyleColor(new Color(0.157f, 0.157f, 0.157f));

            toolbar = new VisualElement();
            toolbar.AddToClassList("flex_row");
            toolbar.AddToClassList("top_row");
            rootVisualElement.Add(toolbar);

            // The title of the behaviour being edited
            TextElement title = new();
            title.text = Behaviour.name;
            title.AddToClassList("title");
            toolbar.Add(title);
            
            // The button to add new triggers to the behaviour
            Button addTrigger = new();
            addTrigger.text = "Add trigger";
            toolbar.Add(addTrigger);
            
            // The button to save the changes to the behaviour
            Button save = new();
            save.text = "Save";
            toolbar.Add(save);

            // The icon to indicate unsaved changes
            unsaved = new();
            unsaved.text = "*";
            unsaved.AddToClassList("unsaved");
            toolbar.Add(unsaved);

            scroll = new();
            rootVisualElement.Add(scroll);
            
            // Adding any pre-existing triggers
            foreach (var i in Behaviour.Triggers) AddTrigger(i);
            
            addTrigger.clicked += () =>
            {
                var newTrigger = new ResonantTrigger();
                Behaviour.Triggers.Add(newTrigger);
                
                AddTrigger(newTrigger);
                Save();
            };

            save.clicked += Save;

            SetUnsaved(false);
        }

        /// <summary>
        /// Removes the toolbar and scroll view
        /// </summary>
        void ClearGUI()
        {
            if (toolbar != null) rootVisualElement.Remove(toolbar);
            if (scroll != null) rootVisualElement.Remove(scroll);
        }

        /// <summary>
        /// Adds a trigger row to the scroll view
        /// </summary>
        /// <param name="trigger">The trigger data to be loaded</param>
        void AddTrigger(ResonantTrigger trigger)
        {
            scroll.Add(new ResonantTriggerRow(trigger, Behaviour, Save, () => { SetUnsaved(true); }));
        }

        /// <summary>
        /// Saves the current ResonantBehaviour
        /// </summary>
        void Save()
        {
            ResonantEditorUtilities.SaveSerializedObject(Behaviour);
            SetUnsaved(false);
        }

        /// <summary>
        /// Sets the on/off value for the unsaved icon
        /// </summary>
        void SetUnsaved(bool value)
        {
            unsaved.style.display = new StyleEnum<DisplayStyle>(value ? DisplayStyle.Flex : DisplayStyle.None);
        }

        /// <summary>
        /// Loads a new ResonantBehaviour into the editor
        /// </summary>
        public void Load(ResonantBehaviour behaviour)
        {
            ClearGUI();
            
            Behaviour = behaviour;
            
            CreateGUI();
        }
    }
}

#endif