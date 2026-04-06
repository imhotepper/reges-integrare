using System.Xml.Serialization;

namespace RegesIntegration.DTOs;

/// <summary>
/// Result of message processing
/// </summary>
[XmlRoot("Result")]
public class Result
{
    [XmlElement("Code")]
    public string? Code { get; set; }

    [XmlElement("Description")]
    public string? Description { get; set; }

    [XmlElement("Ref")]
    public string? Ref { get; set; }
}
