using GeekShopping.CartAPI.Model;
using GeekShopping.CartAPI.Model.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CartApi.Data.ValueObjects
{
    public class CartDetailVO
    {
        public long Id { get; set; }
        public long CartHeaderId { get; set; }

        public CartHeaderVO CartHeader { get; set; }
        public long ProductId {  get; set; }
        public ProductVO Product { get; set; }

        public int Count { get; set; }
    }
}
