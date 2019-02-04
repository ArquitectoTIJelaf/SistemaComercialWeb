ALTER PROCEDURE scwsp_ListarPuntosVenta (@Codi_Sucursal SMALLINT)
AS
BEGIN
	SET NOCOUNT ON

	SELECT Codi_Sucursal
		,Codi_puntoVenta
		,Descripcion
	FROM Tb_PuntoVenta
	WHERE Codi_Sucursal = @Codi_Sucursal

	SET NOCOUNT OFF
END
GO


