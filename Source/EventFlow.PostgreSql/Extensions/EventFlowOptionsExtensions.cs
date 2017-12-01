// The MIT License (MIT)
// 
// Copyright (c) 2015-2017 Rasmus Mikkelsen
// Copyright (c) 2015-2017 eBay Software Foundation
// https://github.com/eventflow/EventFlow
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using EventFlow.Configuration;
using EventFlow.Extensions;
using EventFlow.ReadStores;
using EventFlow.Sql.ReadModels;
using EventFlow.PostgreSql.Connections;
using EventFlow.PostgreSql.EventStores;
using EventFlow.PostgreSql.ReadStores;
using EventFlow.PostgreSql.RetryStrategies;

namespace EventFlow.PostgreSql.Extensions
{
    public static class EventFlowOptionsExtensions
    {
        public static IEventFlowOptions ConfigurePostgreSql(
            this IEventFlowOptions eventFlowOptions,
            IPostgreSqlConfiguration sqLiteConfiguration)
        {
            return eventFlowOptions
                .RegisterServices(f =>
                {
                    f.Register<IPostgreSqlConnection, PostgreSqlConnection>();
                    f.Register<IPostgreSqlConnectionFactory, PostgreSqlConnectionFactory>();
                    f.Register<IPostgreSqlErrorRetryStrategy, PostgreSqlErrorRetryStrategy>();
                    f.Register(_ => sqLiteConfiguration, Lifetime.Singleton);
                });
        }

        public static IEventFlowOptions UsePostgreSqlEventStore(
            this IEventFlowOptions eventFlowOptions)
        {
            return eventFlowOptions
                .UseEventStore<PostgreSqlEventPersistence>();
        }

        public static IEventFlowOptions UsePostgreSqlReadModel<TReadModel, TReadModelLocator>(
            this IEventFlowOptions eventFlowOptions)
            where TReadModel : class, IReadModel
            where TReadModelLocator : IReadModelLocator
        {
            return eventFlowOptions
                .RegisterServices(f =>
                    {
                        f.Register<IReadModelSqlGenerator, ReadModelSqlGenerator>(Lifetime.Singleton, true);
                        f.Register<IPostgreSqlReadModelStore<TReadModel>, PostgreSqlReadModelStore<TReadModel>>();
                        f.Register<IReadModelStore<TReadModel>>(r => r.Resolver.Resolve<IPostgreSqlReadModelStore<TReadModel>>());
                    })
                .UseReadStoreFor<IPostgreSqlReadModelStore<TReadModel>, TReadModel, TReadModelLocator>();
        }

        public static IEventFlowOptions UsePostgreSqlReadModel<TReadModel>(
            this IEventFlowOptions eventFlowOptions)
            where TReadModel : class, IReadModel
        {
            return eventFlowOptions
                .RegisterServices(f =>
                    {
                        f.Register<IReadModelSqlGenerator, ReadModelSqlGenerator>(Lifetime.Singleton, true);
                        f.Register<IPostgreSqlReadModelStore<TReadModel>, PostgreSqlReadModelStore<TReadModel>>();
                        f.Register<IReadModelStore<TReadModel>>(r => r.Resolver.Resolve<IPostgreSqlReadModelStore<TReadModel>>());
                    })
                .UseReadStoreFor<IPostgreSqlReadModelStore<TReadModel>, TReadModel>();
        }
    }
}