IF OBJECT_ID('dbo.usp_AddLog') IS NOT NULL 
	DROP PROCEDURE [dbo].[usp_AddLog]
GO

	CREATE PROCEDURE [dbo].[usp_AddLog]
	(
		@_InBox_ReadID					tinyint					,		-- 執行 Log 時是使用第幾版
		@_InBox_SPNAME					nvarchar(120)		,		-- 執行的預存程序名稱
		@_InBox_GroupID					uniqueidentifier	,		-- 執行群組代碼
		@_InBox_ExProgram				nvarchar(40)			,		-- 執行的動作是什麼
		@_InBox_ActionJSON			nvarchar(Max)		,		-- 執行的過程是什麼
		@_OutBox_ReturnValues		nvarchar(Max)		output -- 回傳執行的項目
	) 
	AS
	
	-- ========================= 新增與維護注意事項(必須遵守規定) =====================
	-- 相關注解說明請寫在這裡，以免從 Visual Studio 轉至 SQL 說明內容沒有一起上去
	-- 如果要修改請以這檔案為主，並以 SSMS 19.0 版以上來修改以便有完整的編輯模式
	-- 編輯時請交由「專案人員」來進行相關的修正，修改前也請確定在 C# Class 內，
	-- 有那些程序有〔參考〕到並再加以確定修改後不會有任何影響，再行修正以下 TSQL 的語法，以免
	-- 你修改後，會使得其他程序也有引用到以下的資料而有所影響。
	-- ==========================================================================
	-- 指定檔案　：usp_AddLog 記錄執行的動作.sql
	-- 專案項目　：
	-- 專案用途　：記錄 sp 執行的動作為何
	-- 專案資料庫：
	-- 專案資料表：
	-- 專案人員　：
	-- 專案日期　：
	-- 專案說明　：@_InBox_ReadID				最主要是拿來做記錄版本使用，因為 Log 記錄往後會以不同的內容或是執行的程序不一定會有不同的記錄內容
	-- 　　　　　：@_InBox_ActionJSON		是記錄執行的項目與內容以利有往後擴充的保留
	-- 　　　　　：@_InBox_GroupID				最主要在執行過程會有一整段的過程，可以透過這 GUID 可以了解到整個執行過程，不然其他項目也記錄時
	--																			都不知道執行到那一個項目了??
	-- =========================================================================

	--======= 宣告預設的變數	================
	DECLARE @_StoredProgramsNAME nvarchar(100) = 'usp_AddLog' -- 執行項目
	
	--======= 宣告要儲存的表格	================
	DECLARE @_ReturnTable Table 
	(
		[RT_Status]					bit							,		--執行結果
		[RT_ActionValues]		nvarchar(2000)			--回傳結果為何
	) 
	
	--======= 執行行為與動作	================
	-- @_InBox_ServiceID				=	0
	-- 單純記錄一筆執行內容
	--=====================================

	if(@_InBox_ReadID = 0) 
		Begin

		INSERT INTO MyOffice_ExcuteionLog 
		(
			DeLog_StoredPrograms,
			DeLog_GroupID,
			DeLog_ExecutionProgram,
			DeLog_ExecutionInfo
		)
		Values
		(
			@_InBox_SPNAME,
			@_InBox_GroupID,
			@_InBox_ExProgram,
			@_InBox_ActionJSON
		)

		SET	@_OutBox_ReturnValues =
		(
				SELECT
						Top 100 
						DeLog_AutoID											AS 'AutoID',
						DeLog_ExecutionProgram					AS 'NAME',
						DeLog_ExecutionInfo							AS 'Action',
						DeLog_ExDateTime								AS 'DateTime'
				FROM
						MyOffice_ExcuteionLog						WITH(NOLOCK)
				WHERE
					DeLog_GroupID = @_InBox_GroupID
		
				ORDER BY
					DeLog_AutoID FOR json PATH,
					ROOT('ProgramLog'),
					INCLUDE_NULL_VALUES
		) 
	
		RETURN

End