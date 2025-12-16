using Microsoft.Extensions.Caching.Memory;

namespace HotelManagement.Application.CachingContract;

public class HotelRoomsCache : IHotelRoomsCache
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5); // TTL 5 хвилин

        public HotelRoomsCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public List<HotelRoomCacheModel>? GetRooms(Guid hotelId)
        {
            _cache.TryGetValue(hotelId, out List<HotelRoomCacheModel>? rooms);
            return rooms;
        }

        public void SetRooms(Guid hotelId, List<HotelRoomCacheModel> rooms)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration, // TTL
                Priority = CacheItemPriority.Normal
            };
            _cache.Set(hotelId, rooms, options);
        }
}