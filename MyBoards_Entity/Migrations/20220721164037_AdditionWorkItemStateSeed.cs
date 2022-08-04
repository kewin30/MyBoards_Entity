using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBoards_Entity.Migrations
{
    public partial class AdditionWorkItemStateSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(table: "WorkItemsStates",
                column: "Value",
                value: "On Hold");
            migrationBuilder.InsertData(table: "WorkItemsStates",
                column: "Value",
                value: "Rejected");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkItemsStates",
                keyColumn: "Value",
                keyValue: "On Hold");
            migrationBuilder.DeleteData(
               table: "WorkItemsStates",
               keyColumn: "Value",
               keyValue: "Rejected");
        }
    }
}
