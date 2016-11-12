use elan;
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