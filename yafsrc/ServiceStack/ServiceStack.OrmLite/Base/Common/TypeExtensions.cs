﻿// ***********************************************************************
// <copyright file="TypeExtensions.cs" company="ServiceStack, Inc.">
//     Copyright (c) ServiceStack, Inc. All Rights Reserved.
// </copyright>
// <summary>Fork for YetAnotherForum.NET, Licensed under the Apache License, Version 2.0</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

using ServiceStack.OrmLite.Base.Text;

namespace ServiceStack;

/// <summary>
/// Delegate ObjectActivator
/// </summary>
/// <param name="args">The arguments.</param>
/// <returns>System.Object.</returns>
public delegate object ObjectActivator(params object[] args);

/// <summary>
/// Delegate MethodInvoker
/// </summary>
/// <param name="instance">The instance.</param>
/// <param name="args">The arguments.</param>
/// <returns>System.Object.</returns>
public delegate object MethodInvoker(object instance, params object[] args);

/// <summary>
/// Delegate StaticMethodInvoker
/// </summary>
/// <param name="args">The arguments.</param>
/// <returns>System.Object.</returns>
public delegate object StaticMethodInvoker(params object[] args);

/// <summary>
/// Delegate ActionInvoker
/// </summary>
/// <param name="instance">The instance.</param>
/// <param name="args">The arguments.</param>
public delegate void ActionInvoker(object instance, params object[] args);

/// <summary>
/// Delegate StaticActionInvoker
/// </summary>
/// <param name="args">The arguments.</param>
public delegate void StaticActionInvoker(params object[] args);

/// <summary>
/// Delegate to return a different value from an instance (e.g. member accessor)
/// </summary>
/// <param name="instance">The instance.</param>
/// <returns>System.Object.</returns>
public delegate object InstanceMapper(object instance);


