﻿using Ecommerce.Integration.Tests.Helpers;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Integration.Tests.Scenario.CartFlow
{
    [TestFixture]
    public class CarrinhoDeCompraTests
    {
        private readonly BaseHttpServiceClient _catalogoServiceClient;

        public CarrinhoDeCompraTests()
        {
            _catalogoServiceClient = NativeInjectorBootStrapper.GetInstanceHttpServiceClient<HttpServiceClientCatalog>();
        }

        /// <summary>
        /// Passo 1: Escolher o produto
        /// Passo 2: Adicionar o produto escolhido no carrinho
        /// Passo 3: Fechar o Pedido
        /// Passo 4: Escolher Forma de Pagamento
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Executar()
        {
            var result = await _catalogoServiceClient.GetAsync("Product/GetAllProducts");


            result.IsSuccessStatusCode.Should().BeTrue();
        }

    }
}