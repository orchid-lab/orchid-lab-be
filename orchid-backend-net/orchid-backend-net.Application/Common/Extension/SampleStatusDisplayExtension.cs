namespace orchid_backend_net.Application.Common.Extension
{
    public static class SampleStatusDisplayExtension
    {
        public static string ToDisplayText(this Domain.Common.Enum.SampleStatus sampleStatus)
            => sampleStatus switch
            {
                Domain.Common.Enum.SampleStatus.Created => "mới tạo",
                Domain.Common.Enum.SampleStatus.InProgressed => "đang trong quá trình thí nghiệm",
                Domain.Common.Enum.SampleStatus.Completed => "đã hoàn thành thí nghiệm",
                Domain.Common.Enum.SampleStatus.ExecutedBecauseOfDisease => "đã bị hủy do bệnh",
                Domain.Common.Enum.SampleStatus.ConvertedToSeedling => "đã chuyển thành seedling",
                _ => throw new NotImplementedException()
            };
    }
}