/// <summary>
/// Class TypeExtensions.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Gets the referenced types.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>Type[].</returns>
    public static Type[] GetReferencedTypes(this Type type)
    {
        var refTypes = new HashSet<Type> { type };

        AddReferencedTypes(type, refTypes);

        return refTypes.ToArray();
    }

    /// <summary>
    /// Adds the referenced types.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="refTypes">The reference types.</param>
    public static void AddReferencedTypes(Type type, HashSet<Type> refTypes)
    {
        if (type.BaseType != null)
        {
            if (!refTypes.Contains(type.BaseType))
            {
                refTypes.Add(type.BaseType);
                AddReferencedTypes(type.BaseType, refTypes);
            }

            if (!type.BaseType.GetGenericArguments().IsEmpty())
            {
                foreach (var arg in type.BaseType.GetGenericArguments())
                {
                    if (!refTypes.Contains(arg))
                    {
                        refTypes.Add(arg);
                        AddReferencedTypes(arg, refTypes);
                    }
                }
            }
        }

        foreach (var iface in type.GetInterfaces())
        {
            if (iface.IsGenericType && !iface.IsGenericTypeDefinition)
            {
                foreach (var arg in iface.GetGenericArguments())
                {
                    if (!refTypes.Contains(arg))
                    {
                        refTypes.Add(arg);
                        AddReferencedTypes(arg, refTypes);
                    }
                }
            }
        }

        var properties = type.GetProperties();
        if (!properties.IsEmpty())
        {
            foreach (var p in properties)
            {
                if (!refTypes.Contains(p.PropertyType))
                {
                    refTypes.Add(p.PropertyType);
                    AddReferencedTypes(type, refTypes);
                }

                var args = p.PropertyType.GetGenericArguments();
                if (!args.IsEmpty())
                {
                    foreach (var arg in args)
                    {
                        if (!refTypes.Contains(arg))
                        {
                            refTypes.Add(arg);
                            AddReferencedTypes(arg, refTypes);
                        }
                    }
                }
                else if (p.PropertyType.IsArray)
                {
                    var elType = p.PropertyType.GetElementType();
                    if (!refTypes.Contains(elType))
                    {
                        refTypes.Add(elType);
                        AddReferencedTypes(elType, refTypes);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets the activator to cache.
    /// </summary>
    /// <param name="ctor">The ctor.</param>
    /// <returns>ObjectActivator.</returns>
    public static ObjectActivator GetActivatorToCache(ConstructorInfo ctor)
    {
        var pi = ctor.GetParameters();
        var paramArgs = Expression.Parameter(typeof(object[]), "args");
        var exprArgs = new Expression[pi.Length];

        var convertFromMethod = typeof(TypeExtensions).GetStaticMethod(nameof(ConvertFromObject));

        for (int i = 0; i < pi.Length; i++)
        {
            var index = Expression.Constant(i);
            var paramType = pi[i].ParameterType;
            var paramAccessorExp = Expression.ArrayIndex(paramArgs, index);
            var paramCastExp = Expression.Convert(paramAccessorExp, paramType);
            var convertParam = convertFromMethod.MakeGenericMethod(paramType);
            exprArgs[i] = Expression.Call(convertParam, paramAccessorExp);
        }

        var newExp = Expression.New(ctor, exprArgs);
        var lambda = Expression.Lambda(typeof(ObjectActivator),
            Expression.Convert(newExp, typeof(object)),
            paramArgs);

        var ctorFn = (ObjectActivator)lambda.Compile();
        return ctorFn;
    }

    /// <summary>
    /// The activator cache
    /// </summary>
    static Dictionary<ConstructorInfo, ObjectActivator> activatorCache =
        new();

    /// <summary>
    /// Gets the activator.
    /// </summary>
    /// <param name="ctor">The ctor.</param>
    /// <returns>ObjectActivator.</returns>
    public static ObjectActivator GetActivator(this ConstructorInfo ctor)
    {
        if (activatorCache.TryGetValue(ctor, out var fn))
            return fn;

        fn = GetActivatorToCache(ctor);

        Dictionary<ConstructorInfo, ObjectActivator> snapshot, newCache;
        do
        {
            snapshot = activatorCache;
            newCache = new Dictionary<ConstructorInfo, ObjectActivator>(activatorCache) { [ctor] = fn };

        } while (!ReferenceEquals(
                     Interlocked.CompareExchange(ref activatorCache, newCache, snapshot), snapshot));

        return fn;
    }

    /// <summary>
    /// Creates the invoker parameter expressions.
    /// </summary>
    /// <param name="method">The method.</param>
    /// <param name="paramArgs">The parameter arguments.</param>
    /// <returns>Expression[].</returns>
    private static Expression[] CreateInvokerParamExpressions(MethodInfo method, ParameterExpression paramArgs)
    {
        var convertFromMethod = typeof(TypeExtensions).GetStaticMethod(nameof(ConvertFromObject));

        var pi = method.GetParameters();
        var exprArgs = new Expression[pi.Length];
        for (int i = 0; i < pi.Length; i++)
        {
            var index = Expression.Constant(i);
            var paramType = pi[i].ParameterType;
            var paramAccessorExp = Expression.ArrayIndex(paramArgs, index);
            var convertParam = convertFromMethod.MakeGenericMethod(paramType);
            exprArgs[i] = Expression.Call(convertParam, paramAccessorExp);
        }

        return exprArgs;
    }

    /// <summary>
    /// Uses the correct invoker error message.
    /// </summary>
    /// <param name="method">The method.</param>
    /// <returns>System.String.</returns>
    private static string UseCorrectInvokerErrorMessage(MethodInfo method)
    {
        var invokerName = method.ReturnType == typeof(void)
                              ? method.IsStatic ? nameof(GetStaticActionInvoker) : nameof(GetActionInvoker)
                              : method.IsStatic ? nameof(GetStaticInvoker) : nameof(GetInvoker);
        var invokerType = method.ReturnType == typeof(void)
                              ? method.IsStatic ? nameof(StaticMethodInvoker) : nameof(MethodInvoker)
                              : method.IsStatic ? nameof(StaticActionInvoker) : nameof(ActionInvoker);
        var methodType = method.ReturnType == typeof(void)
                             ? method.IsStatic ? "static void methods" : "instance void methods"
                             : method.IsStatic ? "static methods" : "instance methods";
        return $"Use {invokerName} to create a {invokerType} for invoking {methodType}";
    }

    /// <summary>
    /// Gets the invoker to cache.
    /// </summary>
    /// <param name="method">The method.</param>
    /// <returns>MethodInvoker.</returns>
    /// <exception cref="System.NotSupportedException"></exception>
    public static MethodInvoker GetInvokerToCache(MethodInfo method)
    {
        if (method.IsStatic)
            throw new NotSupportedException(UseCorrectInvokerErrorMessage(method));

        var paramInstance = Expression.Parameter(typeof(object), "instance");
        var paramArgs = Expression.Parameter(typeof(object[]), "args");

        var exprArgs = CreateInvokerParamExpressions(method, paramArgs);

        var methodCall = method.DeclaringType.IsValueType
                             ? Expression.Call(Expression.Convert(paramInstance, method.DeclaringType), method, exprArgs)
                             : Expression.Call(Expression.TypeAs(paramInstance, method.DeclaringType), method, exprArgs);

        var convertToMethod = typeof(TypeExtensions).GetStaticMethod(nameof(ConvertToObject));
        var convertReturn = convertToMethod.MakeGenericMethod(method.ReturnType);

        var lambda = Expression.Lambda(typeof(MethodInvoker),
            Expression.Call(convertReturn, methodCall),
            paramInstance,
            paramArgs);

        var fn = (MethodInvoker)lambda.Compile();
        return fn;
    }

    /// <summary>
    /// Gets the static invoker to cache.
    /// </summary>
    /// <param name="method">The method.</param>
    /// <returns>StaticMethodInvoker.</returns>
    /// <exception cref="System.NotSupportedException"></exception>
    public static StaticMethodInvoker GetStaticInvokerToCache(MethodInfo method)
    {
        if (!method.IsStatic || method.ReturnType == typeof(void))
            throw new NotSupportedException(UseCorrectInvokerErrorMessage(method));

        var paramArgs = Expression.Parameter(typeof(object[]), "args");

        var exprArgs = CreateInvokerParamExpressions(method, paramArgs);

        var methodCall = Expression.Call(method, exprArgs);

        var convertToMethod = typeof(TypeExtensions).GetStaticMethod(nameof(ConvertToObject));
        var convertReturn = convertToMethod.MakeGenericMethod(method.ReturnType);

        var lambda = Expression.Lambda(typeof(StaticMethodInvoker),
            Expression.Call(convertReturn, methodCall),
            paramArgs);

        var fn = (StaticMethodInvoker)lambda.Compile();
        return fn;
    }

    /// <summary>
    /// Gets the action invoker to cache.
    /// </summary>
    /// <param name="method">The method.</param>
    /// <returns>ActionInvoker.</returns>
    /// <exception cref="System.NotSupportedException"></exception>
    public static ActionInvoker GetActionInvokerToCache(MethodInfo method)
    {
        if (method.IsStatic || method.ReturnType != typeof(void))
            throw new NotSupportedException(UseCorrectInvokerErrorMessage(method));

        var paramInstance = Expression.Parameter(typeof(object), "instance");
        var paramArgs = Expression.Parameter(typeof(object[]), "args");

        var exprArgs = CreateInvokerParamExpressions(method, paramArgs);

        var methodCall = method.DeclaringType.IsValueType
                             ? Expression.Call(Expression.Convert(paramInstance, method.DeclaringType), method, exprArgs)
                             : Expression.Call(Expression.TypeAs(paramInstance, method.DeclaringType), method, exprArgs);

        var lambda = Expression.Lambda(typeof(ActionInvoker),
            methodCall,
            paramInstance,
            paramArgs);

        var fn = (ActionInvoker)lambda.Compile();
        return fn;
    }

    /// <summary>
    /// Gets the static action invoker to cache.
    /// </summary>
    /// <param name="method">The method.</param>
    /// <returns>StaticActionInvoker.</returns>
    /// <exception cref="System.NotSupportedException"></exception>
    public static StaticActionInvoker GetStaticActionInvokerToCache(MethodInfo method)
    {
        if (!method.IsStatic || method.ReturnType != typeof(void))
            throw new NotSupportedException(UseCorrectInvokerErrorMessage(method));

        var paramArgs = Expression.Parameter(typeof(object[]), "args");

        var exprArgs = CreateInvokerParamExpressions(method, paramArgs);

        var methodCall = Expression.Call(method, exprArgs);

        var lambda = Expression.Lambda(typeof(StaticActionInvoker),
            methodCall,
            paramArgs);

        var fn = (StaticActionInvoker)lambda.Compile();
        return fn;
    }

    /// <summary>
    /// The invoker cache
    /// </summary>
    static Dictionary<MethodInfo, MethodInvoker> invokerCache = new();

    /// <summary>
    /// Create an Invoker for public instance methods
    /// </summary>
    /// <param name="method">The method.</param>
    /// <returns>MethodInvoker.</returns>
    public static MethodInvoker GetInvoker(this MethodInfo method)
    {
        if (invokerCache.TryGetValue(method, out var fn))
            return fn;

        fn = GetInvokerToCache(method);

        Dictionary<MethodInfo, MethodInvoker> snapshot, newCache;
        do
        {
            snapshot = invokerCache;
            newCache = new Dictionary<MethodInfo, MethodInvoker>(invokerCache) { [method] = fn };

        } while (!ReferenceEquals(
                     Interlocked.CompareExchange(ref invokerCache, newCache, snapshot), snapshot));

        return fn;
    }

    /// <summary>
    /// The static invoker cache
    /// </summary>
    static Dictionary<MethodInfo, StaticMethodInvoker> staticInvokerCache = new();

    /// <summary>
    /// Create an Invoker for public static methods
    /// </summary>
    /// <param name="method">The method.</param>
    /// <returns>StaticMethodInvoker.</returns>
    public static StaticMethodInvoker GetStaticInvoker(this MethodInfo method)
    {
        if (staticInvokerCache.TryGetValue(method, out var fn))
            return fn;

        fn = GetStaticInvokerToCache(method);

        Dictionary<MethodInfo, StaticMethodInvoker> snapshot, newCache;
        do
        {
            snapshot = staticInvokerCache;
            newCache = new Dictionary<MethodInfo, StaticMethodInvoker>(staticInvokerCache) { [method] = fn };

        } while (!ReferenceEquals(
                     Interlocked.CompareExchange(ref staticInvokerCache, newCache, snapshot), snapshot));

        return fn;
    }

    /// <summary>
    /// The action invoker cache
    /// </summary>
    static Dictionary<MethodInfo, ActionInvoker> actionInvokerCache = new();

    /// <summary>
    /// Create an Invoker for public instance void methods
    /// </summary>
    /// <param name="method">The method.</param>
    /// <returns>ActionInvoker.</returns>
    public static ActionInvoker GetActionInvoker(this MethodInfo method)
    {
        if (actionInvokerCache.TryGetValue(method, out var fn))
            return fn;

        fn = GetActionInvokerToCache(method);

        Dictionary<MethodInfo, ActionInvoker> snapshot, newCache;
        do
        {
            snapshot = actionInvokerCache;
            newCache = new Dictionary<MethodInfo, ActionInvoker>(actionInvokerCache) { [method] = fn };

        } while (!ReferenceEquals(
                     Interlocked.CompareExchange(ref actionInvokerCache, newCache, snapshot), snapshot));

        return fn;
    }

    /// <summary>
    /// The static action invoker cache
    /// </summary>
    static Dictionary<MethodInfo, StaticActionInvoker> staticActionInvokerCache = new();

    /// <summary>
    /// Create an Invoker for public static void methods
    /// </summary>
    /// <param name="method">The method.</param>
    /// <returns>StaticActionInvoker.</returns>
    public static StaticActionInvoker GetStaticActionInvoker(this MethodInfo method)
    {
        if (staticActionInvokerCache.TryGetValue(method, out var fn))
            return fn;

        fn = GetStaticActionInvokerToCache(method);

        Dictionary<MethodInfo, StaticActionInvoker> snapshot, newCache;
        do
        {
            snapshot = staticActionInvokerCache;
            newCache = new Dictionary<MethodInfo, StaticActionInvoker>(staticActionInvokerCache) { [method] = fn };

        } while (!ReferenceEquals(
                     Interlocked.CompareExchange(ref staticActionInvokerCache, newCache, snapshot), snapshot));

        return fn;
    }

    /// <summary>
    /// Converts from object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <returns>T.</returns>
    public static T ConvertFromObject<T>(object value)
    {
        if (value == null)
            return default;

        if (value is T variable)
            return variable;

        if (typeof(T) == typeof(string) && value is IRawString rs)
            return (T)(object)rs.ToRawString();

        return value.ConvertTo<T>();
    }

    /// <summary>
    /// Converts to object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <returns>System.Object.</returns>
    public static object ConvertToObject<T>(T value)
    {
        return value;
    }

    /// <summary>
    /// Check if #nullable enabled reference type is non nullable
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>true if #nullable enabled reference type, false if optional, null if value Type or #nullable not enabled</returns>
    public static bool? IsNotNullable(this PropertyInfo property) =>
        IsNotNullable(property.PropertyType, property.DeclaringType, property.CustomAttributes);

    /// <summary>
    /// Check if #nullable enabled reference type is non nullable
    /// </summary>
    /// <param name="memberType">Type of the member.</param>
    /// <param name="declaringType">Type of the declaring.</param>
    /// <param name="customAttributes">The custom attributes.</param>
    /// <returns>true if #nullable enabled reference type, false if optional, null if value Type or #nullable not enabled</returns>
    public static bool? IsNotNullable(Type memberType, MemberInfo declaringType, IEnumerable<CustomAttributeData> customAttributes)
    {
        if (!memberType.IsValueType)
        {
            var nullable = customAttributes
                .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");
            if (nullable != null && nullable.ConstructorArguments.Count == 1)
            {
                var attributeArgument = nullable.ConstructorArguments[0];
                if (attributeArgument.ArgumentType == typeof(byte[]))
                {
                    var args = (ReadOnlyCollection<CustomAttributeTypedArgument>)attributeArgument.Value;
                    if (args.Count > 0 && args[0].ArgumentType == typeof(byte))
                    {
                        return (byte)args[0].Value == 1;
                    }
                }
                else if (attributeArgument.ArgumentType == typeof(byte))
                {
                    return (byte)attributeArgument.Value == 1;
                }
            }

            for (var type = declaringType; type != null; type = type.DeclaringType)
            {
                var context = type.CustomAttributes
                    .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableContextAttribute");
                if (context != null &&
                    context.ConstructorArguments.Count == 1 &&
                    context.ConstructorArguments[0].ArgumentType == typeof(byte))
                {
                    return (byte)context.ConstructorArguments[0].Value == 1;
                }
            }
        }
        return null;
    }
}