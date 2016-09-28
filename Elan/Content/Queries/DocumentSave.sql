if(not exists (
	select
		Id
		from dbo.Document
		where Name = @Name)
	) begin
insert into dbo.Document
	values(@Name);
	select scope_identity();
end
else begin
	select Id from dbo.Document where Name = @Name;
end;