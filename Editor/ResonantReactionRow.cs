#if UNITY_EDITOR

using System;
using System.Linq;
using Resonant.Runtime;
using UnityEngine.UIElements;

namespace Resonant.Editor
{
    /// <summary>
    /// A row in a ResonantTrigger representing a ResonantReaction
    /// </summary>
    public class ResonantReactionRow : VisualElement
    {
        ScrollView attributes; // The scrolling view for all the attributes for this reaction
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="reaction">The reaction data to load</param>
        /// <param name="trigger">This reaction's ResonantTrigger</param>
        /// <param name="isEven">A toggle to check which background colour to assign to this reaction</param>
        /// <param name="saveAction">The callback to save the data</param>
        /// <param name="editedAction">The callback when the data is edited but not saved</param>
        public ResonantReactionRow(ResonantReaction reaction, ResonantTrigger trigger, bool isEven, Action saveAction, Action editedAction)
        {
            AddToClassList("flex_row");
            AddToClassList("reaction_" + (isEven ? "a" : "b"));

            // The button to delete this reaction
            Button remove = new();
            remove.text = "-";
            Add(remove);

            string typeName = reaction.GetType().ToString().Split('.')[^1];
            typeName = typeName.Substring(0, typeName.Length - 8);
            
            // This reaction's ResonantReaction type
            TextElement name = new();
            name.text = "<b>" + typeName + "</b>";
            name.AddToClassList("reaction_name");
            Add(name);

            remove.clicked += () =>
            {
                trigger.Reactions.Remove(reaction);
                ResonantEditorUtilities.RemoveElement(this);
                
                saveAction?.Invoke();
            };

            attributes = new();
            attributes.mode = ScrollViewMode.Horizontal;
            Add(attributes);

            // Populating the row with the fields and pre-saved values for this ResonantReaction type
            // (currently, only int, float, string, and bool values are accepted)
            foreach (var i in reaction.GetType().GetFields())
            {
                string fieldNameFormat = "<i>({0})</i> " + i.Name + ":";
                var fieldValue = i.GetValue(reaction);
                
                if (i.FieldType == typeof(int))
                {
                    var field = AddAttributeField(fieldValue);
                    field.label = string.Format(fieldNameFormat, "int");
                    
                    field.RegisterValueChangedCallback(evt =>
                    {
                        string value = ResonantTriggerRow.ValidateInputValue(evt.newValue);

                        try
                        {
                            if (string.IsNullOrEmpty(value))
                            {
                                value = "0";
                                field.SetValueWithoutNotify("");
                            }
                            else if (value == "-")
                            {
                                value = "0";
                                field.SetValueWithoutNotify("-");
                            }
                            else
                            {
                                int.Parse(value);
                                field.SetValueWithoutNotify(value);
                            }
                            
                            i.SetValue(reaction, int.Parse(string.IsNullOrEmpty(value) ? "0" : value));

                            editedAction?.Invoke();
                        }
                        catch
                        {
                            field.SetValueWithoutNotify(evt.previousValue);
                        }
                    });
                }
                else if (i.FieldType == typeof(float))
                {
                    var field = AddAttributeField(fieldValue);
                    field.label = string.Format(fieldNameFormat, "float");
                    
                    field.RegisterValueChangedCallback(evt =>
                    {
                        string value = ResonantTriggerRow.ValidateInputValue(evt.newValue);

                        try
                        {
                            if (string.IsNullOrEmpty(value))
                            {
                                value = "0";
                                field.SetValueWithoutNotify("");
                            }
                            else if (value == "-")
                            {
                                value = "0";
                                field.SetValueWithoutNotify("-");
                            }
                            else
                            {
                                float.Parse(value);
                                field.SetValueWithoutNotify(value);
                            }
                            
                            i.SetValue(reaction, float.Parse(string.IsNullOrEmpty(value) ? "0" : value));

                            editedAction?.Invoke();
                        }
                        catch
                        {
                            field.SetValueWithoutNotify(evt.previousValue);
                        }
                    });
                }
                else if (i.FieldType == typeof(string))
                {
                    // Skipping the ID property if this reaction is sourceless
                    if (i.Name == nameof(ResonantReaction.ID) &&
                        ResonantUtilities.HasAttribute<SourcelessAttribute>(reaction))
                        continue;
                    
                    var field = AddAttributeField(fieldValue);
                    field.label = string.Format(fieldNameFormat, "string");

                    field.RegisterValueChangedCallback(evt =>
                    {
                        string value = ResonantTriggerRow.ValidateInputValue(evt.newValue);
                        i.SetValue(reaction, value);
                
                        editedAction?.Invoke();
                    });
                }
                else if (i.FieldType == typeof(bool))
                {
                    // Skipping the WaitUntilEnd property if this reaction can't be skipped / is instantaneous
                    if (i.Name == nameof(ResonantReaction.WaitUntilEnd) &&
                        !ResonantUtilities.HasAttribute<SkippableAttribute>(reaction))
                        continue;

                    var field = new Toggle();
                    if (fieldValue != null) field.value = (bool)fieldValue;
                    field.label = string.Format(fieldNameFormat, "bool");
                    attributes.Add(field);

                    field.RegisterValueChangedCallback(evt =>
                    {
                        i.SetValue(reaction, evt.newValue);
                
                        editedAction?.Invoke();
                    });
                }
            }
        }

        /// <summary>
        /// Adds a new attribute field to this reaction's row
        /// </summary>
        /// <param name="value">The pre-saved value to set the field to</param>
        /// <returns>The field, once it has been created</returns>
        TextField AddAttributeField(object value)
        {
            TextField field = new();
            if (value != null) field.value = value.ToString();
            attributes.Add(field);

            return field;
        }
    }
}

#endif