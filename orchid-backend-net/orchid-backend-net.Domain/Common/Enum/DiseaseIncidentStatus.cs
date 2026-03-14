namespace orchid_backend_net.Domain.Common.Enum
{
    public enum DiseaseIncidentStatus
    {
        AIDetected,    // AI vừa phát hiện, chưa có Researcher/Technician review
        UnderReview,   // Researcher/Technician đang kiểm tra thực tế
        Confirmed,     // Researcher/Technician xác nhận đúng là bệnh
        Dismissed      // Researcher/Technician xác nhận AI phán đoán sai
    }
}
