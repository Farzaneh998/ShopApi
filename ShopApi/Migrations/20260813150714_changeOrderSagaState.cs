using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Migrations
{
    /// <inheritdoc />
    public partial class changeOrderSagaState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderSagaStates",
                table: "OrderSagaStates");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderSagaStates");

            migrationBuilder.AlterColumn<string>(
                name: "CurrentState",
                table: "OrderSagaStates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderSagaStates",
                table: "OrderSagaStates",
                column: "CorrelationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderSagaStates",
                table: "OrderSagaStates");

            migrationBuilder.AlterColumn<string>(
                name: "CurrentState",
                table: "OrderSagaStates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrderSagaStates",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderSagaStates",
                table: "OrderSagaStates",
                column: "Id");
        }
    }
}
