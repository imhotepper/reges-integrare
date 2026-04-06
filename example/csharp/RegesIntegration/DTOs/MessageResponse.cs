using System.Xml.Serialization;

namespace RegesIntegration.DTOs;

/// <summary>
/// Synchronous response received immediately after sending a message
/// Contains a ResponseId (receipt) confirming the message was queued
/// </summary>
[XmlRoot("MessageResponse")]
public class MessageResponse
{
    [XmlElement("Header")]
    public Header? Header { get; set; }

    [XmlElement("ResponseId")]
    public string? ResponseId { get; set; }
}
