﻿CREATE TABLE [tblGeneros] (
    [Id] TINYINT identity(1,1) NOT NULL,
	[Descricao] varchar(128) NOT NULL,
    CONSTRAINT [PK_Genero] PRIMARY KEY ([Id]),
);