/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dolittle.Collections;
using Dolittle.Reflection;

namespace Dolittle.Applications
{
    /// <summary>
    /// Represents an implementation of <see cref="IApplicationStructureFragment"/>
    /// </summary>
    public class ApplicationStructureFragment : IApplicationStructureFragment
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationStructureFragment"/>
        /// </summary>
        /// <param name="type"><see cref="Type"/> of <see cref="IApplicationLocation"/> this fragment represents</param>
        /// <param name="required">Whether or not the fragment is required - default false</param>
        /// <param name="recursive">Whether or not the fragment can appear recursively - default false</param>
        public ApplicationStructureFragment(Type type, bool required = false, bool recursive = false) : this(type, NullApplicationStructureFragment.Instance, new IApplicationStructureFragment[0], required, recursive) { }

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationStructureFragment"/>
        /// </summary>
        /// <param name="type"><see cref="Type"/> of <see cref="IApplicationLocation"/> this fragment represents</param>
        /// <param name="children">Child <see cref="IApplicationStructureFragment">fragments</see></param>
        /// <param name="required">Whether or not the fragment is required - default false</param>
        /// <param name="recursive">Whether or not the fragment can appear recursively - default false</param>
        public ApplicationStructureFragment(
            Type type,
            IEnumerable<IApplicationStructureFragment> children,
            bool required = false,
            bool recursive = false) : this(type, NullApplicationStructureFragment.Instance, children, required, recursive) { }

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationStructureFragment"/>
        /// </summary>
        /// <param name="type"><see cref="Type"/> of <see cref="IApplicationLocation"/> this fragment represents</param>
        /// <param name="parent">The <see cref="IApplicationStructureFragment">parent fragment</see></param>
        /// <param name="required">Whether or not the fragment is required - default false</param>
        /// <param name="recursive">Whether or not the fragment can appear recursively - default false</param>
        public ApplicationStructureFragment(
            Type type,
            IApplicationStructureFragment parent,
            bool required = false,
            bool recursive = false) : this(type, parent, new IApplicationStructureFragment[0], required, recursive) { }

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationStructureFragment"/>
        /// </summary>
        /// <param name="type"><see cref="Type"/> of <see cref="IApplicationLocation"/> this fragment represents</param>
        /// <param name="parent">The <see cref="IApplicationStructureFragment">parent fragment</see></param>
        /// <param name="children">Child <see cref="IApplicationStructureFragment">fragments</see></param>
        /// <param name="required">Whether or not the fragment is required - default false</param>
        /// <param name="recursive">Whether or not the fragment can appear recursively - default false</param>
        public ApplicationStructureFragment(
            Type type,
            IApplicationStructureFragment parent,
            IEnumerable<IApplicationStructureFragment> children,
            bool required = false,
            bool recursive = false)
        {
            ThrowIfTypeIsNotApplicationLocation(type);
            ThrowIfTypeDoesNotHaveADefaultConstructorTakingName(type);

            if (parent != null && parent.GetType() != typeof(NullApplicationStructureFragment))
            {
                ThrowIfTypeDoesNotBelongToParent(type, parent.Type);
                ThrowIfParentCannotHoldType(parent.Type, type);
            }

            ThrowIfAnyChildDoesNotBelongToType(children, type);
            ThrowIfTypeCannotHoldAnyOfTheChildren(type, children);
            Type = type;
            Parent = parent;
            Required = required;
            Recursive = recursive;
            Children = children;
        }

        /// <inheritdoc/>
        public Type Type { get; }

        /// <inheritdoc/>
        public bool Required { get; }

        /// <inheritdoc/>
        public IApplicationStructureFragment Parent { get; }

        /// <inheritdoc/>
        public bool HasParent => Parent != null && Parent.GetType() != typeof(NullApplicationStructureFragment);

        /// <inheritdoc/>
        public IEnumerable<IApplicationStructureFragment> Children { get; }

        /// <inheritdoc/>
        public bool Recursive { get; }

        void ThrowIfTypeIsNotApplicationLocation(Type type)
        {
            if (!typeof(IApplicationLocationSegment).IsAssignableFrom(type)) throw new ApplicationStructureFragmentMustBeApplicationLocation(type);
        }

        void ThrowIfTypeDoesNotHaveADefaultConstructorTakingName(Type type)
        {
            var validParameterType = typeof(string);
            var valid = true;
            var constructor = type.GetConstructors().SingleOrDefault();
            if (constructor == null) valid = false;
            else
            {
                var parameter = constructor.GetParameters().SingleOrDefault();
                if (parameter == null) valid = false;
                else
                {
                    if (type.ImplementsOpenGeneric(typeof(IApplicationLocationSegment<>)))
                        validParameterType = type.GetInterfaces().Single(i => i.Name == typeof(IApplicationLocationSegment<>).Name).GenericTypeArguments[0];

                    if( parameter.ParameterType != validParameterType) valid = false;
                }
            }

            if (!valid) throw new ApplicationLocationSegmentMustHaveADefaultConstructorTakingName(type, validParameterType);
        }

        void ThrowIfParentCannotHoldType(Type parent, Type type)
        {
            var canHoldType = typeof(ICanHoldApplicationLocationSegmentsOfType<>).MakeGenericType(type);
            if (!canHoldType.IsAssignableFrom(parent)) throw new ParentApplicationStructureFragmentMustBeAbleToHoldType(parent, type);
        }

        void ThrowIfTypeDoesNotBelongToParent(Type type, Type parent)
        {
            var belongToType = typeof(IBelongToAnApplicationLocationSegmentTypeOf<>).MakeGenericType(parent);
            if (!belongToType.IsAssignableFrom(type)) throw new ApplicationStructureFragmentMustBelongToParent(parent, type);
        }

        void ThrowIfTypeCannotHoldAnyOfTheChildren(Type type, IEnumerable<IApplicationStructureFragment> children)
        {
            children.ForEach(fragment => ThrowIfParentCannotHoldType(type, fragment.Type));
        }

        void ThrowIfAnyChildDoesNotBelongToType(IEnumerable<IApplicationStructureFragment> children, Type type)
        {
            children.ForEach(fragment => ThrowIfTypeDoesNotBelongToParent(fragment.Type, type));
        }
    }
}