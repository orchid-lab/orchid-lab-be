using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Sample.Helper
{
    public static class CreateSampleByQuantity
    {
        public static List<Samples> CreateMultipleSample(
            string experimentLogName,
            string experimentLogId,
            int quantity)
        {
            var sampleList = new List<Samples>();
            for (int i = 0; i <= quantity; i++)
            {
                var sample = new Samples()
                {
                    ExperimentLogId = experimentLogId,
                    Name = $"Mẫu vật số {i + 1} của thí nghiệm {experimentLogName}",
                };
                sampleList.Add(sample);
            }
            return sampleList;
        }
    }
}
