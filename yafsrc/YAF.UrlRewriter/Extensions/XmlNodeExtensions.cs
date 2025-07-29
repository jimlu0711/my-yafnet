﻿// UrlRewriter - A .NET URL Rewriter module
// Version 2.0
//
// Copyright 2011 Intelligencia
// Copyright 2011 Seth Yates
// 

namespace YAF.UrlRewriter.Extensions;

using System;
using System.Configuration;
using System.Xml;

using YAF.UrlRewriter.Utilities;

/// <summary>
/// Extension methods for the XmlNode class.
/// </summary>
static internal class XmlNodeExtensions
{
    /// <summary>
    /// Gets a required attribute from an XML node.
    /// Throws an error if the required attribute is missing or empty (blank).
    /// </summary>
    /// <param name="node">The XML node</param>
    /// <param name="attributeName">The XML attribute name</param>
    /// <returns>The attribute value</returns>
    public static string GetRequiredAttribute(this XmlNode node, string attributeName)
    {
        return node.GetRequiredAttribute(attributeName, false);
    }

    /// <summary>
    /// Gets a required attribute from an XML node.
    /// Throws an error if the required attribute is missing.
    /// Throws an error if the required attribute is empty (blank) and allowBlank is set to false.
    /// </summary>
    /// <param name="node">The XML node</param>
    /// <param name="attributeName">The XML attribute name</param>
    /// <param name="allowBlank">Blank (empty) values okay?</param>
    /// <returns>The attribute value</returns>
    public static string GetRequiredAttribute(this XmlNode node, string attributeName, bool allowBlank)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        var attribute = node.Attributes.GetNamedItem(attributeName);
        if (attribute == null)
        {
            throw new ConfigurationErrorsException(MessageProvider.FormatString(Message.AttributeRequired, attributeName), node);
        }

        if (attribute.Value.Length == 0 && !allowBlank)
        {
            throw new ConfigurationErrorsException(MessageProvider.FormatString(Message.AttributeCannotBeBlank, attributeName), node);
        }

        return attribute.Value;
    }


    /// <summary>
    /// Gets an optional attribute from an XML node.
    /// Returns null if the attribute is missing.
    /// </summary>
    /// <param name="node">The XML node</param>
    /// <param name="attributeName">The XML attribute name</param>
    /// <returns>The attribute value</returns>
    public static string GetOptionalAttribute(this XmlNode node, string attributeName)
    {
        return node.GetOptionalAttribute(attributeName, null);
    }

    /// <summary>
    /// Gets an optional attribute from an XML node.
    /// Returns null if the attribute is missing.
    /// </summary>
    /// <param name="node">The XML node</param>
    /// <param name="attributeName">The XML attribute name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The attribute value</returns>
    public static string GetOptionalAttribute(this XmlNode node, string attributeName, string defaultValue)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        var attribute = node.Attributes.GetNamedItem(attributeName);

        return attribute == null ? defaultValue : attribute.Value;
    }

    /// <summary>
    /// Gets (parses) a boolean attribute from an XML node.
    /// Throws an exception if the value in the attribute is an invalid boolean value.
    /// </summary>
    /// <param name="node">The XML node</param>
    /// <param name="attributeName">The XML attribute name</param>
    /// <returns>The boolean value, or null if the value was missing</returns>
    public static bool? GetBooleanAttribute(this XmlNode node, string attributeName)
    {
        var attributeValue = node.GetOptionalAttribute(attributeName);
        if (attributeValue == null)
        {
            return null;
        }

        if (!bool.TryParse(attributeValue, out var returnValue))
        {
            throw new ConfigurationErrorsException(MessageProvider.FormatString(Message.InvalidBooleanAttribute, attributeName), node);
        }

        return returnValue;
    }

    /// <summary>
    /// Gets (parses) an integer attribute from an XML node.
    /// Throws an exception if the value in the attribute not an integer.
    /// </summary>
    /// <param name="node">The XML node</param>
    /// <param name="attributeName">The XML attribute name</param>
    /// <returns>The integer value, or null if the value was missing</returns>
    public static int? GetIntegerAttribute(this XmlNode node, string attributeName)
    {
        var attributeValue = node.GetOptionalAttribute(attributeName);
        if (attributeValue == null)
        {
            return null;
        }

        if (!int.TryParse(attributeValue, out var returnValue))
        {
            throw new ConfigurationErrorsException(MessageProvider.FormatString(Message.InvalidIntegerAttribute, attributeName), node);
        }

        return returnValue;
    }
}