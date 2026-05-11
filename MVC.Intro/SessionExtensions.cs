using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace MVC.Intro
{
    public static class SessionExtensions
    {
        public const string FavoritesKey = "FavoritesProductIds";
        public const string CartKey = "CartItems";

        public static List<Guid> GetFavoriteProductIds(this ISession session)
        {
            var json = session.GetString(FavoritesKey);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<Guid>();
            }

            try
            {
                var ids = JsonSerializer.Deserialize<List<Guid>>(json);
                return ids ?? new List<Guid>();
            }
            catch
            {
                return new List<Guid>();
            }
        }

        public static void SetFavoriteProductIds(this ISession session, List<Guid> ids)
        {
            var json = JsonSerializer.Serialize(ids);
            session.SetString(FavoritesKey, json);
        }

        public static bool ToggleFavoriteProductId(this ISession session, Guid id)
        {
            var ids = session.GetFavoriteProductIds();
            var removed = ids.Remove(id);
            if (!removed)
            {
                ids.Add(id);
            }

            session.SetFavoriteProductIds(ids);
            return !removed;
        }

        public static void RemoveFavoriteProductId(this ISession session, Guid id)
        {
            var ids = session.GetFavoriteProductIds();
            ids.Remove(id);
            session.SetFavoriteProductIds(ids);
        }

        public static void ClearFavoriteProductIds(this ISession session)
        {
            session.Remove(FavoritesKey);
        }

        public static Dictionary<Guid, int> GetCartItems(this ISession session)
        {
            var json = session.GetString(CartKey);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new Dictionary<Guid, int>();
            }

            try
            {
                var items = JsonSerializer.Deserialize<Dictionary<Guid, int>>(json);
                return items ?? new Dictionary<Guid, int>();
            }
            catch
            {
                return new Dictionary<Guid, int>();
            }
        }

        public static void SetCartItems(this ISession session, Dictionary<Guid, int> items)
        {
            var json = JsonSerializer.Serialize(items);
            session.SetString(CartKey, json);
        }

        public static void AddToCart(this ISession session, Guid productId, int quantity = 1)
        {
            if (quantity <= 0)
            {
                return;
            }

            var items = session.GetCartItems();
            if (items.TryGetValue(productId, out var existing))
            {
                items[productId] = existing + quantity;
            }
            else
            {
                items[productId] = quantity;
            }

            session.SetCartItems(items);
        }

        public static void DecreaseFromCart(this ISession session, Guid productId, int quantity = 1)
        {
            if (quantity <= 0)
            {
                return;
            }

            var items = session.GetCartItems();
            if (!items.TryGetValue(productId, out var existing))
            {
                return;
            }

            var next = existing - quantity;
            if (next <= 0)
            {
                items.Remove(productId);
            }
            else
            {
                items[productId] = next;
            }

            session.SetCartItems(items);
        }

        public static void RemoveFromCart(this ISession session, Guid productId)
        {
            var items = session.GetCartItems();
            if (items.Remove(productId))
            {
                session.SetCartItems(items);
            }
        }

        public static void ClearCart(this ISession session)
        {
            session.Remove(CartKey);
        }

        public static int GetCartTotalQuantity(this ISession session)
        {
            var items = session.GetCartItems();
            return items.Values.Sum();
        }
    }
}
