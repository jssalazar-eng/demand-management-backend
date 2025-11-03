using System;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Entities;

public sealed class AssociatedDocument : BaseEntity<DocumentId>
{
    public DemandId DemandId { get; init; }
    public string FileName { get; init; }
    public string FileType { get; init; }
    public string Path { get; init; }
    public DateTimeOffset UploadDate { get; init; }
    public UserId UploadedBy { get; init; }

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