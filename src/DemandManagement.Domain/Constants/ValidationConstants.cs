namespace DemandManagement.Domain.Constants;

public static class ValidationConstants
{
    // Demand
    public static class Demand
    {
        public const int TitleMinLength = 3;
        public const int TitleMaxLength = 200;
        public const int DescriptionMaxLength = 2000;
    }
    
    // User
    public static class User
    {
        public const int FullNameMaxLength = 200;
        public const int EmailMaxLength = 200;
        public const int DepartmentMaxLength = 100;
    }
    
    // DemandType & Status
    public static class Catalog
    {
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 500;
        public const int ServiceLevelMaxLength = 100;
    }
    
    // AssociatedDocument
    public static class Document
    {
        public const int FileNameMaxLength = 255;
        public const int FileTypeMaxLength = 50;
        public const int PathMaxLength = 500;
    }
    
    // Pagination
    public static class Pagination
    {
        public const int MinPageNumber = 1;
        public const int MinPageSize = 1;
        public const int MaxPageSize = 100;
        public const int DefaultPageSize = 10;
        public const int DefaultRecentItemsCount = 5;
    }
    
    // Search
    public static class Search
    {
        public const int SearchTermMaxLength = 200;
    }
}