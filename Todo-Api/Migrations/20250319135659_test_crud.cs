using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo_Api.Migrations
{
    /// <inheritdoc />
    public partial class test_crud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskDependencies_Tasks_DependsOnTaskId",
                table: "TaskDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskDependencies_Tasks_TaskId",
                table: "TaskDependencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskDependencies",
                table: "TaskDependencies");

            migrationBuilder.DropIndex(
                name: "IX_TaskDependencies_TaskId",
                table: "TaskDependencies");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TaskDependencies");

            migrationBuilder.RenameColumn(
                name: "DependsOnTaskId",
                table: "TaskDependencies",
                newName: "DependencyId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskDependencies_DependsOnTaskId",
                table: "TaskDependencies",
                newName: "IX_TaskDependencies_DependencyId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Tasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskDependencies",
                table: "TaskDependencies",
                columns: new[] { "TaskId", "DependencyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskDependencies_Tasks_DependencyId",
                table: "TaskDependencies",
                column: "DependencyId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskDependencies_Tasks_TaskId",
                table: "TaskDependencies",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskDependencies_Tasks_DependencyId",
                table: "TaskDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskDependencies_Tasks_TaskId",
                table: "TaskDependencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskDependencies",
                table: "TaskDependencies");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "DependencyId",
                table: "TaskDependencies",
                newName: "DependsOnTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskDependencies_DependencyId",
                table: "TaskDependencies",
                newName: "IX_TaskDependencies_DependsOnTaskId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "Tasks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TaskDependencies",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskDependencies",
                table: "TaskDependencies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TaskDependencies_TaskId",
                table: "TaskDependencies",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskDependencies_Tasks_DependsOnTaskId",
                table: "TaskDependencies",
                column: "DependsOnTaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskDependencies_Tasks_TaskId",
                table: "TaskDependencies",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
