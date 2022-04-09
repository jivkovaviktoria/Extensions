﻿namespace TryAtSoftware.Extensions.Reflection.Tests;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TryAtSoftware.Extensions.Reflection.Tests.Types;
using Xunit;

public static class TypeNamesTests
{
    [Theory]
    [MemberData(nameof(GetTestData))]
    [MemberData(nameof(GetOpenGenericsTestData))]
    public static void TypeNamesShouldBeCorrectlyRetrieved(Type type, string expected)
    {
        Assert.NotNull(type);
        Assert.False(string.IsNullOrWhiteSpace(expected));
        Assert.Equal(expected, TypeNames.Get(type));
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public static void TypeNamesShouldBeCorrectlyRetrievedWithTheGenericShorthand(Type type, string expected)
    {
        Assert.NotNull(type);
        Assert.False(string.IsNullOrWhiteSpace(expected));

        var staticTypeNamesClass = typeof(TypeNames<>).MakeGenericType(type);
        var property = staticTypeNamesClass.GetProperty(nameof(TypeNames<object>.Value));
        Assert.NotNull(property);

        var actualValue = property.GetValue(obj: null);
        Assert.Equal(expected, actualValue);
    }

    public static IEnumerable<object[]> GetTestData()
    {
        yield return new object[] { typeof(Person), "Person" };
        yield return new object[] { typeof(Student), "Student" };
        yield return new object[] { typeof(Task<int>), "Task<Int32>" };
        yield return new object[] { typeof(List<Task<Person>>), "List<Task<Person>>" };
    }

    public static IEnumerable<object[]> GetOpenGenericsTestData()
    {
        yield return new object[] { typeof(Task<>), "Task<TResult>" };
        yield return new object[] { typeof(List<>), "List<T>" };
    }
}