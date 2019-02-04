ALTER PROCEDURE scwsp_BuscarEmpresa (
	@Ruc_Cliente VARCHAR(11)
)
AS
BEGIN
	SELECT Ruc_Cliente
		,Razon_Social
		,Direccion
		,Telefono
	FROM Tb_Ruc
	WHERE Ruc_Cliente = @Ruc_Cliente
END
GO


