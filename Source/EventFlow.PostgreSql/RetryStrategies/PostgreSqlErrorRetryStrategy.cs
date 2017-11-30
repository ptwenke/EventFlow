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

using System;
using Npgsql;
using EventFlow.Core;
using EventFlow.PostgreSql.Connections;

namespace EventFlow.PostgreSql.RetryStrategies
{
    public class PostgreSqlErrorRetryStrategy : IPostgreSqlErrorRetryStrategy
    {
        private readonly IPostgreSqlConfiguration _configuration;

        public PostgreSqlErrorRetryStrategy(
            IPostgreSqlConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Retry ShouldThisBeRetried(Exception exception, TimeSpan totalExecutionTime, int currentRetryCount)
        {
            var postgreSqlException = exception as NpgsqlException;
            if (postgreSqlException == null || currentRetryCount > _configuration.TransientRetryCount)
            {
                return Retry.No;
            }

            return Retry.No;
            /*switch (postgreSqlException.ErrorCode)
            {
                // https://www.PostgreSql.org/rescode.html#locked
                // The PostgreSql_LOCKED result code indicates that a write operation could not continue because of a
                // conflict within the same database connection or a conflict with a different database connection
                // that uses a shared cache.
                case "40P01":

                // https://www.PostgreSql.org/rescode.html#busy
                // The PostgreSql_BUSY result code indicates that the database file could not be written (or in some cases
                // read) because of concurrent activity by some other database connection, usually a database
                // connection in a separate process.
                case postgreSqlException.Busy:
                    return Retry.YesAfter(_configuration.TransientRetryDelay.PickDelay());

                default:
                    return Retry.No;
            }*/
        }
    }
}
