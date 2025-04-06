#if UNITY_EDITOR

using System;
using Resonant.Runtime;
using UnityEngine.UIElements;

namespace Resonant.Editor
{
    public class ResonantReactionRow : VisualElement
    {
        public ResonantReactionRow(ResonantReaction reaction, ResonantTrigger trigger, bool isEven, Action saveAction, Action editedAction)
        {
            AddToClassList("flex_row");
            AddToClassList("reaction_" + (isEven ? "a" : "b"));

            Button remove = new();
            remove.text = "-";
            Add(remove);
            
            TextElement name = new();
            name.text = "<b>" + reaction.GetType().ToString().Split('.')[^1] + "</b>";
            name.AddToClassList("reaction_name");
            Add(name);

            remove.clicked += () =>
            {
                trigger.Reactions.Remove(reaction);
                ResonantEditorUtilities.RemoveElement(this);
                
                saveAction?.Invoke();
            };

            foreach (var i in reaction.GetType().GetFields())
            {
                string fieldNameFormat = "<i>({0})</i> " + i.Name + ":";
                TextField field = new();
                if (i.GetValue(reaction) != null) field.value = i.GetValue(reaction).ToString();
                Add(field);
                
                if (i.FieldType == typeof(int))
                {
                    field.label = string.Format(fieldNameFormat, "int");
                    
                    field.RegisterValueChangedCallback(evt =>
                    {
                        string value = ResonantTriggerRow.ValidateInputValue(evt.newValue);

                        try
                        {
                            int.Parse(value);

                            field.SetValueWithoutNotify(value);
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
                    field.label = string.Format(fieldNameFormat, "float");
                    
                    field.RegisterValueChangedCallback(evt =>
                    {
                        string value = ResonantTriggerRow.ValidateInputValue(evt.newValue);

                        try
                        {
                            float.Parse(value);

                            field.SetValueWithoutNotify(value);
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
                    field.label = string.Format(fieldNameFormat, "string");

                    field.RegisterValueChangedCallback(evt =>
                    {
                        string value = ResonantTriggerRow.ValidateInputValue(evt.newValue);
                        i.SetValue(reaction, value);
                
                        editedAction?.Invoke();
                    });
                }
            }
        }
    }
}

#endif