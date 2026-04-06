using System.Xml.Serialization;

namespace RegesIntegration.DTOs;

/// <summary>
/// Asynchronous response received after message processing
/// Contains the result of the operation (success/fail) and details
/// </summary>
[XmlRoot("MessageResult")]
public class MessageResult
{
    [XmlElement("Header")]
    public Header? Header { get; set; }

    [XmlElement("ResponseId")]
    public string? ResponseId { get; set; }

    [XmlElement("Result")]
    public Result? Result { get; set; }
}
