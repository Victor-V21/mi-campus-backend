using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiCampus.Migrations
{
    /// <inheritdoc />
    public partial class AddTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "campuses",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    careers_ids = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sec_roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sec_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "university_careers",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_university_careers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sec_roles_claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sec_roles_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sec_roles_claims_sec_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "sec_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampusEntityUniversityCareerEntity",
                columns: table => new
                {
                    CareersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CareersIds = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampusEntityUniversityCareerEntity", x => new { x.CareersId, x.CareersIds });
                    table.ForeignKey(
                        name: "FK_CampusEntityUniversityCareerEntity_campuses_CareersIds",
                        column: x => x.CareersIds,
                        principalTable: "campuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampusEntityUniversityCareerEntity_university_careers_CareersId",
                        column: x => x.CareersId,
                        principalTable: "university_careers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sec_users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    account_number = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    avatar_url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    birth_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    registration_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    career_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sec_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sec_users_university_careers_career_id",
                        column: x => x.career_id,
                        principalTable: "university_careers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    career_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    prerequisite_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.id);
                    table.ForeignKey(
                        name: "FK_subjects_subjects_prerequisite_id",
                        column: x => x.prerequisite_id,
                        principalTable: "subjects",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_subjects_university_careers_career_id",
                        column: x => x.career_id,
                        principalTable: "university_careers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "contents",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    content_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    event_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    event_time = table.Column<TimeSpan>(type: "time", nullable: true),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    map_location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cover_image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    event_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_moderated = table.Column<bool>(type: "bit", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contents", x => x.id);
                    table.ForeignKey(
                        name: "FK_contents_sec_users_user_id",
                        column: x => x.user_id,
                        principalTable: "sec_users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "sec_users_claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sec_users_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sec_users_claims_sec_users_UserId",
                        column: x => x.UserId,
                        principalTable: "sec_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sec_users_logins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sec_users_logins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_sec_users_logins_sec_users_UserId",
                        column: x => x.UserId,
                        principalTable: "sec_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sec_users_roles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sec_users_roles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_sec_users_roles_sec_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "sec_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sec_users_roles_sec_users_UserId",
                        column: x => x.UserId,
                        principalTable: "sec_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sec_users_tokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sec_users_tokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_sec_users_tokens_sec_users_UserId",
                        column: x => x.UserId,
                        principalTable: "sec_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subjects_taken",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    period = table.Column<int>(type: "int", nullable: false),
                    grade = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    subject_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects_taken", x => x.id);
                    table.ForeignKey(
                        name: "FK_subjects_taken_sec_users_user_id",
                        column: x => x.user_id,
                        principalTable: "sec_users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_subjects_taken_subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "content_images",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    content_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_content_images", x => x.id);
                    table.ForeignKey(
                        name: "FK_content_images_contents_content_id",
                        column: x => x.content_id,
                        principalTable: "contents",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "contents_feedbacks",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rating = table.Column<int>(type: "int", nullable: true),
                    content_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contents_feedbacks", x => x.id);
                    table.ForeignKey(
                        name: "FK_contents_feedbacks_contents_content_id",
                        column: x => x.content_id,
                        principalTable: "contents",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_contents_feedbacks_sec_users_user_id",
                        column: x => x.user_id,
                        principalTable: "sec_users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampusEntityUniversityCareerEntity_CareersIds",
                table: "CampusEntityUniversityCareerEntity",
                column: "CareersIds");

            migrationBuilder.CreateIndex(
                name: "IX_content_images_content_id",
                table: "content_images",
                column: "content_id");

            migrationBuilder.CreateIndex(
                name: "IX_contents_user_id",
                table: "contents",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_contents_feedbacks_content_id",
                table: "contents_feedbacks",
                column: "content_id");

            migrationBuilder.CreateIndex(
                name: "IX_contents_feedbacks_user_id",
                table: "contents_feedbacks",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "sec_roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_sec_roles_claims_RoleId",
                table: "sec_roles_claims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "sec_users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_sec_users_career_id",
                table: "sec_users",
                column: "career_id");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "sec_users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_sec_users_claims_UserId",
                table: "sec_users_claims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_sec_users_logins_UserId",
                table: "sec_users_logins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_sec_users_roles_RoleId",
                table: "sec_users_roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_subjects_career_id",
                table: "subjects",
                column: "career_id");

            migrationBuilder.CreateIndex(
                name: "IX_subjects_prerequisite_id",
                table: "subjects",
                column: "prerequisite_id");

            migrationBuilder.CreateIndex(
                name: "IX_subjects_taken_subject_id",
                table: "subjects_taken",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_subjects_taken_user_id",
                table: "subjects_taken",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampusEntityUniversityCareerEntity");

            migrationBuilder.DropTable(
                name: "content_images");

            migrationBuilder.DropTable(
                name: "contents_feedbacks");

            migrationBuilder.DropTable(
                name: "sec_roles_claims");

            migrationBuilder.DropTable(
                name: "sec_users_claims");

            migrationBuilder.DropTable(
                name: "sec_users_logins");

            migrationBuilder.DropTable(
                name: "sec_users_roles");

            migrationBuilder.DropTable(
                name: "sec_users_tokens");

            migrationBuilder.DropTable(
                name: "subjects_taken");

            migrationBuilder.DropTable(
                name: "campuses");

            migrationBuilder.DropTable(
                name: "contents");

            migrationBuilder.DropTable(
                name: "sec_roles");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "sec_users");

            migrationBuilder.DropTable(
                name: "university_careers");
        }
    }
}
