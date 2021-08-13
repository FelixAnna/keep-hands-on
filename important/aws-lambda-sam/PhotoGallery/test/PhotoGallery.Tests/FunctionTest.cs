using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using PhotoGallery;
using Amazon.Lambda.S3Events;

namespace PhotoGallery.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TestToUpperFunction()
        {
            // Invoke the lambda function and confirm the string was upper cased.
            var function = new PhotoFunction();
            var context = new TestLambdaContext();
             
            var upperCase = function.PhotoFunctionHandler(new S3Event(), context);
        }
    }
}
