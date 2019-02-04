ALTER PROCEDURE scwsp_Tb_Usuario_ListaByPuntoVenta @Codi_puntoVenta SMALLINT
AS
BEGIN
	SET NOCOUNT ON

	SELECT Codi_Usuario
		,LOGIN AS Usuario
	FROM Tb_Usuario
	WHERE Codi_puntoVenta = 6
	ORDER BY CODI_USUARIO

	SET NOCOUNT OFF
END
GO


