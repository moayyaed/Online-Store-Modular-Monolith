using System;
using Common.Utils.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using StackExchange.Redis;

namespace Common.Caching.Redis
{
    public class RedisMemoryCache : ExtendedMemoryCache
    {
        private readonly ISubscriber _bus;
        private readonly RedisCachingOptions _redisCachingOptions;
        private readonly ILogger _log;
        private readonly RetryPolicy _retryPolicy;

        private static readonly string _cacheId = Guid.NewGuid().ToString("N");
        

        public RedisMemoryCache(IMemoryCache memoryCache, IOptions<CachingOptions> options
            , ISubscriber bus
            , IOptions<RedisCachingOptions> redisCachingOptions
            , ILogger<RedisMemoryCache> log
            ) : base(memoryCache, options, log)
       {
            _log = log;
            _bus = bus;

            _redisCachingOptions = redisCachingOptions.Value;
            _bus.Unsubscribe(_redisCachingOptions.ChannelName);
            _bus.Subscribe(_redisCachingOptions.ChannelName, OnMessage);

            _log.LogInformation($"{nameof(RedisMemoryCache)}: subscribe to channel {_redisCachingOptions.ChannelName } current instance:{ _cacheId }");

            _retryPolicy = Policy.Handle<Exception>().WaitAndRetry(
                _redisCachingOptions.BusRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt - 1)));
        }
        

        private void OnMessage(RedisChannel channel, RedisValue redisValue)
        {
            var message = JsonConvert.DeserializeObject<RedisCachingMessage>(redisValue);

            if (!string.IsNullOrEmpty(message.Id) && !message.Id.EqualsInvariant(_cacheId))
            {
                foreach (var item in message.CacheKeys)
                {
                    base.Remove(item);

                    _log.LogInformation($"{nameof(RedisMemoryCache)}: channel[{_redisCachingOptions.ChannelName }] remove local cache that cache key is {item} from instance:{ _cacheId }");
                }
            }
        }

        protected override void EvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            _log.LogInformation($"{nameof(RedisMemoryCache)}: channel[{_redisCachingOptions.ChannelName }] sending a message with key:{key} from instance:{ _cacheId } to all subscribers");

            var message = new RedisCachingMessage { Id = _cacheId, CacheKeys = new[] { key } };
            _retryPolicy.Execute(() => _bus.Publish(_redisCachingOptions.ChannelName, JsonConvert.SerializeObject(message)));

            base.EvictionCallback(key, value, reason, state);
        }
    }
}
