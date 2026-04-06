using System.Xml.Serialization;

namespace RegesIntegration.DTOs;

/// <summary>
/// Message header containing metadata about the message
/// </summary>
[XmlRoot("Header")]
public class Header
{
    [XmlElement("MessageId")]
    public string? MessageId { get; set; }

    [XmlElement("ClientApplication")]
    public string? ClientApplication { get; set; }

    [XmlElement("Version")]
    public string? Version { get; set; }

    [XmlElement("Operation")]
    public string? Operation { get; set; }

    [XmlElement("AuthorId")]
    public string? AuthorId { get; set; }

    [XmlElement("SessionId")]
    public string? SessionId { get; set; }

    [XmlElement("User")]
    public string? User { get; set; }

    [XmlElement("UserId")]
    public string? UserId { get; set; }

    [XmlElement("Timestamp")]
    public DateTime? Timestamp { get; set; }
}
