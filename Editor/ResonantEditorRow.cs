using System;
using System.Collections.Generic;
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
        
        /// <summary>
        /// Returns a string, without the disallowed characters
        /// </summary>
        /// <param name="value">The string to be checked</param>
        /// <returns>A validated string, with no disallowed characters</returns>
        string ValidateInputValue(string value) => new(value.Where(c => !DISALLOWED_CHARACTERS.Contains(c)).ToArray());
        
        public ResonantEditorRow(ResonantTrigger trigger, Action saveAction)
        {
            AddToClassList("flex_row");

            TextElement when = new();
            when.text = "When";
            Add(when);

            TextField triggerID = new();
            triggerID.value = trigger.ID;
            Add(triggerID);

            TextElement arrow = new();
            arrow.text = "->";
            Add(arrow);

            VisualElement reactions = new();
            reactions.AddToClassList("flex_column");
            Add(reactions);

            VisualElement addReactionRow = new();
            addReactionRow.AddToClassList("flex_row");
            reactions.Add(addReactionRow);
            
            Button addReaction = new();
            addReaction.text = "+";
            addReactionRow.Add(addReaction);

            var reactionTypes = Assembly.GetAssembly(typeof(ResonantReaction)).GetTypes()
                .Where(type => type.IsSubclassOf(typeof(ResonantReaction)))
                .ToList();
            
            DropdownField reactionDropdown = new DropdownField(reactionTypes
                .Select(type => type.ToString().Split('.')[^1])
                .ToList(), 0);
            addReactionRow.Add(reactionDropdown);
            
            foreach (var i in trigger.Reactions)
                AddReaction(i, reactions, saveAction);

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
                AddReaction(newReaction, reactions, saveAction);
                
                trigger.Reactions.Add(newReaction);
                
                saveAction?.Invoke();
            };
        }

        void AddReaction(ResonantReaction reaction, VisualElement parent, Action saveAction)
        {
            VisualElement root = new();
            root.AddToClassList("flex_row");
            parent.Add(root);
            
            TextElement name = new();
            name.text = reaction.GetType().ToString().Split('.')[^1];
            root.Add(name);

            foreach (var i in reaction.GetType().GetFields())
            {
                TextField field = new TextField(i.Name + ":");
                if (i.GetValue(reaction) != null) field.value = i.GetValue(reaction).ToString();
                root.Add(field);
                
                if (i.FieldType == typeof(int))
                {
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