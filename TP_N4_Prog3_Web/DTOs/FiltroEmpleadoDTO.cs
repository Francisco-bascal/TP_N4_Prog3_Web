namespace TP_N4_Prog3_Web.DTOs
{
    public record FiltroEmpleadoDTO
    (
        int? IdDepartamento,
        string? Busqueda,
        bool? Estado,
        int? CantidadHijosMin,
        int? CantidadHijosMax,
        DateOnly? FechaIngresoDesde,
        DateOnly? FechaIngresoHasta
    );
}