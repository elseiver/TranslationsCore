WITH ORDERED_BY_ID AS
	(
	SELECT
		id
	,   ROW_NUMBER() OVER (PARTITION BY [key], CultureId ORDER BY Id DESC) AS rn
	FROM
		Translation
	)
	DELETE FROM Translation
	WHERE Id IN (
	SELECT
		Id
	FROM
		ORDERED_BY_ID
	WHERE
		rn > 1
	);

