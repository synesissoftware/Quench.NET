// Created: 22nd July 2013
// Updated: 19th September 2026

using Quench.Exceptions;

using System.Xml;

namespace Quench.Configuration;

/// <summary>
///  Parser for Framework-era <c>&lt;quench&gt;</c> XML (0.1.1 schema).
/// </summary>
internal static class QuenchXmlParser
{
    private const string QuenchSectionName = "quench";
    private const string ProgramSectionName = "program";
    private const string DefaultQuenchActionAttributeName = "defaultQuenchAction";
    private const string ActionsSectionName = "actions";
    private const string ForExceptionSectionName = "forException";
    private const string OfClassAttributeName = "ofClass";
    private const string QuenchActionAttributeName = "quenchAction";
    private const string ExceptWhenSectionName = "exceptWhen";
    private const string InClassAttributeName = "inClass";

    internal static QuenchConfigurationSnapshot Parse(string xml)
    {
        XmlDocument document = new();
        document.LoadXml(xml);

        XmlElement? quenchElement = FindQuenchElement(document);
        if (quenchElement is null)
        {
            throw new UnrecognisedConfigurationSectionException(
                null,
                document.DocumentElement?.Name ?? string.Empty,
                $"Given section name '{document.DocumentElement?.Name}' is unexpected; only <{QuenchSectionName}> sections may be served");
        }

        return ParseQuenchElement(quenchElement);
    }

    private static XmlElement? FindQuenchElement(XmlDocument document)
    {
        if (document.DocumentElement is null)
        {
            return null;
        }

        if (document.DocumentElement.Name == QuenchSectionName)
        {
            return document.DocumentElement;
        }

        XmlNodeList? matches = document.GetElementsByTagName(QuenchSectionName);
        if (matches.Count == 1)
        {
            return matches[0] as XmlElement;
        }

        return null;
    }

    private static QuenchConfigurationSnapshot ParseQuenchElement(XmlElement section)
    {
        QuenchConfigurationBuilder builder = new();
        bool programSpecified = false;
        bool actionsSpecified = false;

        foreach (XmlNode subsectionNode in section.ChildNodes)
        {
            if (subsectionNode.NodeType != XmlNodeType.Element)
            {
                continue;
            }

            switch (subsectionNode.Name)
            {
                case ProgramSectionName:
                    if (programSpecified)
                    {
                        throw new InvalidQuenchConfigurationException(
                            section.Name,
                            $"Given subsection name '{subsectionNode.Name}' is already specified",
                            subsectionNode.Name);
                    }

                    builder.DefaultAction = PrepareProgram(subsectionNode);
                    programSpecified = true;
                    break;

                case ActionsSectionName:
                    if (actionsSpecified)
                    {
                        throw new InvalidQuenchConfigurationException(
                            section.Name,
                            $"Given subsection name '{subsectionNode.Name}' is already specified",
                            subsectionNode.Name);
                    }

                    PrepareActions(subsectionNode, builder);
                    actionsSpecified = true;
                    break;

                default:
                    throw new UnrecognisedConfigurationSectionException(
                        section.Name,
                        subsectionNode.Name,
                        $"Given subsection name '{subsectionNode.Name}' (of section '{section.Name}') is not recognised");
            }
        }

        if (!programSpecified)
        {
            throw new InvalidQuenchConfigurationException(
                section.Name,
                $"'{ProgramSectionName}' section was not specified");
        }

        return builder.Build();
    }

    private static QuenchAction PrepareProgram(XmlNode subsectionNode)
    {
        XmlAttribute? attr = subsectionNode.Attributes?[DefaultQuenchActionAttributeName];
        if (attr is null)
        {
            throw new MissingQuenchConfigurationValueException(
                ProgramSectionName,
                DefaultQuenchActionAttributeName);
        }

        return QuenchActionFromString(subsectionNode.Name, attr.Name, attr.Value);
    }

