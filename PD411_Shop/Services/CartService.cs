using PD411_Shop.ViewModels;

namespace PD411_Shop.Services
{
    public static class CartService
    {
        public static void AddToCart(ISession session, int productId)
        {
            if (!IsInCart(session, productId))
            {
                var items = session.Get<List<CartItemVM>>() ?? new List<CartItemVM>();
                var newItem = new CartItemVM { ProductId = productId };
                items.Add(newItem);
                session.Set(items);
            }
        }

        public static void RemoveFromCart(ISession session, int productId)
        {
            var items = session.Get<List<CartItemVM>>() ?? new List<CartItemVM>();
            items = items.Where(i => i.ProductId != productId).ToList();
            session.Set(items);
        }

        public static void Increment(ISession session, int productId)
        {
            var items = session.Get<List<CartItemVM>>() ?? new List<CartItemVM>();
            var item = items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.Count++;
                session.Set(items);
            }
        }

        public static void Decrement(ISession session, int productId)
        {
            var items = session.Get<List<CartItemVM>>() ?? new List<CartItemVM>();
            var item = items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.Count--;
                session.Set(items);
            }
        }

        public static bool IsInCart(ISession session, int productId)
        {
            var items = session.Get<List<CartItemVM>>() ?? new List<CartItemVM>();
            return items.Any(x => x.ProductId == productId);
        }

        public static int ItemsCount(ISession session)
        {
            var items = session.Get<List<CartItemVM>>() ?? new List<CartItemVM>();
            return items.Count();
        }
    }
}
