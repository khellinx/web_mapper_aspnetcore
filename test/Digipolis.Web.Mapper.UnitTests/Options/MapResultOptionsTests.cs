using Digipolis.Web.Mapper.Options;
using Digipolis.Web.Mapper.UnitTests._Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Digipolis.Web.Mapper.UnitTests.Options
{
    public class MapResultOptionsTests
    {
        [Theory]
        [InlineData(typeof(int?), typeof(long?))]
        [InlineData(typeof(int), typeof(string))]
        [InlineData(typeof(List<int>), typeof(List<string>))]
        [InlineData(typeof(PersonEntity), typeof(PersonDetailModel))]
        [InlineData(typeof(List<PersonEntity>), typeof(IEnumerable<PersonDetailModel>))]
        [InlineData(typeof(IEnumerable<PersonDetailModel>), typeof(List<PersonEntity>))]
        public void AddMappingWithSourceAndDestinationType_ShouldReturn_DestinationTypeOnGetWithSourceTypeAsInput(Type source, Type destination)
        {
            var options = new MapResultOptions();

            options.AddMapping(source, destination);

            var result = options.GetMapping(source);
            Assert.Equal(destination, result);
        }

        [InlineData(null, null)]
        [InlineData(null, typeof(int))]
        [InlineData(typeof(int), null)]
        public void AddMappingWithSourceOrDestinationTypeEqualToNull_ShouldThrow_ArgumentNullException(Type source, Type destination)
        {
            var options = new MapResultOptions();

            Assert.Throws<ArgumentNullException>(() => options.AddMapping(source, destination));
        }

        [Theory]
        [InlineData(typeof(int), typeof(void))]
        [InlineData(typeof(void), typeof(int))]
        [InlineData(typeof(int), typeof(List<>))]
        [InlineData(typeof(List<>), typeof(int))]
        [InlineData(typeof(int), typeof(Dictionary<,>))]
        [InlineData(typeof(Dictionary<,>), typeof(int))]
        public void AddMappingWithWithNonCustructedSourceOrDestinationType_ShouldThrow_ArgumentException(Type source, Type destination)
        {
            var options = new MapResultOptions();

            Assert.Throws<ArgumentException>(() => options.AddMapping(source, destination));
        }

        [Theory]
        [InlineData(typeof(List<>), typeof(IEnumerable<>))]
        [InlineData(typeof(IEnumerable<>), typeof(List<>))]
        [InlineData(typeof(Dictionary<,>), typeof(IDictionary<,>))]
        public void AddGenericMappingWithConstructedGenericSourceAndDestinationType_ShouldReturn_DestinationTypeOnGetWithSourceTypeAsInput(Type source, Type destination)
        {
            var options = new MapResultOptions();

            options.AddGenericMapping(source, destination);

            var result = options.GetGenericMapping(source);
            Assert.Equal(destination, result);
        }

        [InlineData(null, null)]
        [InlineData(null, typeof(int))]
        [InlineData(typeof(int), null)]
        public void AddGenericMappingWithSourceOrDestinationTypeEqualToNull_ShouldThrow_ArgumentNullException(Type source, Type destination)
        {
            var options = new MapResultOptions();

            Assert.Throws<ArgumentNullException>(() => options.AddGenericMapping(source, destination));
        }

        [Theory]
        [InlineData(typeof(List<>), typeof(void))]
        [InlineData(typeof(void), typeof(List<>))]
        [InlineData(typeof(int), typeof(string))]
        [InlineData(typeof(List<int>), typeof(List<string>))]
        [InlineData(typeof(PersonEntity), typeof(PersonDetailModel))]
        [InlineData(typeof(List<PersonEntity>), typeof(IEnumerable<PersonDetailModel>))]
        public void AddGenericMappingWithWithNonCustructedSourceOrDestinationType_ShouldThrow_ArgumentException(Type source, Type destination)
        {
            var options = new MapResultOptions();

            Assert.Throws<ArgumentException>(() => options.AddGenericMapping(source, destination));
        }
    }
}
