ALTER PROCEDURE scwsp_GrabarEmpresa (
	@Ruc_Cliente VARCHAR(11)
	,@Razon_Social VARCHAR(50)
	,@Direccion VARCHAR(50)
	,@Telefono VARCHAR(15)
	)
AS
BEGIN
	BEGIN TRANSACTION

	INSERT INTO Tb_Ruc (
		Ruc_Cliente
		,Razon_Social
		,Direccion
		,Telefono
		)
	VALUES (
		@Ruc_Cliente
		,Upper(@Razon_Social)
		,@Direccion
		,@Telefono
		)

	IF @@Error <> 0
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION
END
GO


