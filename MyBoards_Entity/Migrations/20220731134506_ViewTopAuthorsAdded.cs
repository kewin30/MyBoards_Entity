using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBoards_Entity.Migrations
{
    public partial class ViewTopAuthorsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE VIEW View_TopAuthors AS
SELECT TOP 5 u.FullName, COUNT(*) as [WorkItemsCreated]
From Users u 
JOIN WorkItems wi on wi.AuthorId = u.Id
GROUP BY u.id, u.FullName
ORDER BY [WorkItemsCreated] DESC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW View_TopAuthors");
        }
    }
}
