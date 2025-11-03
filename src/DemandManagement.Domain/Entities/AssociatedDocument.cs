using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Entities;

public sealed class AssociatedDocument : BaseEntity<DocumentId>
{
    public DemandId DemandId { get; private set; }
    public string FileName { get; private set; }
    public string FileType { get; private set; }
    public string Path { get; private set; }
    public DateTimeOffset UploadDate { get; private set; }
    public UserId UploadedBy { get; private set; }

    private AssociatedDocument() : base(default!)
    {
        DemandId = default!;
        FileName = string.Empty;
        FileType = string.Empty;
        Path = string.Empty;
        UploadedBy = default!;
    }

    public AssociatedDocument(DocumentId id,
                              DemandId demandId,
                              string fileName,
                              string fileType,
                              string path,
                              DateTimeOffset uploadDate,
                              UserId uploadedBy) : base(id)
    {
        FileName = string.IsNullOrWhiteSpace(fileName) ? throw new ArgumentException("FileName required", nameof(fileName)) : fileName.Trim();
        FileType = string.IsNullOrWhiteSpace(fileType) ? throw new ArgumentException("FileType required", nameof(fileType)) : fileType.Trim();
        Path = string.IsNullOrWhiteSpace(path) ? throw new ArgumentException("Path required", nameof(path)) : path.Trim();
        DemandId = demandId;
        UploadDate = uploadDate;
        UploadedBy = uploadedBy;
    }

    public static AssociatedDocument Create(DemandId demandId,
                                            string fileName,
                                            string fileType,
                                            string path,
                                            UserId uploadedBy)
    {
        var id = DocumentId.New();
        return new AssociatedDocument(id, demandId, fileName, fileType, path, DateTimeOffset.UtcNow, uploadedBy);
    }
}