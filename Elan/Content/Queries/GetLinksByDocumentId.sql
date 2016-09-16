select
	Id,
	DocumentId,
	StartNodeId,
	EndNodeId,
	Label,
	StartPointX,
	StartPointY,
	EndPointX,
	EndPointY
	from dbo.Link
	where DocumentId = @documentId;