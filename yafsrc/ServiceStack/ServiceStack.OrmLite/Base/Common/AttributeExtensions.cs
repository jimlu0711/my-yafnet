﻿// ***********************************************************************
// <copyright file="AttributeExtensions.cs" company="ServiceStack, Inc.">
//     Copyright (c) ServiceStack, Inc. All Rights Reserved.
// </copyright>
// <summary>Fork for YetAnotherForum.NET, Licensed under the Apache License, Version 2.0</summary>
// ***********************************************************************

using ServiceStack.OrmLite.Base.Text;

namespace ServiceStack;

using System;
using System.Reflection;

/// <summary>
/// Class AttributeExtensions.
/// </summary>
public static class AttributeExtensions
{
    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>System.String.</returns>
    public static string GetDescription(this Type type)
    {
        var apiAttr = type.FirstAttribute<ApiAttribute>();
        if (apiAttr != null)
            return apiAttr.Description;

        var componentDescAttr = type.FirstAttribute<System.ComponentModel.DescriptionAttribute>();
        if (componentDescAttr != null)
            return componentDescAttr.Description;

        var ssDescAttr = type.FirstAttribute<DataAnnotations.DescriptionAttribute>();
        return ssDescAttr?.Description;
    }

    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <param name="mi">The mi.</param>
    /// <returns>System.String.</returns>
    public static string GetDescription(this MemberInfo mi)
    {
        var apiAttr = mi.FirstAttribute<ApiMemberAttribute>();
        if (apiAttr != null)
            return apiAttr.Description;

        var componentDescAttr = mi.FirstAttribute<System.ComponentModel.DescriptionAttribute>();
        if (componentDescAttr != null)
            return componentDescAttr.Description;

        var ssDescAttr = mi.FirstAttribute<DataAnnotations.DescriptionAttribute>();
        return ssDescAttr?.Description;
    }

    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <param name="pi">The pi.</param>
    /// <returns>System.String.</returns>
    public static string GetDescription(this ParameterInfo pi)
    {
        var componentDescAttr = pi.FirstAttribute<System.ComponentModel.DescriptionAttribute>();
        if (componentDescAttr != null)
            return componentDescAttr.Description;

        var ssDescAttr = pi.FirstAttribute<DataAnnotations.DescriptionAttribute>();
        return ssDescAttr?.Description;
    }
}