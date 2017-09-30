use DForm;
go
create procedure DForm.sQuestionBaseCreation
(
	@ParentQuestionId  int,
	@FormId int,
	@QuestionIdResult int output,
	@Title nvarchar(128)
)
as
begin
	declare @NewQuestionId int;
	Insert Into DForm.tQuestionBase( FormId, ParentId, Title) values (@FormId, @ParentQuestionId, @Title);
	select @NewQuestionId = SCOPE_IDENTITY()
	insert into  DForm.tClosure (ParentId, ChildId)
		select c.ParentId, @NewQuestionId
		from DForm.tClosure c
		where c.ChildId = @ParentQuestionId
		union all 
		select @NewQuestionId, @NewQuestionId
end
Go
create procedure DForm.sQuestionBaseDelete
(
	@QuestionId int
)
as 
begin
	declare @AnswerId int;
	declare @ToDestroy table(childId int);
	select @AnswerId = ab.AnswerId from DForm.tAnswerBase ab where ab.QuestionId = @QuestionId;
	delete from DForm.tOpenAnswer where  DForm.tOpenAnswer.AnswerId = @AnswerId;
	delete from DForm.tAnswerBase where  DForm.tAnswerBase.AnswerId = @AnswerId;
	delete from DForm.tOpenQuestion where  DForm.tOpenQuestion.QuestionId = @QuestionId;
	insert into @ToDestroy
		select from DForm.tClosure c where c.ParentId = @QuestionId;
	delete from DForm.tClosure where DForm.tClosure.ParentId = @QuestionId and DForm.tClosure.ChildId = @QuestionId;
	delete from DForm.tQuestionBase qb where qb.QuestionId in ( 
		select childId from @ToDestroy
	);
end
Go