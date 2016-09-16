if(not exists (
	select
		Id
		from dbo.Document
		where Name = @Name)
	) begin
insert into dbo.Document
	values(@Name, @Width, @Height);
	select scope_identity();
end
else begin
	update dbo.Document
		set Width = @Width,
			Height = @Height
		where Name = @Name;
	select Id from dbo.Document where Name = @Name;
end;