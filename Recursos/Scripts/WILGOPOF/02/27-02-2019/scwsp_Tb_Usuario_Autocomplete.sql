SET QUOTED_IDENTIFIER OFF
GO

ALTER PROC scwsp_Tb_Usuario_Autocomplete --scwsp_Tb_Usuario_Autocomplete 'JULIO'
	@LOGIN VARCHAR(30)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @DynamicQuery NVARCHAR(MAX);

	SET @DynamicQuery = "SELECT Codi_Usuario, LOGIN AS Login FROM Tb_Usuario WHERE" + CASE ISNUMERIC(@LOGIN)
			WHEN 0
				THEN ("(Login LIKE '%" + @LOGIN + "%')")
			WHEN 1
				THEN ("(Codi_Usuario LIKE '" + @LOGIN + "%')")
			END + " ORDER BY Login ASC OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY";

	EXECUTE sp_executesql @DynamicQuery
	SET NOCOUNT OFF
END
GO