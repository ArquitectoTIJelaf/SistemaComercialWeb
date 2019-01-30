ALTER PROCEDURE scwsp_ListarPuntosVenta
AS
BEGIN
	SET NOCOUNT ON

	SELECT Codi_Sucursal
		,Codi_puntoVenta
		,Descripcion
	FROM Tb_PuntoVenta

	SET NOCOUNT OFF
END
GO


