IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230617225201_Inicial')
BEGIN
    CREATE TABLE [Autores] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NULL,
        CONSTRAINT [PK_Autores] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230617225201_Inicial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230617225201_Inicial', N'7.0.7');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230620180204_Libros')
BEGIN
    CREATE TABLE [Libros] (
        [Id] int NOT NULL IDENTITY,
        [Titulo] nvarchar(max) NULL,
        [AutorId] int NOT NULL,
        CONSTRAINT [PK_Libros] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Libros_Autores_AutorId] FOREIGN KEY ([AutorId]) REFERENCES [Autores] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230620180204_Libros')
BEGIN
    CREATE INDEX [IX_Libros_AutorId] ON [Libros] ([AutorId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230620180204_Libros')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230620180204_Libros', N'7.0.7');
END;
GO

COMMIT;
GO

