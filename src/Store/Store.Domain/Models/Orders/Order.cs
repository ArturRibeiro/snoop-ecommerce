﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Shared.Code.Models;
using Store.Domain.Models.Orders.Validations;

namespace Store.Domain.Models.Orders
{
    /// <summary>
    /// Pedido
    /// </summary>
    public class Order : Entity<int>, IAggregateRoot
    {
        private readonly IList<OrderItem> _items = new List<OrderItem>();

        #region Constructor
        protected Order() { }

        private Order(OrderAddress address, Guid customerId, Guid paymentId)
        {
            Address = address;
            CustomerId = customerId;
            PaymentId = paymentId;
        }
        #endregion

        #region Properties

        public IReadOnlyCollection<OrderItem> Items => new ReadOnlyCollection<OrderItem>(_items);

        /// <summary>
        /// Endereço de entrega
        /// </summary>
        public OrderAddress Address { get; private set; }

        /// <summary>
        /// Cliente que realizou a compra
        /// </summary>
        public Guid CustomerId { get; private set; }

        /// <summary>
        /// Código do Pedido
        /// </summary>
        public string OrderNumber { get; private set; }

        public DateTime OrderDate { get; private set; } = DateTime.Now;

        public OrderStatus Status { get; private set; } = OrderStatus.Submitted;

        /// <summary>
        /// Códido da forma como foi paga o pedido
        /// </summary>
        public Guid PaymentId { get; private set; }

        #endregion

        public void AddItem(Guid productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
        {
            _items.Add(OrderItem.Create(productId, productName, unitPrice, discount, pictureUrl, units));

        }

        public static Order Create(OrderAddress address, Guid customerId, Guid paymentId)
            => new Order(address, customerId, paymentId);

        public bool IsValid()
            => new OrderValidation().Validate(this).IsValid;
    }
}
