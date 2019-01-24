ALTER PROCEDURE scwsp_GrabarPasajero (
	@Tipo_Doc_Id CHAR(1)
	,@Numero_Doc VARCHAR(15)
	,@Nombre_Clientes VARCHAR(100)
	,@Apellido_P VARCHAR(50)
	,@Apellido_M VARCHAR(50)
	,@fec_nac SMALLDATETIME
	,@edad TINYINT
	,@Direccion VARCHAR(100)
	,@telefono VARCHAR(15)
	,@ruc_contacto VARCHAR(11)
	,@Id_Clientes INT OUTPUT
	)
AS
BEGIN
	BEGIN TRANSACTION

	INSERT INTO Tb_Cliente_Pasajes (
		Tipo_Doc_id
		,Numero_Doc
		,Nombre_Clientes
		,Apellido_P
		,Apellido_M
		,fec_nac
		,edad
		,Direccion
		,telefono
		,ruc_contacto
		)
	VALUES (
		@Tipo_Doc_Id
		,@Numero_Doc
		,@Nombre_Clientes
		,@Apellido_P
		,@Apellido_M
		,@fec_nac
		,@edad
		,@Direccion
		,@telefono
		,@ruc_contacto
		)

	SET @Id_Clientes = SCOPE_IDENTITY()

	IF @@ERROR <> 0
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION
END
GO


