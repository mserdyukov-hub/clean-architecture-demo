using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNamesInboxMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Topic",
                table: "InboxMessage",
                newName: "topic");

            migrationBuilder.RenameColumn(
                name: "Partition",
                table: "InboxMessage",
                newName: "partition");

            migrationBuilder.RenameColumn(
                name: "Offset",
                table: "InboxMessage",
                newName: "offset");

            migrationBuilder.RenameColumn(
                name: "Error",
                table: "InboxMessage",
                newName: "error");

            migrationBuilder.RenameColumn(
                name: "ReceivedOnUtc",
                table: "InboxMessage",
                newName: "received_on_utc");

            migrationBuilder.RenameColumn(
                name: "ProcessedOnUtc",
                table: "InboxMessage",
                newName: "processed_on_utc");

            migrationBuilder.RenameIndex(
                name: "IX_InboxMessage_Topic_Partition_Offset",
                table: "InboxMessage",
                newName: "IX_InboxMessage_topic_partition_offset");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "topic",
                table: "InboxMessage",
                newName: "Topic");

            migrationBuilder.RenameColumn(
                name: "partition",
                table: "InboxMessage",
                newName: "Partition");

            migrationBuilder.RenameColumn(
                name: "offset",
                table: "InboxMessage",
                newName: "Offset");

            migrationBuilder.RenameColumn(
                name: "error",
                table: "InboxMessage",
                newName: "Error");

            migrationBuilder.RenameColumn(
                name: "received_on_utc",
                table: "InboxMessage",
                newName: "ReceivedOnUtc");

            migrationBuilder.RenameColumn(
                name: "processed_on_utc",
                table: "InboxMessage",
                newName: "ProcessedOnUtc");

            migrationBuilder.RenameIndex(
                name: "IX_InboxMessage_topic_partition_offset",
                table: "InboxMessage",
                newName: "IX_InboxMessage_Topic_Partition_Offset");
        }
    }
}
