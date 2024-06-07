using Unity.Entities;
using Unity.Properties;
using UnityEngine;

namespace Baracuda.DOTS.Entities
{
    /// <summary>
    ///     Counts the number of Entity fields in a type. It recursively traverses nested types.
    /// </summary>
    internal class EntityFieldCountVisitor : IPropertyBagVisitor, IPropertyVisitor
    {
        /// <summary>
        ///     The number of Entity fields found during visitation.
        /// </summary>
        public int EntityFieldCount { get; private set; }

        void IPropertyBagVisitor.Visit<TContainer>(IPropertyBag<TContainer> propertyBag, ref TContainer container)
        {
            foreach (var property in propertyBag.GetProperties(ref container))
            {
                property.Accept(this, ref container);
            }
        }

        void IPropertyVisitor.Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container)
        {
            if (property is Property<TContainer, Entity> && !property.HasAttribute<HideInInspector>())
            {
                EntityFieldCount++;
            }
            else
            {
                var value = property.GetValue(ref container);
                PropertyContainer.TryAccept(this, ref value);
            }
        }
    }

    /// <summary>
    ///     Bakes the GameObject references and sets the value on the Entity fields of the traversed component.
    /// </summary>
    internal class ComponentDataPatcher : IPropertyBagVisitor, IPropertyVisitor
    {
        private readonly IBaker _Baker;
        private GameObject[] _References;
        private int _Index;

        public ComponentDataPatcher(IBaker baker, GameObject[] references = null)
        {
            _Baker = baker;
            Reset(references);
        }

        public void Reset(GameObject[] references)
        {
            _References = references;
            _Index = 0;
        }

        void IPropertyBagVisitor.Visit<TContainer>(IPropertyBag<TContainer> propertyBag, ref TContainer container)
        {
            foreach (var property in propertyBag.GetProperties(ref container))
            {
                property.Accept(this, ref container);
            }
        }

        void IPropertyVisitor.Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container)
        {
            var value = property.GetValue(ref container);
            if (property is Property<TContainer, Entity> entityProperty && !property.HasAttribute<HideInInspector>())
            {
                var reference = _References[_Index++];
                // TODO: Review if TransformUsageFlags.Dynamic is correct in this case
                var entity = _Baker.GetEntity(reference, TransformUsageFlags.Dynamic);

                entityProperty.SetValue(ref container, entity);
            }
            else
            {
                if (PropertyContainer.TryAccept(this, ref value))
                {
                    property.SetValue(ref container, value);
                }
            }
        }
    }
}