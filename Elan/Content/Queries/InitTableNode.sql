use elan;
if (not exists (
	select
		TABLE_NAME
        from information_schema.tables 
        where TABLE_SCHEMA = 'dbo' 
			and TABLE_NAME = 'Node'))
begin
	create table dbo.Node
	(
		Id			bigint not null,
		DocumentId	bigint not null,
		Type		int not null,
		Label		nvarchar(max) not null,
		X			int not null,
		Y			int not null,
		Width		int not null,
		Height		int not null
	);
end;