﻿// The MIT License (MIT)
// 
// Copyright (c) 2015-2018 Rasmus Mikkelsen
// Copyright (c) 2015-2018 eBay Software Foundation
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

using System.Linq;
using EventFlow.Core;
using EventFlow.TestHelpers;
using FluentAssertions;
using NUnit.Framework;

namespace EventFlow.Tests.UnitTests.Core
{
    [Category(Categories.Unit)]
    public class CircularBufferTests
    {
        [TestCase(1)] // Below capasity
        [TestCase(1, 2)] // At capasity
        [TestCase(1, 2, 3)] // Once above capasity
        [TestCase(1, 2, 3, 4)] // Loop twice over capasity
        [TestCase(1, 2, 3, 4, 5)] // One more than of capasity
        public void Put(params int[] numbers)
        {
            // Arrange
            const int capasity = 2;
            var sut = new CircularBuffer<int>(capasity);

            // Act
            foreach (var number in numbers)
            {
                sut.Put(number);
            }

            // Assert
            var shouldContain = numbers.Reverse().Take(capasity).ToList();
            sut.Should().Contain(shouldContain);
        }
    }
}