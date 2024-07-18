using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Services;
using Xunit;

namespace Todo.Tests.Services;

public class ApplicationDbContextConvenienceTests
{
    private readonly Fixture _fixture;
    private readonly DbContextOptions<ApplicationDbContext> _options;
    private readonly IdentityUser _user;
    private readonly string _userId;

    public ApplicationDbContextConvenienceTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;
        _fixture = new Fixture();

        _userId = _fixture.Create<string>();
        _user = new IdentityUser { Id = _userId, UserName = _fixture.Create<string>() };
    }

    [Fact]
    public void RelevantTodoLists_ReturnsCorrectTodoListsForUser()
    {
        using var context = new ApplicationDbContext(_options);
        context.Users.Add(_user);
        var expectedListName1 = _fixture.Create<string>();
        var expectedListName2 = _fixture.Create<string>();
        
        var todoList1 = new TestTodoListBuilder(_user, expectedListName1)
            .WithItem(_fixture.Create<string>(), Importance.High)
            .Build();
        context.TodoLists.Add(todoList1);

        var todoList2 = new TestTodoListBuilder(new IdentityUser(_fixture.Create<string>()), expectedListName2)
            .WithItem(_fixture.Create<string>(), Importance.High)
            .Build();
        context.TodoLists.Add(todoList2);
        context.SaveChanges();

        var result = context.RelevantTodoLists(_userId).ToList();

        using (new AssertionScope())
        {
            result.Should().HaveCount(1);
            result[0].Title.Should().Be(expectedListName1);
        }
    }

    [Fact]
    public async Task RelevantTodoLists_NoTodoListsForUser_ReturnsEmpty()
    {
        await using var context = new ApplicationDbContext(_options);
        context.Users.Add(_user);
        context.SaveChanges();

        var result = () => Task.FromResult(context.RelevantTodoLists(_userId).ToList());

        using (new AssertionScope())
        {
            (await result.Should().NotThrowAsync()).Which.Should().BeEmpty();
        }
    }

    [Fact]
    public void RelevantTodoLists_ReturnsItemsInCorrectOrder()
    {
        using var context = new ApplicationDbContext(_options);
        context.Users.Add(_user);
        var expectedListName1 = _fixture.Create<string>();
        var todoList1 = new TestTodoListBuilder(_user, expectedListName1)
            .WithItem(_fixture.Create<string>(), Importance.Medium)
            .WithItem(_fixture.Create<string>(), Importance.Low)
            .WithItem(_fixture.Create<string>(), Importance.High)
            .Build();
        context.TodoLists.Add(todoList1);
        context.SaveChanges();

        var result = context.RelevantTodoLists(_userId).ToList();

        using (new AssertionScope())
        {
            result.Should().HaveCount(1);
            result[0].Items.Should().HaveCount(3);
            result[0].Items.First().Importance.Should().Be(Importance.High);
            result[0].Items.ElementAt(1).Importance.Should().Be(Importance.Medium);
            result[0].Items.Last().Importance.Should().Be(Importance.Low);
        }
    }


    [Fact]
    public void RelevantTodoLists_UserAssignedToItemInOtherList_ResultIncludesOtherList()
    {
        using var context = new ApplicationDbContext(_options);
        context.Users.Add(_user);
        var expectedListName1 = _fixture.Create<string>();
        var expectedListName2 = _fixture.Create<string>();
        var otherUser = new IdentityUser { Id = _fixture.Create<string>(), UserName = _fixture.Create<string>() };
        var todoList1 = new TestTodoListBuilder(_user, expectedListName1)
            .WithItem(_fixture.Create<string>(), Importance.High)
            .Build();
        context.TodoLists.Add(todoList1);
        
        var todoList2 = new TestTodoListBuilder(otherUser, expectedListName2)
            .WithItem(_fixture.Create<string>(), Importance.High, _userId)
            .WithItem(_fixture.Create<string>(), Importance.High)
            .Build();
        context.TodoLists.Add(todoList2);
        context.SaveChanges();

        var result = context.RelevantTodoLists(_userId).ToList();

        using (new AssertionScope())
        {
            result.Should().HaveCount(2);
            result.Where(r => r.Owner.Id == _userId).Should().HaveCount(1);
            result.Where(r => r.Owner.Id == otherUser.Id).Should().HaveCount(1);
            var otherList = result.Single(r => r.Owner.Id == otherUser.Id);
            otherList.Items.Where(i => i.ResponsiblePartyId == _userId).Should().HaveCount(1);
        }
    }
}