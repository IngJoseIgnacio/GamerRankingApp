	IF NOT EXISTS ( SELECT name FROM sys.databases WHERE name = N'GamingRankingDB')
	BEGIN
	    EXEC('CREATE DATABASE GamingRankingDB');
	    PRINT 'Base de datos creada exitosamente.';
	END
	ELSE
	BEGIN
	    PRINT 'La base de datos ya existe.';
	END

       -- Eliminar el procedimiento si ya existe
    IF OBJECT_ID('dbo.GenerateRandomCalificaciones', 'P') IS NOT NULL
        DROP PROCEDURE dbo.GenerateRandomCalificaciones;
    GO

    CREATE PROCEDURE [dbo].[GenerateRandomCalificaciones]
        @cantidad INT = 0, -- Parámetro de entrada: cantidad de calificaciones a generar
        @ErrorCode INT OUTPUT, -- Parámetro de salida: código de error (0 para éxito)
        @ErrorMessage NVARCHAR(MAX) OUTPUT -- Parámetro de salida: mensaje de error
    AS
    BEGIN
        SET NOCOUNT ON; -- Suprime los mensajes de "filas afectadas"

        -- Inicializar parámetros de salida
        SET @ErrorCode = 0;
        SET @ErrorMessage = NULL;

        BEGIN TRY
            -- Validación del parámetro de entrada
            IF @cantidad <= 0
            BEGIN
                SET @ErrorCode = 1;
                SET @ErrorMessage = 'El valor ingresado para la cantidad de calificaciones no es válido. Debe ser un entero positivo mayor a cero.';
                RETURN;
            END

            -- Verificar si hay Videojuegoes disponibles para calificar
            DECLARE @videojuegoCount INT;
            SELECT @videojuegoCount = COUNT(Id) FROM Videojuegoes;

            IF @videojuegoCount = 0
            BEGIN
                SET @ErrorCode = 2;
                SET @ErrorMessage = 'No hay Videojuegoes en la base de datos para asignar calificaciones. Por favor, asegúrese de que la tabla Videojuegoes esté poblada.';
                RETURN;
            END

            -- Tabla temporal para almacenar IDs de Videojuegoes para una selección eficiente
            CREATE TABLE #TempVideojuegoIds (
                RowNum INT IDENTITY(1,1),
                VideojuegoId INT
            );

            INSERT INTO #TempVideojuegoIds (VideojuegoId)
            SELECT Id FROM Videojuegoes;

            DECLARE @minRowNum INT = 1;
            DECLARE @maxRowNum INT;
            SELECT @maxRowNum = MAX(RowNum) FROM #TempVideojuegoIds;

            -- Variables para el bucle
            DECLARE @i INT = 0;
            DECLARE @randomVideojuegoId INT;
            DECLARE @randomNickname NVARCHAR(30);
            DECLARE @randomPuntuacion DECIMAL(3,2);
            DECLARE @chars NVARCHAR(62) = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
            DECLARE @charCount INT = 62;

            WHILE @i < @cantidad
            BEGIN
                -- Generar Nickname aleatorio (hasta 30 caracteres, letras y números)
                SET @randomNickname = '';
                DECLARE @nicknameLength INT = FLOOR(RAND() * 25) + 5; -- Longitud entre 5 y 30

                DECLARE @j INT = 0;
                WHILE @j < @nicknameLength
                BEGIN
                    SET @randomNickname = @randomNickname + SUBSTRING(@chars, CAST(RAND() * @charCount AS INT) + 1, 1);
                    SET @j = @j + 1;
                END;

                -- Generar Puntuacion aleatoria (0 a 5, con máximo dos decimales)
                -- Multiplicamos por 500, truncamos, y dividimos por 100 para 2 decimales
                SET @randomPuntuacion = CAST(FLOOR(RAND() * 501) AS DECIMAL(5,0)) / 100.00;

                -- Obtener un VideojuegoId aleatorio de la tabla temporal
                SELECT TOP 1 @randomVideojuegoId = VideojuegoId
                FROM #TempVideojuegoIds
                WHERE RowNum = CAST(RAND() * (@maxRowNum - @minRowNum + 1) + @minRowNum AS INT);

                -- Insertar la nueva calificación
                INSERT INTO Calificacions (Id, NicknameJugador, VideojuegoId, Puntuacion, FechaActualizacion, UsuarioActualizacion)
                VALUES (NEWID(), @randomNickname, @randomVideojuegoId, @randomPuntuacion, GETDATE(), 'SP_Calificaciones');

                SET @i = @i + 1;
            END;

            -- Eliminar la tabla temporal
            DROP TABLE #TempVideojuegoIds;

        END TRY
        BEGIN CATCH
            -- Capturar información del error
            SET @ErrorCode = ERROR_NUMBER();
            SET @ErrorMessage = ERROR_MESSAGE();

            -- Limpieza si la tabla temporal aún existe debido a un error
            IF OBJECT_ID('tempdb..#TempVideojuegoIds') IS NOT NULL
                DROP TABLE #TempVideojuegoIds;
        END CATCH
    END;
    GO

    -- Script para ejecutar el procedimiento y generar 1,000,000 de registros
    -- Este bloque solo se ejecuta una vez para la generación masiva.
    DECLARE @outputErrorCode INT;
    DECLARE @outputErrorMessage NVARCHAR(MAX);
    DECLARE @recordsToGenerate INT = 1000000; -- Cantidad de calificaciones a generar

    PRINT 'Iniciando la generación de ' + CAST(@recordsToGenerate AS NVARCHAR(MAX)) + ' calificaciones...';
    EXEC [dbo].[GenerateRandomCalificaciones]
        @cantidad = @recordsToGenerate,
        @ErrorCode = @outputErrorCode OUTPUT,
        @ErrorMessage = @outputErrorMessage OUTPUT;

    SELECT
        CASE
            WHEN @outputErrorCode = 0 THEN 'Éxito'
            ELSE 'Error'
        END AS Estado,
        @outputErrorCode AS CodigoError,
        @outputErrorMessage AS MensajeError;

    PRINT 'Proceso de generación finalizado.';
    