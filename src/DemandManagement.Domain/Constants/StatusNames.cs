namespace DemandManagement.Domain.Constants;

public static class StatusNames
{
    public const string EnAnalisis = "En Análisis";
    public const string EnDesarrollo = "En Desarrollo";
    public const string EnPruebas = "En Pruebas";
    
    public const string Cerrada = "Cerrada";
    public const string Rechazada = "Rechazada";
    
    public const string Nueva = "Nueva";
    public const string Abierta = "Abierta";
    
    public static readonly string[] InProgressStatuses = new[] 
    { 
        EnAnalisis, 
        EnDesarrollo, 
        EnPruebas 
    };
    
    public static readonly string[] CompletedStatuses = new[] 
    { 
        Cerrada 
    };
}