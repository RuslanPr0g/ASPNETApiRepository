namespace YTSearch.Shared.Models
{
    public struct SuccessOrFailure<T>
    {
        public bool IsNull { get; set; }
        public T Value { get; set; }

        public bool HasValue => !IsNull;
        public string NullMessage { get; set; }

        public static SuccessOrFailure<T> CreateValue(T value)
        {
            return new SuccessOrFailure<T>
            {
                IsNull = false,
                Value = value
            };
        }

        public static SuccessOrFailure<T> CreateNull(string nullMessage = null)
        {
            return new SuccessOrFailure<T>
            {
                IsNull = true,
                Value = default,
                NullMessage = nullMessage
            };
        }

        public static implicit operator SuccessOrFailure<T>(T value)
        {
            if (value == null)
            {
                return CreateNull();
            }
            return CreateValue(value);
        }
    }
}
