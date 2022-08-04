using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using MyBoards_Entity.Dto;
using MyBoards_Entity.Entities;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});


builder.Services.AddDbContext<MyBoardsContext>(
        option=>option
        //.UseLazyLoadingProxies() Shouldn't really use it- more problems than advantages
        .UseSqlServer(builder.Configuration.GetConnectionString("MyBoardsConnectionString"))
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<MyBoardsContext>();

var pendingMigrations = dbContext.Database.GetPendingMigrations();
if(pendingMigrations.Any())
{
    dbContext.Database.Migrate();
}
var users = dbContext.Users.ToList();
if(!users.Any())
{
    var user1 = new User()
    {
        Email = "user1@test.com",
        FullName = "User one",
        Address = new Address()
        {
            City = "Gdansk",
            Street = "Srodmiejska"
        }
    };

    var user2 = new User()
    {
        Email = "user2@test.com",
        FullName = "User two",
        Address = new Address()
        {
            City = "Gdynia",
            Street = "Staromiejska"
        }
    };
    dbContext.Users.AddRange(user1,user2);
    dbContext.SaveChanges();
}
var tag = dbContext.Tags.ToList();
if(!tag.Any())
{
    var tag1 = new Tag()
    {
        Value = "Web Api",
        Category = "Web"
    };
    var tag2 = new Tag()
    {
        Value = "UI application",
        Category = "Ui"
    };
    var tag3 = new Tag()
    {
        Value = "Desktop application",
        Category = "Desktop"
    };
    var tag4 = new Tag()
    {
        Value = "Rest Api",
        Category = "Api"
    };
    var tag5 = new Tag()
    {
        Value = "Web Service",
        Category = "Service"
    };
    dbContext.Tags.AddRange(tag1, tag2, tag3, tag4, tag5);
    dbContext.SaveChanges();
}

app.MapGet("pagination", async (MyBoardsContext db) =>
{
    //user input
    string filter = "a";
    string sortBy = "FullName";
    bool sortByDescending = false;
    int pageNumber = 1;
    int pageSize = 10;
    //

    var query = db.Users
    .Where(u => filter == null 
    || (u.Email.ToLower().Contains(filter.ToLower())
    || u.FullName.ToLower().Contains(filter.ToLower())));

    var totalCount = query.Count();

    if(sortBy != null)
    {
        Dictionary<string, Expression<Func<User, object>>> columnsSelector = new Dictionary<string, Expression<Func<User, object>>>
        {
            {nameof(User.Email), user=>user.Email },
            {nameof(User.FullName), user=>user.FullName }
        };
        var sortByExpression = columnsSelector[sortBy];
        
        query = sortByDescending
        ? query.OrderByDescending(sortByExpression)
        : query.OrderBy(sortByExpression);
    }
    var result = query.Skip(pageSize * (pageNumber - 1))
    .Take(pageSize)
    .ToList();

    var pagedResult = new PagedResult<User>(result, totalCount, pageSize, pageNumber);

    return pagedResult;

});

app.MapGet("data", async (MyBoardsContext db) =>
{
    {
        //List<Tag> tags = db.Tags.ToList();
        //Epic epic = db.Epics.First();
        //User user = db.Users.FirstOrDefault(u => u.FullName == "User One");
        //return new { epic, user };
        //return tags;
        //var toDoWorkItems = db.WorkItems.Where(w=>w.StateId==1).ToList();
        //return new { toDoWorkItems };

        //Async : 
        //var newComments = await db
        //    .Comments
        //    .Where(c => c.CreatedDate > new DateTime(2022, 7, 23))
        //    .ToListAsync();
        //return newComments;

        //var top5NewestComments = await db
        //    .Comments
        //    .OrderByDescending(c => c.CreatedDate)
        //    .Take(5)
        //    .ToListAsync();
        //return top5NewestComments;

        //var statesCount = await db
        //    .WorkItems
        //    .GroupBy(x => x.StateId)
        //    .Select(g => new { stateId = g.Key, count = g.Count() })
        //    .ToListAsync();
        //return statesCount;

        //var orderedEpics = await db
        //    .WorkItems
        //    .Where(x=>x.StateId==4)
        //    .OrderBy(x => x.Priority)
        //    .ToListAsync();

        //return orderedEpics;


        //var userInformation = await db
        //.Comments
        //.GroupBy(c => c.AuthorId)
        //.Select(g => new { g.Key, Count = g.Count() })
        //.ToListAsync();

        //var topAuthor = userInformation.FirstOrDefault(a => a.Count == userInformation.Max(acc => acc.Count));

        //User userDetails = db.Users.First(u => u.Id == topAuthor.Key);

        //return new { userDetails, commentCount = topAuthor.Count };
    }
    //var user = await db.Users
    //.Include(u=>u.Comments).ThenInclude(c=>c.WorkItem)
    //.Include(u=>u.Address)
    //.FirstAsync(u => u.Id == Guid.Parse("68366DBE-0809-490F-CC1D-08DA10AB0E61"));

    //string minWorkItemsCount = "85";
    //var states = db.WorkItemsStates
    //.FromSqlInterpolated($@"SELECT wis.Id, wis.Value FROM WorkItemsStates wis JOIN WorkItems wi on wi.StateId = wis.Id 
    //              GROUP BY wis.Id, wis.Value HAVING COUNT(*) > {minWorkItemsCount}")
    //.AsNoTracking()
    //.ToList();

    //db.Database.ExecuteSqlRaw(@"Update");
    //var entries = db.ChangeTracker.Entries();

    var usersComments = await db.Users
                .Include(u => u.Address)
                .Include(u=>u.Comments)
                .Where(u => u.Address.Country == "Albania")
                .SelectMany(u=>u.Comments)
                .Select(c=>c.Message)
                .ToListAsync();

    return usersComments;
});
app.MapPost("update", async (MyBoardsContext db) =>
{
    Epic epic =await db.Epics.FirstAsync(epic => epic.Id == 1);

    var rejectedState = await db.WorkItemsStates.FirstAsync(a => a.Value == "Rejected");
    epic.State = rejectedState;
    await db.SaveChangesAsync();

    return epic;
});

app.MapPost("create", async (MyBoardsContext db) =>
{
    //Tag tag = new Tag()
    //{
    //    Value="EF"
    //};
    //Tag tag1 = new Tag()
    //{
    //    Value = "MVC"
    //};
    //Tag tag2 = new Tag()
    //{
    //    Value = "ASP"
    //};
    //List<Tag> tags = new List<Tag>() { tag,tag1, tag2 };
    //await db.Tags.AddRangeAsync(tags);

    Address address = new Address()
    {
        Id=Guid.Parse("9A8E164A-F3C2-40C3-CBCD-08DA10AB0E61"),
        City="Gdansk",
        Country="Polska",
        Street="D³uga"
    };

    User user = new User()
    {
        Email = "user@test.com",
        FullName = "Test User",
        Address = address
    };
    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
    return tag;

});

app.MapDelete("delete", async (MyBoardsContext db) =>
{
    //var workItemTags = await db.WorkItemTag.Where(c => c.WorkItemId == 12).ToListAsync();
    //db.RemoveRange(workItemTags);

    //var workItem = await db.WorkItems.FirstAsync(c => c.Id == 16);
    //db.RemoveRange(workItem);
    //await db.SaveChangesAsync();

    var user = await db.Users
    .Include(u=>u.Comments)
    .FirstAsync(u => u.Id == Guid.Parse("1EFA050D-FD55-46ED-CC18-08DA10AB0E61"));

    db.Users.Remove(user);

    await db.SaveChangesAsync();
});

app.Run();
