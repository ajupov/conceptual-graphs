select
	Id,
	DocumentId,
	Type,
	Label,
	X,
	Y,
	Width,
	Height
	from dbo.Node
	where DocumentId = @documentId;