﻿IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ContactUs] (
    [Id] uniqueidentifier NOT NULL,
    [FullName] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [Message] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_ContactUs] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [NewsSubscription] (
    [Id] uniqueidentifier NOT NULL,
    [Email] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_NewsSubscription] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Schools] (
    [Id] uniqueidentifier NOT NULL,
    [School] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Schools] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [FullName] nvarchar(50) NULL,
    [TweeterUrl] nvarchar(250) NULL,
    [LinkedInUrl] nvarchar(250) NULL,
    [InstagramUrl] nvarchar(250) NULL,
    [FacebookUrl] nvarchar(250) NULL,
    [IsDeleted] bit NULL,
    [Role] int NULL,
    [SecurityQuestion] nvarchar(max) NULL,
    [SecurityAnswer] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [Title] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [NumbersOfStudents] int NULL,
    [YearStartExperience] datetime2 NULL,
    [SchoolId] uniqueidentifier NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUsers_Schools_SchoolId] FOREIGN KEY ([SchoolId]) REFERENCES [Schools] ([Id])
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Coaches] (
    [Id] uniqueidentifier NOT NULL,
    [FullName] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [TweeterUrl] nvarchar(max) NULL,
    [LinkedInUrl] nvarchar(max) NULL,
    [InstagramUrl] nvarchar(max) NULL,
    [FacebookUrl] nvarchar(max) NULL,
    [IdentityUserId] uniqueidentifier NOT NULL,
    [IdentityUserId1] nvarchar(450) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Coaches] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Coaches_AspNetUsers_IdentityUserId1] FOREIGN KEY ([IdentityUserId1]) REFERENCES [AspNetUsers] ([Id])
);
GO

CREATE TABLE [Courses] (
    [Id] uniqueidentifier NOT NULL,
    [CourseTitle] nvarchar(max) NULL,
    [CourseIntro] nvarchar(max) NULL,
    [CoachId] nvarchar(450) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Courses_AspNetUsers_CoachId] FOREIGN KEY ([CoachId]) REFERENCES [AspNetUsers] ([Id])
);
GO

CREATE TABLE [Subjects] (
    [Id] uniqueidentifier NOT NULL,
    [SubjectName] nvarchar(max) NULL,
    [CoachFrikaUsersId] nvarchar(450) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Subjects] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Subjects_AspNetUsers_CoachFrikaUsersId] FOREIGN KEY ([CoachFrikaUsersId]) REFERENCES [AspNetUsers] ([Id])
);
GO

CREATE TABLE [Teachers] (
    [Id] uniqueidentifier NOT NULL,
    [School] nvarchar(max) NULL,
    [NumbersOfStudents] int NOT NULL,
    [YearOfExperience] int NOT NULL,
    [IdentityUserId] uniqueidentifier NOT NULL,
    [IdentityUserId1] nvarchar(450) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Teachers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Teachers_AspNetUsers_IdentityUserId1] FOREIGN KEY ([IdentityUserId1]) REFERENCES [AspNetUsers] ([Id])
);
GO

CREATE TABLE [Batches] (
    [Id] uniqueidentifier NOT NULL,
    [Title] nvarchar(max) NULL,
    [TeachersId] uniqueidentifier NULL,
    [TeachersId1] nvarchar(450) NULL,
    [CourseId] uniqueidentifier NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Batches] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Batches_AspNetUsers_TeachersId1] FOREIGN KEY ([TeachersId1]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_Batches_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id])
);
GO

CREATE TABLE [Schedules] (
    [Id] uniqueidentifier NOT NULL,
    [Description] nvarchar(max) NULL,
    [Subject] nvarchar(max) NULL,
    [MaterialUrl] nvarchar(max) NULL,
    [Schedule] datetime2 NULL,
    [BatcheId] uniqueidentifier NULL,
    [CourseId] uniqueidentifier NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Schedules] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Schedules_Batches_BatcheId] FOREIGN KEY ([BatcheId]) REFERENCES [Batches] ([Id]),
    CONSTRAINT [FK_Schedules_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id])
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE INDEX [IX_AspNetUsers_SchoolId] ON [AspNetUsers] ([SchoolId]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

CREATE INDEX [IX_Batches_CourseId] ON [Batches] ([CourseId]);
GO

CREATE INDEX [IX_Batches_TeachersId1] ON [Batches] ([TeachersId1]);
GO

CREATE INDEX [IX_Coaches_IdentityUserId1] ON [Coaches] ([IdentityUserId1]);
GO

CREATE INDEX [IX_Courses_CoachId] ON [Courses] ([CoachId]);
GO

CREATE INDEX [IX_Schedules_BatcheId] ON [Schedules] ([BatcheId]);
GO

CREATE INDEX [IX_Schedules_CourseId] ON [Schedules] ([CourseId]);
GO

CREATE INDEX [IX_Subjects_CoachFrikaUsersId] ON [Subjects] ([CoachFrikaUsersId]);
GO

CREATE INDEX [IX_Teachers_IdentityUserId1] ON [Teachers] ([IdentityUserId1]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241117221001_init', N'6.0.0');
GO

COMMIT;
GO

