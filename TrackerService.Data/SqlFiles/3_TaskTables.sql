if not exists(select * from sysobjects where name = 'tasks' and xtype = 'U')
begin
	create table dbo.task(
		id bigint identity(1, 1),
		public_id nvarchar(64) not null,
		parent_id bigint,
		tenant_id nvarchar(64) not null,
		title nvarchar(500) not null,
		[description] ntext,
		due_date datetime,
		[status] nvarchar(30) not null,
		[type] nvarchar(30) default 'general' not null,
		resolved bit,
		created_date datetime not null,
		created_by nvarchar(64) not null,
		modified_date datetime not null,
		modified_by nvarchar(64) not null,

		constraint pk_task_id primary key (id),
		constraint fk_task_parent foreign key (parent_id) references task(id)
	);
end
go

if not exists(select * from sysobjects where name = 'task_assignment' and xtype = 'U')
begin
	create table dbo.task_assignment(
		id int identity(1, 1),
		task_id bigint not null,
		entity_type nvarchar(30) default 'user' not null,
		[entity_id] nvarchar(64) not null,
		created_date datetime not null,
		created_by nvarchar(64) not null,

		constraint pk_task_assignment_id primary key (id),
		constraint fk_task_assignment_task_id foreign key (task_id) references task(id)
	);
end
go

if not exists(select * from sysobjects where name = 'task_context' and xtype = 'U')
begin
	create table dbo.task_context(
		id bigint identity(1, 1),
		task_id bigint not null,
		[key] nvarchar(100) not null,
		[value] ntext,
		created_date datetime not null,
		created_by nvarchar(64) not null,

		constraint pk_task_context_id primary key (id),
		constraint fk_task_context_task_id foreign key (task_id) references task(id)
	)
end
go

if not exists(select * from sysobjects where name = 'task_history' and xtype = 'U')
begin
	create table dbo.task_history(
		id bigint identity(1, 1),
		public_id nvarchar(64) not null,
		task_id bigint not null,
		action_type nvarchar(30) not null,
		created_date datetime not null,
		created_by nvarchar(64) not null,

		constraint pk_task_history_id primary key (id),
		constraint fk_task_history_task_ foreign key (task_id) references task(id)
	)
end
go
