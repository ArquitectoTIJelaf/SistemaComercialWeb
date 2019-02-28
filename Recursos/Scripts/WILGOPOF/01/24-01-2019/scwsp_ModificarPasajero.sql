CREATE PROCEDURE scwsp_ModificarPasajero (
	@Id_Clientes INT
	,@Tipo_Doc_Id CHAR(1)
	,@Numero_Doc VARCHAR(15)
	,@Nombre_Clientes VARCHAR(100)
	,@Apellido_P VARCHAR(50)
	,@Apellido_M VARCHAR(50)
	,@fec_nac SMALLDATETIME
	,@edad TINYINT
	,@Direccion VARCHAR(100)
	,@telefono VARCHAR(15)
	,@ruc_contacto VARCHAR(11)
	,@sexo CHAR(1)
	)
AS
BEGIN
	BEGIN TRANSACTION

	UPDATE Tb_Cliente_Pasajes
	SET Tipo_Doc_id = @Tipo_Doc_Id
		,Numero_Doc = @Numero_Doc
		,Nombre_Clientes = @Nombre_Clientes
		,Apellido_P = @Apellido_P
		,Apellido_M = @Apellido_M
		,fec_nac = @fec_nac
		,edad = @edad
		,Direccion = @Direccion
		,telefono = @telefono
		,ruc_contacto = @ruc_contacto
		,sexo = @sexo
	WHERE Id_Clientes = @Id_Clientes

	IF @@ERROR <> 0
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION
END
GO


