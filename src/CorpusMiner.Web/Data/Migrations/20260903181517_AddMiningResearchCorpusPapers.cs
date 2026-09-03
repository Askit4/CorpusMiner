using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorpusMiner.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMiningResearchCorpusPapers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MiningResearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiningResearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiningResearches_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Corpora",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MiningResearchId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corpora", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Corpora_MiningResearches_MiningResearchId",
                        column: x => x.MiningResearchId,
                        principalTable: "MiningResearches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CorpusSourceFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorpusId = table.Column<int>(type: "int", nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceType = table.Column<int>(type: "int", nullable: false),
                    BlobPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UploadedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RecordCount = table.Column<int>(type: "int", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorpusSourceFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorpusSourceFiles_AspNetUsers_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorpusSourceFiles_Corpora_CorpusId",
                        column: x => x.CorpusId,
                        principalTable: "Corpora",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Papers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorpusId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Doi = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    SourceTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Abstract = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RawMetadataJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Papers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Papers_Corpora_CorpusId",
                        column: x => x.CorpusId,
                        principalTable: "Corpora",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaperSourceRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaperId = table.Column<int>(type: "int", nullable: false),
                    CorpusSourceFileId = table.Column<int>(type: "int", nullable: false),
                    RawFieldsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaperSourceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaperSourceRecords_CorpusSourceFiles_CorpusSourceFileId",
                        column: x => x.CorpusSourceFileId,
                        principalTable: "CorpusSourceFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaperSourceRecords_Papers_PaperId",
                        column: x => x.PaperId,
                        principalTable: "Papers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Corpora_MiningResearchId",
                table: "Corpora",
                column: "MiningResearchId");

            migrationBuilder.CreateIndex(
                name: "IX_CorpusSourceFiles_CorpusId",
                table: "CorpusSourceFiles",
                column: "CorpusId");

            migrationBuilder.CreateIndex(
                name: "IX_CorpusSourceFiles_UploadedByUserId",
                table: "CorpusSourceFiles",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MiningResearches_CreatedByUserId",
                table: "MiningResearches",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Papers_CorpusId_Doi",
                table: "Papers",
                columns: new[] { "CorpusId", "Doi" },
                unique: true,
                filter: "[Doi] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Papers_CorpusId_Title_Year",
                table: "Papers",
                columns: new[] { "CorpusId", "Title", "Year" });

            migrationBuilder.CreateIndex(
                name: "IX_PaperSourceRecords_CorpusSourceFileId",
                table: "PaperSourceRecords",
                column: "CorpusSourceFileId");

            migrationBuilder.CreateIndex(
                name: "IX_PaperSourceRecords_PaperId",
                table: "PaperSourceRecords",
                column: "PaperId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaperSourceRecords");

            migrationBuilder.DropTable(
                name: "CorpusSourceFiles");

            migrationBuilder.DropTable(
                name: "Papers");

            migrationBuilder.DropTable(
                name: "Corpora");

            migrationBuilder.DropTable(
                name: "MiningResearches");
        }
    }
}
