#if UNITY_EDITOR

using System;
using System.Linq;
using System.Reflection;
using Resonant.Runtime;
using UnityEngine.UIElements;

namespace Resonant.Editor
{
    /// <summary>
    /// A row in the ResonantEditor representing a ResonantTrigger and its ResonantReactions
    /// </summary>
    public class ResonantTriggerRow : VisualElement
    {
        static readonly string DISALLOWED_CHARACTERS = ",\\/\n\t\r"; // Characters not allowed in the entry fields
        bool isEven; // A toggle to check which background colour to assign to the next added reaction
        
        /// <summary>
        /// Returns a string, without the disallowed characters
        /// </summary>
        /// <param name="value">The string to be checked</param>
        /// <returns>A validated string, with no disallowed characters</returns>
        public static string ValidateInputValue(string value) => new(value.Where(c => !DISALLOWED_CHARACTERS.Contains(c)).ToArray());
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="trigger">The trigger data to load</param>
        /// <param name="behaviour">This trigger's ResonantBehaviour</param>
        /// <param name="saveAction">The callback to save the data</param>
        /// <param name="editedAction">The callback when the data is edited but not saved</param>
        public ResonantTriggerRow(ResonantTrigger trigger, ResonantBehaviour behaviour, Action saveAction, Action editedAction)
        {
            AddToClassList("flex_row");

            // The left-hand section of the trigger
            VisualElement triggerRow = new();
            triggerRow.AddToClassList("flex_row");
            triggerRow.AddToClassList("trigger");
            Add(triggerRow);
            
            // The button to delete the trigger from the behaviour
            Button remove = new();
            remove.text = "-";
            triggerRow.Add(remove);

            TextElement when = new();
            when.text = "When";
            triggerRow.Add(when);

            // The ID of this trigger, used to trigger its reactions
            TextField triggerID = new();
            triggerID.value = trigger.ID;
            triggerRow.Add(triggerID);

            TextElement arrow = new();
            arrow.text = "->";
            triggerRow.Add(arrow);

            // The right-hand section of the trigger
            VisualElement reactions = new();
            reactions.AddToClassList("flex_column");
            reactions.AddToClassList("reactions");
            Add(reactions);

            VisualElement addReactionRow = new();
            addReactionRow.AddToClassList("flex_row");
            addReactionRow.AddToClassList("reaction_add");
            reactions.Add(addReactionRow);
            
            // The button to add new reactions
            Button addReaction = new();
            addReaction.text = "Add reaction";
            addReactionRow.Add(addReaction);

            // Getting a formatted list of all the inherited ResonantReaction classes
            var reactionTypes = Assembly.GetAssembly(typeof(ResonantReaction)).GetTypes()
                .Where(type => type.IsSubclassOf(typeof(ResonantReaction)))
                .ToList();
            
            // The dropdown to select the type of reaction to add
            DropdownField reactionDropdown = new DropdownField(reactionTypes
                .Select(type => type.ToString().Split('.')[^1])
                .Select(type => type.Substring(0, type.Length - 8))
                .ToList(), 0);
            addReactionRow.Add(reactionDropdown);
            
            remove.clicked += () =>
            {
                behaviour.Triggers.Remove(trigger);
                ResonantEditorUtilities.RemoveElement(this);
                
                saveAction?.Invoke();
            };
            
            // Loading in all the pre-saved reactions
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

        /// <summary>
        /// Adds a new row to this trigger that represents a ResonantReaction
        /// </summary>
        /// <param name="reaction">The reaction data to load</param>
        /// <param name="parent">The parent to add the reaction row to</param>
        /// <param name="trigger">This ResonantTrigger</param>
        /// <param name="saveAction">The callback to save the data</param>
        /// <param name="editedAction">The callback when the data is edited but not saved</param>
        void AddReaction(ResonantReaction reaction, VisualElement parent, ResonantTrigger trigger, Action saveAction, Action editedAction)
        {
            isEven = !isEven;

            var reactionRow = new ResonantReactionRow(reaction, trigger, isEven, saveAction, editedAction);
            parent.Add(reactionRow);
        }
    }
}

#endif