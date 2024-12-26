using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYPTourneyPro.Migrations
{
    /// <inheritdoc />
    public partial class changeChatRoomSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "ChatRooms");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "ChatRooms");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "ChatRooms");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "ChatRooms");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ChatRooms");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "ChatRoomParticipants");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "ChatRoomParticipants");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ChatRoomParticipants");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ChatMessages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "ChatRooms",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "ChatRooms",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "ChatRooms",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "ChatRooms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ChatRooms",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "ChatRoomParticipants",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "ChatRoomParticipants",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ChatRoomParticipants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "ChatMessages",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "ChatMessages",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "ChatMessages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "ChatMessages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ChatMessages",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
