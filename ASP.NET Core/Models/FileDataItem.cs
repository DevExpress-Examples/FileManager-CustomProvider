using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASP_NET_Core.Models;

public class FileDataItem {
    [Key]
    [JsonPropertyName("key")]
    public string Key { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("dateCreated")]
    public DateTime Created { get; set; }
    [JsonPropertyName("dateModified")]
    public DateTime Modified { get; set; }
    [JsonPropertyName("isDirectory")]
    public bool IsDirectory { get; set; }
    [JsonPropertyName("hasSubDirectories")]
    public bool HasSubDirectories { get; set; }
}
