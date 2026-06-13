using System.Text.Json;
using YasmahCraft.Models;

namespace YasmahCraft.Services
{
    public interface ICartService
    {
        List<CartItem> GetCart(ISession session);
        void AddToCart(ISession session, CartItem item);
        void RemoveFromCart(ISession session, Guid productId, string size);
        void UpdateQuantity(ISession session, Guid productId, string size, int quantity);
        void ClearCart(ISession session);
        int GetCartCount(ISession session);
        decimal GetCartTotal(ISession session);
    }

    public class CartService : ICartService
    {
        private const string CartSessionKey = "Cart";

        public List<CartItem> GetCart(ISession session)
        {
            var cartJson = session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        private void SaveCart(ISession session, List<CartItem> cart)
        {
            session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
        }

        public void AddToCart(ISession session, CartItem item)
        {
            var cart = GetCart(session);
            var existing = cart.FirstOrDefault(c => c.ProductId == item.ProductId && c.Size == item.Size);

            if (existing != null)
            {
                existing.Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }

            SaveCart(session, cart);
        }

        public void RemoveFromCart(ISession session, Guid productId, string size)
        {
            var cart = GetCart(session);
            cart.RemoveAll(c => c.ProductId == productId && c.Size == size);
            SaveCart(session, cart);
        }

        public void UpdateQuantity(ISession session, Guid productId, string size, int quantity)
        {
            var cart = GetCart(session);
            var item = cart.FirstOrDefault(c => c.ProductId == productId && c.Size == size);
            if (item != null)
            {
                if (quantity <= 0)
                    cart.Remove(item);
                else
                    item.Quantity = quantity;
            }
            SaveCart(session, cart);
        }

        public void ClearCart(ISession session)
        {
            session.Remove(CartSessionKey);
        }

        public int GetCartCount(ISession session)
        {
            return GetCart(session).Sum(c => c.Quantity);
        }

        public decimal GetCartTotal(ISession session)
        {
            return GetCart(session).Sum(c => c.TotalPrice);
        }
    }
}