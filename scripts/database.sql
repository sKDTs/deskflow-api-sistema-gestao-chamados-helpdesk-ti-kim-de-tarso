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
CREATE TABLE [Categorias] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
);

CREATE TABLE [Chamados] (
    [Id] int NOT NULL IDENTITY,
    [Titulo] nvarchar(200) NOT NULL,
    [Descricao] nvarchar(max) NOT NULL,
    [Prioridade] int NOT NULL,
    [Status] int NOT NULL,
    [SolicitanteNome] nvarchar(150) NOT NULL,
    [DataAbertura] datetime2 NOT NULL,
    [DataFechamento] datetime2 NULL,
    [Solucao] nvarchar(2000) NULL,
    [CategoriaId] int NOT NULL,
    CONSTRAINT [PK_Chamados] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Chamados_Categorias_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Interacoes] (
    [Id] int NOT NULL IDENTITY,
    [ChamadoId] int NOT NULL,
    [Autor] nvarchar(150) NOT NULL,
    [Mensagem] nvarchar(2000) NOT NULL,
    [DataRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_Interacoes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Interacoes_Chamados_ChamadoId] FOREIGN KEY ([ChamadoId]) REFERENCES [Chamados] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Chamados_CategoriaId] ON [Chamados] ([CategoriaId]);

CREATE INDEX [IX_Interacoes_ChamadoId] ON [Interacoes] ([ChamadoId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261005182430_InitialCreate', N'10.0.9');

COMMIT;
GO

