using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiCampus.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabase : Migration
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
                name: "careers",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_careers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notification_types",
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
                    table.PrimaryKey("PK_notification_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "publications_types",
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
                    table.PrimaryKey("PK_publications_types", x => x.id);
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
                name: "subjects",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subjects_movements",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    movement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects_movements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sec_users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    no_account = table.Column<int>(type: "int", nullable: false),
                    id_campus = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                        name: "FK_sec_users_campuses_id_campus",
                        column: x => x.id_campus,
                        principalTable: "campuses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "campuses_careers",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    id_campus = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    id_career = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campuses_careers", x => x.id);
                    table.ForeignKey(
                        name: "FK_campuses_careers_campuses_id_campus",
                        column: x => x.id_campus,
                        principalTable: "campuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_campuses_careers_careers_id_career",
                        column: x => x.id_career,
                        principalTable: "careers",
                        principalColumn: "id");
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
                name: "careers_subjects",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    id_career = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    id_clase = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_careers_subjects", x => x.id);
                    table.ForeignKey(
                        name: "FK_careers_subjects_careers_id_career",
                        column: x => x.id_career,
                        principalTable: "careers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_careers_subjects_subjects_id_clase",
                        column: x => x.id_clase,
                        principalTable: "subjects",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "chats",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    id_emisor = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    id_receptor = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_send = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_modify = table.Column<DateTime>(type: "datetime2", nullable: false),
                    received = table.Column<bool>(type: "bit", nullable: false),
                    seen = table.Column<bool>(type: "bit", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chats", x => x.id);
                    table.ForeignKey(
                        name: "FK_chats_sec_users_id_emisor",
                        column: x => x.id_emisor,
                        principalTable: "sec_users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_chats_sec_users_id_receptor",
                        column: x => x.id_receptor,
                        principalTable: "sec_users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    id_user = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    id_type = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seen = table.Column<bool>(type: "bit", nullable: false),
                    date_creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_modify = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_notifications_notification_types_id_type",
                        column: x => x.id_type,
                        principalTable: "notification_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notifications_sec_users_id_user",
                        column: x => x.id_user,
                        principalTable: "sec_users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "publications",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    id_user = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    id_type = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_modify = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publications", x => x.id);
                    table.ForeignKey(
                        name: "FK_publications_publications_types_id_type",
                        column: x => x.id_type,
                        principalTable: "publications_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_publications_sec_users_id_user",
                        column: x => x.id_user,
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
                name: "subject_users",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    id_subject = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    id_users = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    id_movement = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subject_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_subject_users_sec_users_id_users",
                        column: x => x.id_users,
                        principalTable: "sec_users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_subject_users_subjects_id_subject",
                        column: x => x.id_subject,
                        principalTable: "subjects",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_subject_users_subjects_movements_id_movement",
                        column: x => x.id_movement,
                        principalTable: "subjects_movements",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_campuses_careers_id_campus",
                table: "campuses_careers",
                column: "id_campus");

            migrationBuilder.CreateIndex(
                name: "IX_campuses_careers_id_career",
                table: "campuses_careers",
                column: "id_career");

            migrationBuilder.CreateIndex(
                name: "IX_careers_subjects_id_career",
                table: "careers_subjects",
                column: "id_career");

            migrationBuilder.CreateIndex(
                name: "IX_careers_subjects_id_clase",
                table: "careers_subjects",
                column: "id_clase");

            migrationBuilder.CreateIndex(
                name: "IX_chats_id_emisor",
                table: "chats",
                column: "id_emisor");

            migrationBuilder.CreateIndex(
                name: "IX_chats_id_receptor",
                table: "chats",
                column: "id_receptor");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_id_type",
                table: "notifications",
                column: "id_type");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_id_user",
                table: "notifications",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_publications_id_type",
                table: "publications",
                column: "id_type");

            migrationBuilder.CreateIndex(
                name: "IX_publications_id_user",
                table: "publications",
                column: "id_user");

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
                name: "IX_sec_users_id_campus",
                table: "sec_users",
                column: "id_campus");

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
                name: "IX_subject_users_id_movement",
                table: "subject_users",
                column: "id_movement");

            migrationBuilder.CreateIndex(
                name: "IX_subject_users_id_subject",
                table: "subject_users",
                column: "id_subject");

            migrationBuilder.CreateIndex(
                name: "IX_subject_users_id_users",
                table: "subject_users",
                column: "id_users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "campuses_careers");

            migrationBuilder.DropTable(
                name: "careers_subjects");

            migrationBuilder.DropTable(
                name: "chats");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "publications");

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
                name: "subject_users");

            migrationBuilder.DropTable(
                name: "careers");

            migrationBuilder.DropTable(
                name: "notification_types");

            migrationBuilder.DropTable(
                name: "publications_types");

            migrationBuilder.DropTable(
                name: "sec_roles");

            migrationBuilder.DropTable(
                name: "sec_users");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "subjects_movements");

            migrationBuilder.DropTable(
                name: "campuses");
        }
    }
}
