using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBoards_Entity.Migrations
{
    public partial class WorkItemStateSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "WorkItemsStates",
                newName: "Value");

            migrationBuilder.InsertData(
                table: "WorkItemsStates",
                columns: new[] { "Id", "Value" },
                values: new object[] { 1, "To do" });

            migrationBuilder.InsertData(
                table: "WorkItemsStates",
                columns: new[] { "Id", "Value" },
                values: new object[] { 2, "Doing" });

            migrationBuilder.InsertData(
                table: "WorkItemsStates",
                columns: new[] { "Id", "Value" },
                values: new object[] { 3, "Done" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkItemsStates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WorkItemsStates",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WorkItemsStates",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "WorkItemsStates",
                newName: "State");
        }
    }
}
