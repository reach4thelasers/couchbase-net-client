﻿ using System;
 using System.Threading;
 using Common.Logging;
 using Couchbase.Configuration;
 using Couchbase.Configuration.Server.Providers;
 using Couchbase.IO.Operations;

namespace Couchbase.Core.Buckets
{
    public class MemcachedBucket : IBucket, IConfigObserver
    {
        private readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly IClusterManager _clusterManager;
        private IConfigInfo _configInfo;
        private volatile bool _disposed;

         internal MemcachedBucket(IClusterManager clusterManager, string bucketName)
        {
            _clusterManager = clusterManager;
            Name = bucketName;
        }

        public string Name { get; set; }

        void IConfigObserver.NotifyConfigChanged(IConfigInfo configInfo)
        {
            Interlocked.Exchange(ref _configInfo, configInfo);
        }

        public IOperationResult<T> Insert<T>(string key, T value)
        {
            var keyMapper = _configInfo.GetKeyMapper(Name);
            var bucket = keyMapper.MapKey(key);
            var server = bucket.LocatePrimary();

            var operation = new SetOperation<T>(key, value, null);
            var operationResult = server.Send(operation);
            return operationResult;
        }
        
        public IOperationResult<T> Get<T>(string key)
        {
            var keyMapper = _configInfo.GetKeyMapper(Name);
            var bucket = keyMapper.MapKey(key);
            var server = bucket.LocatePrimary();

            var operation = new GetOperation<T>(key, null);
            var operationResult = server.Send(operation);
            return operationResult;
        }

        public System.Threading.Tasks.Task<IOperationResult<T>> GetAsync<T>(string key)
        {
            throw new NotImplementedException("This method is only supported on Couchbase Bucket (persistent) types.");
        }

        public System.Threading.Tasks.Task<IOperationResult<T>> InsertAsync<T>(string key, T value)
        {
            throw new NotImplementedException("This method is only supported on Couchbase Bucket (persistent) types.");
        }

        public void Dispose()
        {
            throw new NotImplementedException("This method is only supported on Couchbase Bucket (persistent) types.");
        }

        public Views.IViewResult<T> Get<T>(Views.IViewQuery query)
        {
            throw new NotImplementedException("This method is only supported on Couchbase Bucket (persistent) types.");
        }

        public N1QL.IQueryResult<T> Query<T>(string query)
        {
            throw new NotImplementedException("This method is only supported on Couchbase Bucket (persistent) types.");
        }

        public Views.IViewQuery CreateQuery(bool development)
        {
            throw new NotImplementedException("This method is only supported on Couchbase Bucket (persistent) types.");
        }

        public Views.IViewQuery CreateQuery(string designdoc, bool development)
        {
            throw new NotImplementedException();
        }

        public Views.IViewQuery CreateQuery(string designdoc, string view, bool development)
        {
            throw new NotImplementedException();
        }
    }
}
