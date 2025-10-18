using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    PasswordResetCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordResetExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillsType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    TypeName = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillsType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
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
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
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
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companys",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    About = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: true),
                    Image = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companys", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Companys_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID1 = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    UserID2 = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    CreatedAT = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Conversations_AspNetUsers_UserID1",
                        column: x => x.UserID1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversations_AspNetUsers_UserID2",
                        column: x => x.UserID2,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    secoundName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    AboutYou = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    UniverCity = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    Image = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    Content = table.Column<string>(type: "NVARCHAR(600)", maxLength: 600, nullable: true),
                    Image = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "smalldatetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Followers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyID = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    FollowerID = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Followers_AspNetUsers_FollowerID",
                        column: x => x.FollowerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Followers_Companys_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companys",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyID = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    Content = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    SkillsTypeID = table.Column<int>(type: "INT", nullable: true),
                    FullTimeORPartTime = table.Column<bool>(type: "BIT", nullable: false),
                    RemoteOROnSite = table.Column<bool>(type: "BIT", nullable: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "smalldatetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Jobs_Companys_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companys",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobs_SkillsType_SkillsTypeID",
                        column: x => x.SkillsTypeID,
                        principalTable: "SkillsType",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConversationId = table.Column<int>(type: "INT", nullable: false),
                    SenderBy = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    MassageText = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false),
                    SendAT = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SenderBy",
                        column: x => x.SenderBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sender = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    Reseiver = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    Created = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    Status = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Connections_Employees_Reseiver",
                        column: x => x.Reseiver,
                        principalTable: "Employees",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Connections_Employees_Sender",
                        column: x => x.Sender,
                        principalTable: "Employees",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Experience",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    CompanyID = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: true),
                    CompanyName = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    Title = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false),
                    StartAT = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    FinishAT = table.Column<DateTime>(type: "smalldatetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experience", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Experience_Companys_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companys",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Experience_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    SkillTypeID = table.Column<int>(type: "INT", nullable: true),
                    SkillsTypeid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Skills_SkillsType_SkillsTypeid",
                        column: x => x.SkillsTypeid,
                        principalTable: "SkillsType",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    PostID = table.Column<int>(type: "INT", nullable: false),
                    Comment = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "smalldatetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostID",
                        column: x => x.PostID,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    PostID = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Likes_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Likes_Posts_PostID",
                        column: x => x.PostID,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplyJob",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: true),
                    JobID = table.Column<int>(type: "INT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyJob", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ApplyJob_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplyJob_Jobs_JobID",
                        column: x => x.JobID,
                        principalTable: "Jobs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplyExperience",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExperienceID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyExperience", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ApplyExperience_Experience_ExperienceID",
                        column: x => x.ExperienceID,
                        principalTable: "Experience",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplySkill",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillID = table.Column<int>(type: "INT", nullable: false),
                    UserID = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplySkill", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ApplySkill_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplySkill_Skills_SkillID",
                        column: x => x.SkillID,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LikesOnComments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    CommentID = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikesOnComments", x => x.id);
                    table.ForeignKey(
                        name: "FK_LikesOnComments_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LikesOnComments_Comments_CommentID",
                        column: x => x.CommentID,
                        principalTable: "Comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SkillsType",
                columns: new[] { "id", "TypeName" },
                values: new object[,]
                {
                    { 1, "C#" },
                    { 2, ".NET Core" },
                    { 3, "ASP.NET Core" },
                    { 4, "Entity Framework Core" },
                    { 5, "LINQ" },
                    { 6, "RESTful APIs" },
                    { 7, "Design Patterns" },
                    { 8, "SOLID Principles" },
                    { 9, "Dependency Injection" },
                    { 10, "SignalR" },
                    { 11, "Microservices" },
                    { 12, "Clean Architecture" },
                    { 13, "CQRS" },
                    { 14, "HTML" },
                    { 15, "CSS" },
                    { 16, "JavaScript" },
                    { 17, "TypeScript" },
                    { 18, "React" },
                    { 19, "Angular" },
                    { 20, "Vue.js" },
                    { 21, "Bootstrap" },
                    { 22, "Tailwind CSS" },
                    { 23, "Responsive Design" },
                    { 24, "UI/UX Design" },
                    { 25, "SQL Server" },
                    { 26, "MySQL" },
                    { 27, "PostgreSQL" },
                    { 28, "MongoDB" },
                    { 29, "Redis" },
                    { 30, "Database Design" },
                    { 31, "Query Optimization" },
                    { 32, "Data Analysis" },
                    { 33, "Git" },
                    { 34, "GitHub" },
                    { 35, "CI/CD" },
                    { 36, "Docker" },
                    { 37, "Kubernetes" },
                    { 38, "Azure" },
                    { 39, "AWS" },
                    { 40, "Linux" },
                    { 41, "Nginx" },
                    { 42, "IIS Deployment" }
                });

            migrationBuilder.InsertData(
                table: "SkillsType",
                columns: new[] { "id", "TypeName" },
                values: new object[,]
                {
                    { 43, "JWT" },
                    { 44, "OAuth 2.0" },
                    { 45, "Password Hashing" },
                    { 46, "HTTPS" },
                    { 47, "SQL Injection Prevention" },
                    { 48, "XSS Prevention" },
                    { 49, "Data Encryption" },
                    { 50, "Postman" },
                    { 51, "Swagger" },
                    { 52, "Visual Studio" },
                    { 53, "VS Code" },
                    { 54, "Figma" },
                    { 55, "Trello" },
                    { 56, "Jira" },
                    { 57, "Problem Solving" },
                    { 58, "Teamwork" },
                    { 59, "Communication" },
                    { 60, "Time Management" },
                    { 61, "Adaptability" },
                    { 62, "Critical Thinking" },
                    { 63, "Creativity" },
                    { 64, "Attention to Detail" },
                    { 65, "Leadership" },
                    { 66, "Work Under Pressure" },
                    { 67, "Continuous Learning" },
                    { 68, "Self-Motivation" },
                    { 69, "Decision Making" },
                    { 70, "Responsibility" },
                    { 71, "Project Management" },
                    { 72, "Business Analysis" },
                    { 73, "Strategic Planning" },
                    { 74, "Operations Management" },
                    { 75, "Team Leadership" },
                    { 76, "Customer Service" },
                    { 77, "Negotiation" },
                    { 78, "Sales" },
                    { 79, "Financial Management" },
                    { 80, "Digital Marketing" },
                    { 81, "Social Media Marketing" },
                    { 82, "Content Writing" },
                    { 83, "Copywriting" },
                    { 84, "SEO (Search Engine Optimization)" }
                });

            migrationBuilder.InsertData(
                table: "SkillsType",
                columns: new[] { "id", "TypeName" },
                values: new object[,]
                {
                    { 85, "Email Marketing" },
                    { 86, "Advertising Campaigns" },
                    { 87, "Brand Management" },
                    { 88, "Market Research" },
                    { 89, "Graphic Design" },
                    { 90, "Video Editing" },
                    { 91, "Photography" },
                    { 92, "Illustration" },
                    { 93, "Motion Graphics" },
                    { 94, "UI Design" },
                    { 95, "UX Design" },
                    { 96, "Canva" },
                    { 97, "Adobe Photoshop" },
                    { 98, "Adobe Illustrator" },
                    { 99, "Adobe Premiere Pro" },
                    { 100, "After Effects" },
                    { 101, "Public Speaking" },
                    { 102, "Networking" },
                    { 103, "Negotiation Skills" },
                    { 104, "Customer Relationship Management" },
                    { 105, "Conflict Resolution" },
                    { 106, "Presentation Skills" },
                    { 107, "Time Management" },
                    { 108, "Self-Discipline" },
                    { 109, "Adaptability" },
                    { 110, "Problem Solving" },
                    { 111, "Creativity" },
                    { 112, "Critical Thinking" },
                    { 113, "Decision Making" },
                    { 114, "Attention to Detail" },
                    { 115, "Work Under Pressure" },
                    { 116, "Translation" },
                    { 117, "Typing" },
                    { 118, "Microsoft Word" },
                    { 119, "Microsoft Excel" },
                    { 120, "Microsoft PowerPoint" },
                    { 121, "Google Workspace" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplyExperience_ExperienceID",
                table: "ApplyExperience",
                column: "ExperienceID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyJob_EmployeeID",
                table: "ApplyJob",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyJob_JobID",
                table: "ApplyJob",
                column: "JobID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplySkill_SkillID",
                table: "ApplySkill",
                column: "SkillID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplySkill_UserID",
                table: "ApplySkill",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostID",
                table: "Comments",
                column: "PostID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserID",
                table: "Comments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_Reseiver",
                table: "Connections",
                column: "Reseiver");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_Sender",
                table: "Connections",
                column: "Sender");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_UserID1",
                table: "Conversations",
                column: "UserID1");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_UserID2",
                table: "Conversations",
                column: "UserID2");

            migrationBuilder.CreateIndex(
                name: "IX_Experience_CompanyID",
                table: "Experience",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Experience_EmployeeID",
                table: "Experience",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_CompanyID",
                table: "Followers",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_FollowerID",
                table: "Followers",
                column: "FollowerID");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CompanyID",
                table: "Jobs",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_SkillsTypeID",
                table: "Jobs",
                column: "SkillsTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_PostID",
                table: "Likes",
                column: "PostID");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserID",
                table: "Likes",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_LikesOnComments_CommentID",
                table: "LikesOnComments",
                column: "CommentID");

            migrationBuilder.CreateIndex(
                name: "IX_LikesOnComments_UserID",
                table: "LikesOnComments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderBy",
                table: "Messages",
                column: "SenderBy");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserID",
                table: "Posts",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_EmployeeID",
                table: "Skills",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SkillsTypeid",
                table: "Skills",
                column: "SkillsTypeid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplyExperience");

            migrationBuilder.DropTable(
                name: "ApplyJob");

            migrationBuilder.DropTable(
                name: "ApplySkill");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "Followers");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "LikesOnComments");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Experience");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Companys");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "SkillsType");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
