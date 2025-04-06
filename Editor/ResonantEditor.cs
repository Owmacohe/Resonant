﻿#if UNITY_EDITOR

using System;
using Resonant.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace Resonant.Editor
{
    public class ResonantEditor : EditorWindow
    {
        ResonantBehaviour Behaviour;

        VisualElement topRow;
        TextElement unsaved;
        ScrollView scroll;
        
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

            topRow = new VisualElement();
            topRow.AddToClassList("flex_row");
            topRow.AddToClassList("top_row");
            rootVisualElement.Add(topRow);

            TextElement title = new();
            title.text = Behaviour.name;
            title.AddToClassList("title");
            topRow.Add(title);
            
            Button addTrigger = new();
            addTrigger.text = "Add trigger";
            topRow.Add(addTrigger);
            
            Button save = new();
            save.text = "Save";
            topRow.Add(save);

            unsaved = new();
            unsaved.text = "*";
            unsaved.AddToClassList("unsaved");
            topRow.Add(unsaved);

            scroll = new();
            rootVisualElement.Add(scroll);
            
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

        void ClearGUI()
        {
            if (topRow != null) rootVisualElement.Remove(topRow);
            if (scroll != null) rootVisualElement.Remove(scroll);
        }

        void AddTrigger(ResonantTrigger trigger)
        {
            scroll.Add(new ResonantTriggerRow(trigger, Behaviour, Save, () => { SetUnsaved(true); }));
        }

        void Save()
        {
            ResonantEditorUtilities.SaveSerializedObject(Behaviour);
            SetUnsaved(false);
        }

        void SetUnsaved(bool value)
        {
            unsaved.style.display = new StyleEnum<DisplayStyle>(value ? DisplayStyle.Flex : DisplayStyle.None);
        }

        public void Load(ResonantBehaviour behaviour)
        {
            ClearGUI();
            
            Behaviour = behaviour;
            
            CreateGUI();
        }
    }
}

#endif