select
	convert(bit, iif(exists(
		select
			Id
			from dbo.Document
			where Name = @name), 1, 0));