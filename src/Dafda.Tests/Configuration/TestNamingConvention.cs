﻿using Dafda.Configuration;
using Xunit;

namespace Dafda.Tests.Configuration
{
    public class TestNamingConvention
    {
        [Fact]
        public void Has_correct_name_for_default_conventions()
        {
            var sut = NamingConvention.Default;

            var result = sut.GetKey("group.id");

            Assert.Equal("group.id", result);
        }

        [Fact]
        public void Can_use_custom_naming()
        {
            var sut = NamingConvention.UseCustom(s => s.ToUpper());

            var result = sut.GetKey("group.id");

            Assert.Equal("GROUP.ID", result);
        }

        [Fact]
        public void Can_use_environment_style_naming_convention()
        {
            var sut = NamingConvention.UseEnvironmentStyle();

            var result = sut.GetKey("group.id");

            Assert.Equal("GROUP_ID", result);
        }

        [Fact]
        public void Can_use_environment_style_naming_convention_with_prefix()
        {
            var sut = NamingConvention.UseEnvironmentStyle("DEFAULT_KAFKA");

            var result = sut.GetKey("group.id");

            Assert.Equal("DEFAULT_KAFKA_GROUP_ID", result);
        }
    }
}