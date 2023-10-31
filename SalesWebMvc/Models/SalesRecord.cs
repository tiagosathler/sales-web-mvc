﻿using SalesWebMVC.Models.Enums;

namespace SalesWebMVC.Models
{
    public class SalesRecord

    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public SalesStatus Status { get; set; }
        public Seller Seller { get; set; } = default!;

        public SalesRecord()
        { }

        public SalesRecord(int id, DateTime date, double amount, SalesStatus status, Seller seller)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Status = status;
            Seller = seller;
        }

        public override bool Equals(object? obj)
        {
            return obj is SalesRecord record &&
                   Id == record.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}