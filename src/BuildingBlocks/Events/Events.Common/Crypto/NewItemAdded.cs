﻿namespace Events.Common.Crypto
{
    public class NewItemAdded
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; } = default!;
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public string Currency { get; set; } = default!;
        public Guid CorrelationId { get; set; }
    }
}
