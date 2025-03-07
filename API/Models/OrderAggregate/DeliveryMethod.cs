﻿namespace API.Models.OrderAggregate
{
    public class DeliveryMethod : BaseEntity
    {
        public string? ShortNime { get; set; }
        public string? DeliveryTime { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}
