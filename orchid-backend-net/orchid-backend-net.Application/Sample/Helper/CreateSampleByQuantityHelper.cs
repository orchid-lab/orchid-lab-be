using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Sample.Helper
{
    public static class CreateSampleByQuantityHelper
    {
        public static List<Samples> CreateMultipleSample(
            string experimentLogName,
            string experimentLogId,
            int firstStageDefinitionId,
            int quantity,
            string userId)
        {
            var sampleList = new List<Samples>();
            for (int i = 0; i < quantity; i++)
            {
                var sample = new Samples()
                {
                    ExperimentLogId = experimentLogId,
                    Name = $"Mẫu vật số {i + 1} của thí nghiệm {experimentLogName}",
                    CreatedDate = DateTime.Now,
                    CreatedBy = userId
                };
                sampleList.Add(sample);
                sample.StartOnCreation(firstStageDefinitionId);
            }
            return sampleList;
        }
    }
}
