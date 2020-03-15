 CREATE TRIGGER [deleteCheck] ON dbo.[check] --Указывается таблица, для которой будет действовать триггер
 FOR DELETE --Действие, на которое будет срабатывать триггер
 AS
 IF SYSTEM_USER = 'user' OR  SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления