/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Dolittle.PropertyBags
{
    using System.Linq.Expressions;
    using System.Reflection;
    using Dolittle.Reflection;
    using Dolittle.Concepts;
    using System;
    using System.Linq;

    /// <summary>
    /// Defines a method for creating an instance using the provided constructor parameters
    /// </summary>
    /// <param name="args">Parameters to be used in the constructor</param>
    /// <returns>An instance of an object</returns>
    public delegate object InstanceActivator(params object[] args);   

    /// <summary>
    /// Helps create a delegate for instantiating an instance of a Type which requires 
    /// parameters in the constructor
    /// </summary>
    public static class Instantiator 
    {
        /// <summary>
        /// Creates a delegate instance that can be used to instantiate an instance with the 
        /// provided values
        /// </summary>
        /// <param name="ctor">The <see cref="ConstructorInfo" /></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static InstanceActivator GetInstanceActivator(ConstructorInfo ctor, IObjectFactory factory)
        {
            var type = ctor.DeclaringType;
            var paramsInfo = ctor.GetParameters();                  

            var param = Expression.Parameter(typeof(object[]), "args");
        
            var argsExp = new Expression[paramsInfo.Length];            
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                var index = Expression.Constant(i);
                var paramType = paramsInfo[i].ParameterType;              
                argsExp[i] = Expression.Convert(Expression.ArrayIndex(param, index), paramType);
            }                  

            var newExp = Expression.New(ctor,argsExp);                  
            var lambda = Expression.Lambda(typeof(InstanceActivator), newExp, param);              
            var compiled = (InstanceActivator)lambda.Compile();
            return compiled;
        }
    }
}