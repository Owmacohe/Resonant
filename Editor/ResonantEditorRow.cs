#if UNITY_EDITOR

using System;
using System.Linq;
using System.Reflection;
using Resonant.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace Resonant.Editor
{
    public class ResonantEditorRow : VisualElement
    {
        readonly string DISALLOWED_CHARACTERS = ",\\/\n\t\r"; // Characters not allowed in the entry fields
        bool isEven;
        
        /// <summary>
        /// Returns a string, without the disallowed characters
        /// </summary>
        /// <param name="value">The string to be checked</param>
        /// <returns>A validated string, with no disallowed characters</returns>
        string ValidateInputValue(string value) => new(value.Where(c => !DISALLOWED_CHARACTERS.Contains(c)).ToArray());
        
        public ResonantEditorRow(ResonantTrigger trigger, ResonantBehaviour behaviour, Action saveAction)
        {
            AddToClassList("flex_row");

            VisualElement triggerRow = new();
            triggerRow.AddToClassList("flex_row");
            triggerRow.AddToClassList("trigger");
            Add(triggerRow);
            
            Button remove = new();
            remove.text = "-";
            triggerRow.Add(remove);

            TextElement when = new();
            when.text = "When";
            triggerRow.Add(when);

            TextField triggerID = new();
            triggerID.value = trigger.ID;
            triggerRow.Add(triggerID);

            TextElement arrow = new();
            arrow.text = "->";
            triggerRow.Add(arrow);

            VisualElement reactions = new();
            reactions.AddToClassList("flex_column");
            reactions.AddToClassList("reactions");
            Add(reactions);

            VisualElement addReactionRow = new();
            addReactionRow.AddToClassList("flex_row");
            addReactionRow.AddToClassList("reaction_add");
            reactions.Add(addReactionRow);
            
            Button addReaction = new();
            addReaction.text = "Add reaction";
            addReactionRow.Add(addReaction);

            remove.clicked += () =>
            {
                behaviour.Triggers.Remove(trigger);
                ResonantEditorUtilities.RemoveElement(this);
                
                saveAction?.Invoke();
            };

            var reactionTypes = Assembly.GetAssembly(typeof(ResonantReaction)).GetTypes()
                .Where(type => type.IsSubclassOf(typeof(ResonantReaction)))
                .ToList();
            
            DropdownField reactionDropdown = new DropdownField(reactionTypes
                .Select(type => type.ToString().Split('.')[^1])
                .ToList(), 0);
            addReactionRow.Add(reactionDropdown);
            
            foreach (var i in trigger.Reactions)
                AddReaction(i, reactions, trigger, saveAction);

            triggerID.RegisterValueChangedCallback(evt =>
            {
                var validated = ValidateInputValue(evt.newValue);
                triggerID.SetValueWithoutNotify(validated);

                trigger.ID = validated;
                
                saveAction?.Invoke();
            });

            addReaction.clicked += () =>
            {
                var reactionType = reactionTypes[reactionDropdown.index];
                var newReaction = (ResonantReaction)Activator.CreateInstance(reactionType);
                AddReaction(newReaction, reactions, trigger, saveAction);
                
                trigger.Reactions.Add(newReaction);
                
                saveAction?.Invoke();
            };
        }

        void AddReaction(ResonantReaction reaction, VisualElement parent, ResonantTrigger trigger, Action saveAction)
        {
            isEven = !isEven;
            
            VisualElement root = new();
            root.AddToClassList("flex_row");
            root.AddToClassList("reaction");
            root.AddToClassList("reaction_" + (isEven ? "a" : "b"));
            parent.Add(root);

            Button remove = new();
            remove.text = "-";
            root.Add(remove);
            
            TextElement name = new();
            name.text = "<b>" + reaction.GetType().ToString().Split('.')[^1] + "</b>";
            name.AddToClassList("reaction_name");
            root.Add(name);

            remove.clicked += () =>
            {
                trigger.Reactions.Remove(reaction);
                ResonantEditorUtilities.RemoveElement(root);
                
                saveAction?.Invoke();
            };

            foreach (var i in reaction.GetType().GetFields())
            {
                string fieldNameFormat = "<i>({0})</i> " + i.Name + ":";
                TextField field = new();
                if (i.GetValue(reaction) != null) field.value = i.GetValue(reaction).ToString();
                root.Add(field);
                
                if (i.FieldType == typeof(int))
                {
                    field.label = string.Format(fieldNameFormat, "int");
                    
                    field.RegisterValueChangedCallback(evt =>
                    {
                        string value = ValidateInputValue(evt.newValue);
                        field.SetValueWithoutNotify(value);
                        i.SetValue(reaction, int.Parse(string.IsNullOrEmpty(value) ? "0" : value));
                
                        saveAction?.Invoke();
                    });
                }
                else if (i.FieldType == typeof(float))
                {
                    field.label = string.Format(fieldNameFormat, "float");
                    
                    field.RegisterValueChangedCallback(evt =>
                    {
                        string value = ValidateInputValue(evt.newValue);
                        field.SetValueWithoutNotify(value);
                        i.SetValue(reaction, float.Parse(string.IsNullOrEmpty(value) ? "0" : value));
                
                        saveAction?.Invoke();
                    });
                }
                else if (i.FieldType == typeof(string))
                {
                    field.label = string.Format(fieldNameFormat, "string");

                    field.RegisterValueChangedCallback(evt =>
                    {
                        string value = ValidateInputValue(evt.newValue);
                        i.SetValue(reaction, value);
                
                        saveAction?.Invoke();
                    });
                }
            }
        }
    }
}

#endif