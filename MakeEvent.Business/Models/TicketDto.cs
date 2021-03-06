﻿using System;

namespace MakeEvent.Business.Models
{
    public class TicketDto
    {
        public int Id { get; set; }
        public int CategoryId     { get; set; }

        public string OwnerFirstName { get; set; }
        public string OwnerLastName  { get; set; }
        public string OwnerPhone     { get; set; }
        public string OwnerEmail     { get; set; }

        public string  EventTitle    { get; set; }
        public decimal Cost          { get; set; }
        public string  Status        { get; set; }

        public bool    IsPaid        { get; set; }

        public DateTime? BookDate    { get; set; }
    }
}
