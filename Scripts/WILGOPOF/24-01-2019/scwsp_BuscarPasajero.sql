ALTER PROCEDURE scwsp_BuscarPasajero (
	@Tipo_Doc_Id CHAR(1)
	,@Numero_Doc VARCHAR(15)
	)
AS
BEGIN
	SELECT Id_Clientes
		,Tipo_Doc_id
		,Numero_Doc
		,Nombre_Clientes
		,Apellido_P
		,Apellido_M
		,fec_nac
		,edad
		,Direccion
		,telefono
		,ruc_contacto
	FROM Tb_Cliente_Pasajes
	WHERE Tipo_Doc_id = @Tipo_Doc_Id
		AND Numero_Doc = @Numero_Doc
END
GO


