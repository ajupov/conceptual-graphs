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
		Id		bigint identity (1, 1) not null,
		Name	nvarchar(max) not null
	);
end;