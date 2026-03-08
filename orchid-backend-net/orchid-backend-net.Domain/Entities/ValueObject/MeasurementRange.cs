using orchid_backend_net.Domain.Common.Exceptions;

namespace orchid_backend_net.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing acceptable measurement range for stage requirements.
    /// Encapsulates validation logic for measured values.
    /// </summary>
    public record MeasurementRange
    {
        public decimal? MinValue { get; init; }
        public decimal? MaxValue { get; init; }

        private MeasurementRange(decimal? minValue, decimal? maxValue)
        {
            // Validate business rule: min cannot be greater than max
            if (minValue.HasValue && maxValue.HasValue && minValue > maxValue)
                throw new DomainException("Giá trị min không thể lớn hơn max.");
            
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Factory method to create range with both min and max.
        /// </summary>
        public static MeasurementRange Create(decimal? minValue, decimal? maxValue)
            => new(minValue, maxValue);

        /// <summary>
        /// Factory method to create range with no limits.
        /// </summary>
        public static MeasurementRange NoLimit()
            => new(null, null);

        /// <summary>
        /// Validates if measured value is within acceptable range.
        /// Returns true if value is valid, false otherwise.
        /// </summary>
        public bool IsValueInRange(decimal measuredValue)
        {
            if (MinValue.HasValue && measuredValue < MinValue.Value)
                return false;
            
            if (MaxValue.HasValue && measuredValue > MaxValue.Value)
                return false;
            
            return true;
        }

        /// <summary>
        /// Returns human-readable range description.
        /// </summary>
        public override string ToString()
        {
            if (!MinValue.HasValue && !MaxValue.HasValue)
                return "Không giới hạn";
            
            if (!MinValue.HasValue)
                return $"≤ {MaxValue}";
            
            if (!MaxValue.HasValue)
                return $"≥ {MinValue}";
            
            return $"{MinValue} - {MaxValue}";
        }
    }
}