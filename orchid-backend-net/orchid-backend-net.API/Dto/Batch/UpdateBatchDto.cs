namespace orchid_backend_net.API.Dto.Batch
{
    /// <summary>
    /// use this dto to transfer to update batch command
    /// </summary>
    /// <param name="LabRoomId"></param>
    /// <param name="BatchName"></param>
    /// <param name="BatchSizeWidth"></param>
    /// <param name="BatchSizeHeight"></param>
    /// <param name="WidthUnit"></param>
    /// <param name="HeightUnit"></param>
    public record UpdateBatchDto(
        int? LabRoomId,
        string? BatchName,
        decimal? BatchSizeWidth,
        decimal? BatchSizeHeight,
        string? WidthUnit,
        string? HeightUnit);
}
