using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Todo.Services;
using Xunit;

namespace Todo.Tests.Services
{
    public class GravatarServiceTests
    {
        [Fact]
        public void GetSha256Hash_CalculatesHashCorrectly()
        {
            var aRandomEmailAddress = "aRandomEmailAddress@gmail.com";

            var result = GravatarHashService.GetSha256Hash(aRandomEmailAddress);

            // This has been calculated using an online tool
            result.Should().Be("f8ff525f15202122f50ab36cfb199db28e23c665f21e383288aab1be846bf34e");
        }
    }
}
