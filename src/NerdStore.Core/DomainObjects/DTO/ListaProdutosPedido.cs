using System;
using System.Collections.Generic;

namespace NerdStore.Core.DomainObjects.DTO
{
    public class ListaProdutosPedido
    {
        public Guid ProdutoId { get; set; }
        public ICollection<Item> Itens { get; set; }
    }
}