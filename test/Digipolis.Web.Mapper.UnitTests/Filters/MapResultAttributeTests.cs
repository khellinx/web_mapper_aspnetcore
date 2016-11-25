using Digipolis.Web.Mapper.Filters;
using Digipolis.Web.Mapper.UnitTests._Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Digipolis.Web.Mapper.UnitTests.Filters
{
    public class MapResultAttributeTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData(typeof(int))]
        [InlineData(typeof(int?))]
        [InlineData(typeof(List<int>))]
        [InlineData(typeof(string))]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(IEnumerable<string>))]
        [InlineData(typeof(PersonDetailModel))]
        [InlineData(typeof(List<PersonDetailModel>))]
        [InlineData(typeof(IList<PersonDetailModel>))]
        [InlineData(typeof(Dictionary<int, PersonDetailModel>))]
        [InlineData(typeof(IDictionary<int, PersonDetailModel>))]
        public void ConstructorWithSourceType_ShouldReturn_AttributeWithDestinationTypeSetToInput(Type to)
        {
            var attribute = new MapResultAttribute(to);

            Assert.Equal(to, attribute.DestinationType);
        }

        [Theory]
        [InlineData(typeof(void))]
        [InlineData(typeof(List<>))]
        [InlineData(typeof(Dictionary<,>))]
        public void ConstructorWithWithNonCustructableSourceType_ShouldThrow_ArgumentException(Type to)
        {
            Assert.Throws<ArgumentException>(() => new MapResultAttribute(to));
        }

        [Theory]
        [InlineData(null, typeof(string))]
        [InlineData(typeof(int), null)]
        [InlineData(typeof(int), typeof(string))]
        [InlineData(typeof(int?), typeof(long?))]
        [InlineData(typeof(List<int>), typeof(IEnumerable<string>))]
        [InlineData(typeof(PersonEntity), typeof(PersonDetailModel))]
        [InlineData(typeof(List<PersonEntity>), typeof(IEnumerable<PersonDetailModel>))]
        [InlineData(typeof(IEnumerable<PersonEntity>), typeof(List<PersonDetailModel>))]
        [InlineData(typeof(IDictionary<int, string>), typeof(Dictionary<string, string>))]
        [InlineData(typeof(Dictionary<int, string>), typeof(IDictionary<string, string>))]
        public void ConstructorWithSourceAndDestinationType_ShouldReturn_AttributeWithSourceAndDestinationTypesSetToInput(Type from, Type to)
        {
            var attribute = new MapResultAttribute(from, to);

            Assert.Equal(from, attribute.SourceType);
            Assert.Equal(to, attribute.DestinationType);
        }

        [Theory]
        [InlineData(typeof(int), typeof(void))]
        [InlineData(typeof(void), typeof(int))]
        [InlineData(typeof(int), typeof(List<>))]
        [InlineData(typeof(List<>), typeof(int))]
        [InlineData(typeof(int), typeof(Dictionary<,>))]
        [InlineData(typeof(Dictionary<,>), typeof(int))]
        public void ConstructorWithWithNonCustructableSourceOrDestinationType_ShouldThrow_ArgumentException(Type from, Type to)
        {
            Assert.Throws<ArgumentException>(() => new MapResultAttribute(from, to));
        }
    }
}
