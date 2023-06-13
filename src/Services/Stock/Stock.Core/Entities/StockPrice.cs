﻿namespace Stock.Core.Entities
{
    public class StockPrice : AuditableEntity, IEntity
    {
        public long Id { get; set; }
        public decimal Price { get; set; }
        public int StockId { get; set; }
        public virtual Stock Stock { get; set; }
    }
}