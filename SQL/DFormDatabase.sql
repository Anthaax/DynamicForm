use DForm;
drop table DForm.tClosure;
drop table DForm.tOpenAnswer;
drop table DForm.tAnswerBase;
drop table DForm.tFormAnswer;
drop table DForm.tOpenQuestion;
drop table DForm.tQuestionBase;
drop table DForm.tForm;
drop table UM.tUser;
Go
create schema DForm;
Go
create schema UM;
Go
create table UM.tUser
(
	UserId int not null identity(0,1),
	UserName nvarchar(255) collate Latin1_General_CI_AI not null,
	CreationDate datetime2(3) constraint DF_UM_tForm_CreationDate default(sysutcdatetime()),

	constraint PK_UM_tUser primary key ( UserId ),
	constraint UK_UM_tUser_Username unique ( UserName ),
);
insert into UM.tUser(UserName) values( N'');
insert into UM.tUser(UserName) values( N'System');
Go
create table DForm.tForm
(
	FormId int not null identity(0,1),
	CreatorId int not null,
	CreationDate datetime2(3) constraint DF_UM_tForm_CreationDate default(sysutcdatetime()),

	constraint PK_DForm_tForm primary key ( FormId ),
	constraint FK_DForm_tForm_CreatorId foreign key( CreatorId ) references UM.tUser( UserId )
);
insert into DForm.tForm( CreatorId ) values( 0 );
Go
create table DForm.tQuestionBase
(
	QuestionId int not null identity(0,1),
	FormId int not null,
	ParentId int not null,
	Title nvarchar(255) not null,

	constraint PK_DForm_tQuestionBase primary key ( QuestionId ),
	constraint FK_DForm_tQuestionBase_FormId foreign key ( FormId ) references DForm.tForm ( FormId ) 
);
insert into DForm.tQuestionBase( FormId, ParentId, Title ) values( 0, 0, N'' );
Go
create table DForm.tClosure
(
	ParentId int not null,
	ChildId int not null,

	constraint FK_DForm_tClosure_ParentId foreign key ( ParentId ) references DForm.tQuestionBase ( QuestionId ),
	constraint FK_DForm_tClosure_ChildId foreign key ( ChildId ) references DForm.tQuestionBase ( QuestionId ),
);
Go
create table DForm.tOpenQuestion
(
	QuestionId int not null,
	EmptyAnswer bit,

	constraint FK_DForm_tOpenQuestion_QuestionId foreign key ( QuestionId ) references DForm.tQuestionBase ( QuestionId )
);
Go
create table DForm.tFormAnswer
(
	FormAnswerId int not null identity(0,1),
	FormId int not null,
	UserId int not null,
	Notation int not null,

	constraint PK_DForm_tFormAnswer primary key ( FormAnswerId ),
	constraint FK_DForm_tFormAnswer_UserId foreign key( UserId ) references UM.tUser( UserId ),
	constraint FK_DForm_tFormAnswer_FormId foreign key ( FormId ) references DForm.tForm ( FormId ) 
);
Go
create table DForm.tAnswerBase 
(
	AnswerId int not null identity(0,1),
	FormAnswerId int not null,
	QuestionId int not null

	constraint PK_DForm_tAnswerBase primary key ( AnswerId ),
	constraint FK_DForm_tAnswerBase_FormAnswerId foreign key ( FormAnswerId ) references DForm.tFormAnswer ( FormAnswerId ),
	constraint FK_DForm_tAnswerBase_QuestionId foreign key ( QuestionId ) references DForm.tQuestionBase ( QuestionId )
);
Go
create table DForm.tOpenAnswer 
(
	AnswerId int not null identity(0,1),

	constraint PK_DForm_tOpenAnswer primary key ( AnswerId ),
	constraint FK_DForm_tOpenAnswer_AnswerId foreign key ( AnswerId ) references DForm.tAnswerBase ( AnswerId )
);
Go
create view DForm.vQuestion
as
    select  q.QuestionId,
            q.Title,
            q.FormId,
            q.ParentId,
            Depth = (select count(*) from DForm.tClosure qb where q.QuestionId = ChildId),
			ChildCount = (select count(*) from DForm.tClosure qb where  q.QuestionId = ParentId ),
            AnswerCount = (select count(*) from DForm.tAnswerBase where QuestionId = q.QuestionId )
	from DForm.tQuestionBase q
Go
create view DForm.vAnswer
as 
	select	ab.AnswerId,
			ab.FormAnswerId,
			ab.QuestionId,
			FormId = (select fa.FormId from DForm.tFormAnswer fa where fa.FormAnswerId = ab.FormAnswerId )
	from DForm.tAnswerBase ab
Go
create view DForm.vForm
as 
	select	f.FormId,
			f.CreatorId,
			QuestionsCount = ( select count(*) from DForm.tQuestionBase qb where qb.FormId = f.FormId ),
			FormAnswersCount = ( select count(*) from DForm.tFormAnswer fa where fa.FormId = f.FormId )
	from DForm.tForm f
GO
create view UM.vUser
as 
	select	u.UserId,
			u.UserName,
			u.CreationDate,
			FormAnswerCount = ( select count(*) from DForm.tFormAnswer fa where fa.UserId = u.UserId ),
			AverageForm = ( select AVG(fa.Notation) from DForm.tFormAnswer fa where fa.UserId = u.UserId ) 

	from UM.tUser u