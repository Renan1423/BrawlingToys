using UnityEngine;
using UnityEditor;

namespace BrawlingToys.Core
{
    public class TagSelectorAttribute : PropertyAttribute { }

    [CustomPropertyDrawer(typeof(TagSelectorAttribute))]
    public class TagSelectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string[] tags = CreateTagsStrings();

            if (property.propertyType == SerializedPropertyType.String)
            {
                int index = Mathf.Max(0, System.Array.IndexOf(tags, property.stringValue));
                index = EditorGUI.Popup(position, label.text, index, tags);

                property.stringValue = tags[index];
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use [TagSelector] with strings.");
            }
        }

        private string[] CreateTagsStrings() 
        {
            string[] tagsArray = {
                TagManager.PlayerUiController.CLICK
            };

            return tagsArray;
        }
    }
}
