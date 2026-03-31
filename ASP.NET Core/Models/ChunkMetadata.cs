namespace ASP_NET_Core.Models;

public class ChunkMetadata {
    public string uploadId { get; set; }
    public string fileName { get; set; }
    public long index { get; set; }
    public long totalCount { get; set; }
    public long fileSize { get; set; }
}
