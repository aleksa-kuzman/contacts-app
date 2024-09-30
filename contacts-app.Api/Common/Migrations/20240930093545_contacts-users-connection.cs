using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace contacts_app.Common.Migrations
{
    /// <inheritdoc />
    public partial class contactsusersconnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                schema: "contacts",
                table: "contacts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_contacts_user_id",
                schema: "contacts",
                table: "contacts",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_contacts_users_user_id",
                schema: "contacts",
                table: "contacts",
                column: "user_id",
                principalSchema: "contacts",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_contacts_users_user_id",
                schema: "contacts",
                table: "contacts");

            migrationBuilder.DropIndex(
                name: "ix_contacts_user_id",
                schema: "contacts",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "contacts",
                table: "contacts");
        }
    }
}
