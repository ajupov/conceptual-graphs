use elan;

if (not exists (
	select
		TABLE_NAME
        from information_schema.tables 
        where TABLE_SCHEMA = 'dbo' 
			and TABLE_NAME = 'Document'))
begin
	create table dbo.Document
	(
		Id		int identity (1, 1) not null,
		Name	nvarchar(max) not null,
		Width	int not null,
		Height	int not null
	);
end;
go

if (not exists (
	select
		TABLE_NAME
        from information_schema.tables 
        where TABLE_SCHEMA = 'dbo' 
			and TABLE_NAME = 'Node'))
begin
	create table dbo.Node
	(
		Id			int not null,
		DocumentId	int not null,
		Type		int not null,
		Label		nvarchar(max) not null,
		X			int not null,
		Y			int not null,
		Width		int not null,
		Height		int not null
	);
end;
go

if (not exists (
	select
		TABLE_NAME
        from information_schema.tables 
        where TABLE_SCHEMA = 'dbo' 
			and TABLE_NAME = 'Link'))
begin
	create table dbo.Link
	(
		Id			int not null,
		DocumentId	int not null,
		StartNodeId	int not null,
		EndNodeId	int not null,
		Label		nvarchar(max) not null,
		StartPointX	int not null,
		StartPointY	int not null,
		EndPointX	int not null,
		EndPointY	int not null
	);
end;
go