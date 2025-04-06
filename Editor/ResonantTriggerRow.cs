#if UNITY_EDITOR

using System;
using System.Linq;
using System.Reflection;
using Resonant.Runtime;
using UnityEngine.UIElements;

namespace Resonant.Editor
{
    public class ResonantTriggerRow : VisualElement
    {
        static readonly string DISALLOWED_CHARACTERS = ",\\/\n\t\r"; // Characters not allowed in the entry fields
        bool isEven;
        
        /// <summary>
        /// Returns a string, without the disallowed characters
        /// </summary>
        /// <param name="value">The string to be checked</param>
        /// <returns>A validated string, with no disallowed characters</returns>
        public static string ValidateInputValue(string value) => new(value.Where(c => !DISALLOWED_CHARACTERS.Contains(c)).ToArray());
        
        public ResonantTriggerRow(ResonantTrigger trigger, ResonantBehaviour behaviour, Action saveAction, Action editedAction)
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
                AddReaction(i, reactions, trigger, saveAction, editedAction);

            triggerID.RegisterValueChangedCallback(evt =>
            {
                var validated = ValidateInputValue(evt.newValue);
                triggerID.SetValueWithoutNotify(validated);

                trigger.ID = validated;
                
                editedAction?.Invoke();
            });

            addReaction.clicked += () =>
            {
                var reactionType = reactionTypes[reactionDropdown.index];
                var newReaction = (ResonantReaction)Activator.CreateInstance(reactionType);
                AddReaction(newReaction, reactions, trigger, saveAction, editedAction);
                
                trigger.Reactions.Add(newReaction);
                
                saveAction?.Invoke();
            };
        }

        void AddReaction(ResonantReaction reaction, VisualElement parent, ResonantTrigger trigger, Action saveAction, Action editedAction)
        {
            isEven = !isEven;

            var reactionRow = new ResonantReactionRow(reaction, trigger, isEven, saveAction, editedAction);
            parent.Add(reactionRow);
        }
    }
}

#endif