using Baracuda.DOTS.Entities;
using System;
using Unity.Entities;
using Unity.Properties;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Baracuda.DOTS.Editor
{
    [UnityEditor.CustomPropertyDrawer(typeof(AutoAuthoringData))]
    internal sealed class AutoAuthoringPropertyDrawer : UnityEditor.PropertyDrawer
    {
        private const string ArrayPathSubstring = "InfoArray.ComponentArray.Array.data[0]";

        public override VisualElement CreatePropertyGUI(UnityEditor.SerializedProperty property)
        {
            var targetComponent = property.serializedObject.targetObject as AutoAuthoringBase;
            if (targetComponent == null)
            {
                return null;
            }

            var root = new VisualElement();

            if (!targetComponent.IsBufferComponent)
            {
                var authoringDataProperty = property.FindPropertyRelative("ComponentArray.Array.data[0].AuthoringData");
                var referencesProperty = property.FindPropertyRelative("ComponentArray.Array.data[0].References");

                if (authoringDataProperty == null)
                {
                    root.Add(new HelpBox($"Authoring data type {targetComponent.GetComponentType().Name} is not serializable", HelpBoxMessageType.Error));
                    return root;
                }

                if (typeof(ISharedComponentData).IsAssignableFrom(targetComponent.GetComponentType()) && referencesProperty.arraySize != 0)
                {
                    root.Add(new HelpBox("Shared Components do not support patching the entity references. " +
                                         "The baked entity references will not be valid at runtime."
                        , HelpBoxMessageType.Warning));
                }

                var visitor = new PropertyFieldBuilderVisitor(root, authoringDataProperty, referencesProperty);
                PropertyContainer.Accept(visitor, authoringDataProperty.boxedValue);
            }
            else
            {
                var visitor = new PropertyFieldBuilderVisitor();

                var listView = new ListView();
                listView.makeItem = () =>
                {
                    var authoringDataProperty = property.FindPropertyRelative("ComponentArray.Array.data[0].AuthoringData");
                    var referencesProperty = property.FindPropertyRelative("ComponentArray.Array.data[0].References");

                    var template = new VisualElement();
                    visitor.Reset(template, authoringDataProperty, referencesProperty);
                    PropertyContainer.Accept(visitor, Activator.CreateInstance(targetComponent.GetComponentType()));
                    return template;
                };
                listView.bindItem = (element, i) =>
                {
                    var pathPrefix = $"InfoArray.ComponentArray.Array.data[{i}]";

                    RebindChildren(element, i, pathPrefix, property.serializedObject);
                    element.Bind(property.serializedObject);
                };

                listView.showAddRemoveFooter = true;
                listView.showAlternatingRowBackgrounds = AlternatingRowBackground.All;
                listView.bindingPath = "ComponentArray";
                listView.Bind(property.serializedObject);
                listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
                listView.showBoundCollectionSize = false;
                listView.showBorder = true;

                root.Add(listView);
            }

            return root;
        }

        private static void RebindChildren(VisualElement element, int i, string pathPrefix, UnityEditor.SerializedObject serializedObject)
        {
            foreach (var c in element.Children())
            {
                if (c is IBindable {bindingPath: not null} bindable)
                {
                    var oldPp = bindable.bindingPath;
                    var pp = pathPrefix + oldPp[ArrayPathSubstring.Length..];
                    var p = serializedObject.FindProperty(pp);
                    bindable.BindProperty(p);
                }
                RebindChildren(c, i, pathPrefix, serializedObject);
            }
        }
    }
}