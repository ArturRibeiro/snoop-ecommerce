﻿using Shared.Code.Models;
using System.Collections.Generic;

namespace Order.Domain.Models.Orders
{
    /// <summary>
    /// Endereço de entrega do pedido
    /// </summary>
    public class OrderAddress : ValueObject
    {
        /// <summary>
        /// 
        /// </summary>
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }

        //EF
        protected OrderAddress() { }

        public OrderAddress(string street, string city, string state, string country, string zipcode)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipcode;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Using a yield return statement to return each element one at a time
            yield return Street;
            yield return City;
            yield return State;
            yield return Country;
            yield return ZipCode;
        }
    }
}
