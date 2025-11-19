using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orchid_backend_net.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change_Sample_Status_type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<int>(
            //    name: "Status",
            //    table: "Samples",
            //    type: "integer",
            //    nullable: false,
            //    oldClrType: typeof(bool),
            //    oldType: "boolean");
            migrationBuilder.Sql("ALTER TABLE \"Samples\" ALTER COLUMN \"Status\" TYPE integer USING \"Status\"::integer;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<bool>(
            //    name: "Status",
            //    table: "Samples",
            //    type: "boolean",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "integer");
            migrationBuilder.Sql("ALTER TABLE \"Samples\" ALTER COLUMN \"Status\" TYPE boolean USING CASE WHEN \"Status\" = 0 THEN false ELSE true END;");
        }
    }
}
