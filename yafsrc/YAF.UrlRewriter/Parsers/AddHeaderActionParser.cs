// UrlRewriter - A .NET URL Rewriter module
// Version 2.0
//
// Copyright 2011 Intelligencia
// Copyright 2011 Seth Yates
// 

namespace YAF.UrlRewriter.Parsers;

using System;
using System.Xml;

using YAF.UrlRewriter.Actions;
using YAF.UrlRewriter.Configuration;
using YAF.UrlRewriter.Extensions;
using YAF.UrlRewriter.Utilities;

/// <summary>
/// Action parser for the add-header action.
/// </summary>
public sealed class AddHeaderActionParser : RewriteActionParserBase
{
    /// <summary>
    /// The name of the action.
    /// </summary>
    public override string Name => Constants.ElementAdd;

    /// <summary>
    /// Whether the action allows nested actions.
    /// </summary>
    public override bool AllowsNestedActions => false;

    /// <summary>
    /// Whether the action allows attributes.
    /// </summary>
    public override bool AllowsAttributes => true;

    /// <summary>
    /// Parses the node.
    /// </summary>
    /// <param name="node">The node to parse.</param>
    /// <param name="config">The rewriter configuration.</param>
    /// <returns>The parsed action, or null if no action parsed.</returns>
    public override IRewriteAction Parse(XmlNode node, IRewriterConfiguration config)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        if (config == null)
        {
            throw new ArgumentNullException(nameof(config));
        }

        var headerName = node.GetOptionalAttribute(Constants.AttrHeader);
        if (headerName == null)
        {
            return null;
        }

        var headerValue = node.GetRequiredAttribute(Constants.AttrValue, true);

        return new AddHeaderAction(headerName, headerValue);
    }
}