    private static void PrepareActions(
        XmlNode sectionNode,
        QuenchConfigurationBuilder builder)
    {
        foreach (XmlNode subsectionNode in sectionNode.ChildNodes)
        {
            if (subsectionNode.NodeType != XmlNodeType.Element)
            {
                continue;
            }

            switch (subsectionNode.Name)
            {
                case ForExceptionSectionName:
                    PrepareForException(subsectionNode, builder);
                    break;

                default:
                    throw new InvalidQuenchConfigurationException(
                        sectionNode.Name,
                        $"Unrecognised subsection name '{subsectionNode.Name}' in section '{sectionNode.Name}'",
                        subsectionNode.Name);
            }
        }
    }

    private static void PrepareForException(
        XmlNode sectionNode,
        QuenchConfigurationBuilder builder)
    {
        string? ofClass = null;
        QuenchAction? quenchAction = null;

        if (sectionNode.Attributes is not null)
        {
            foreach (XmlAttribute attribute in sectionNode.Attributes)
            {
                switch (attribute.Name)
                {
                    case OfClassAttributeName:
                        ofClass = attribute.Value;
                        break;

                    case QuenchActionAttributeName:
                        quenchAction = QuenchActionFromString(
                            sectionNode.Name,
                            attribute.Name,
                            attribute.Value);
                        break;

                    default:
                        throw new UnrecognisedConfigurationAttributeException(
                            sectionNode.Name,
                            attribute.Name);
                }
            }
        }

        if (ofClass is null)
        {
            throw new InvalidQuenchConfigurationException(
                sectionNode.Name,
                $"Missing '{OfClassAttributeName}' in section '{sectionNode.Name}'");
        }

        builder.AddNamedRule(ofClass, quenchAction);

        foreach (XmlNode subsectionNode in sectionNode.ChildNodes)
        {
            if (subsectionNode.NodeType != XmlNodeType.Element)
            {
                continue;
            }

            switch (subsectionNode.Name)
            {
                case ExceptWhenSectionName:
                    PrepareExceptWhen(subsectionNode, builder);
                    break;

                default:
                    throw new InvalidQuenchConfigurationException(
                        sectionNode.Name,
                        $"Unrecognised subsection name '{subsectionNode.Name}' in section '{sectionNode.Name}'",
                        subsectionNode.Name);
            }
        }
    }

    private static void PrepareExceptWhen(
        XmlNode sectionNode,
        QuenchConfigurationBuilder builder)
    {
        string? inClass = null;
        QuenchAction? quenchAction = null;

        if (sectionNode.Attributes is not null)
        {
            foreach (XmlAttribute attribute in sectionNode.Attributes)
            {
                switch (attribute.Name)
                {
                    case InClassAttributeName:
                        inClass = attribute.Value;
                        break;

                    case QuenchActionAttributeName:
                        quenchAction = QuenchActionFromString(
                            sectionNode.Name,
                            attribute.Name,
                            attribute.Value);
                        break;

                    default:
                        throw new UnrecognisedConfigurationAttributeException(
                            sectionNode.Name,
                            attribute.Name);
                }
            }
        }

        if (inClass is null)
        {
            throw new InvalidQuenchConfigurationException(
                sectionNode.Name,
                $"Missing '{InClassAttributeName}' in '{sectionNode.Name}'");
        }

        if (!quenchAction.HasValue)
        {
            throw new InvalidQuenchConfigurationException(
                sectionNode.Name,
                $"Missing '{QuenchActionAttributeName}' in '{sectionNode.Name}'");
        }

        builder.AddNamedExceptWhen(inClass, quenchAction.Value);
    }

    private static QuenchAction QuenchActionFromString(
        string subsectionName,
        string attributeName,
        string? value)
    {
        switch (value)
        {
            case "quench":
                return QuenchAction.Quench;
            case "throw":
                return QuenchAction.Throw;
            default:
                throw new InvalidQuenchConfigurationValueException(
                    subsectionName,
                    attributeName,
                    value ?? string.Empty);
        }
    }
}
