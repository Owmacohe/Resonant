#if UNITY_EDITOR

using System;
using Resonant.Runtime;
using UnityEditor;
using UnityEngine.UIElements;

namespace Resonant.Editor
{
    public class ResonantEditor : EditorWindow
    {
        ResonantBehaviour Behaviour;
        
        [MenuItem("Window/Resonant/Editor"), MenuItem("Tools/Resonant/Editor")]
        public static void Open()
        {
            GetWindow<ResonantEditor>("Resonant Editor");
        }

        void CreateGUI()
        {
            ResonantEditorUtilities.AddStyleSheet(rootVisualElement.styleSheets, "ResonantEditorStyleSheet");

            if (Behaviour == null) return;
            
            Button addTrigger = new();
            addTrigger.text = "+";
            addTrigger.style.width = new StyleLength(22);
            rootVisualElement.Add(addTrigger);
            
            foreach (var i in Behaviour.Triggers) AddTrigger(i);
                
            addTrigger.clicked += () =>
            {
                var newTrigger = new ResonantTrigger();
                Behaviour.Triggers.Add(newTrigger);
                
                AddTrigger(newTrigger);
                Save();
            };
        }

        void AddTrigger(ResonantTrigger trigger)
        {
            rootVisualElement.Add(new ResonantEditorRow(trigger, Save));
        }

        void Save()
        {
            ResonantEditorUtilities.SaveSerializedObject(Behaviour);
        }

        public void Load(ResonantBehaviour behaviour)
        {
            Behaviour = behaviour;
            
            CreateGUI();
        }
    }
}

#endif