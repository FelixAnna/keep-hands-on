
truncate table [hss].[GroupMembers];
GO;
truncate table [hss].[Friends];
GO;
DELETE FROM [hss].[Groups];
DBCC CHECKIDENT ('ssh-db.hss.Groups', RESEED, 0)
GO;

insert into [hss].[Groups]([Name]) values('Hello World');
insert into [hss].[Groups]([Name]) values('Test Group');

--EPAM
--XXX

-- add support for different tenants
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
Values ('asupportid','Support', 'SUPPORT', '70962115-06d7-49f9-9774-c99cda0abb96');

-- fake data
/*
TRUNCATE TABLE [hss].[GroupMembers];

TRUNCATE TABLE [hss].[Friends];

TRUNCATE TABLE [hss].[MESSAGES];

TRUNCATE TABLE [dbo].[AspNetUserRoles];
*/
insert into [hss].[GroupMembers](UserId, GroupId)
select Id, 1
from(
	select row_number() over (partition by tenantId order by Email) as Num, *
	from [dbo].[AspNetUsers]
	where ISADHOC=0
) t where t.Num<=3;

insert into [hss].[GroupMembers](UserId, GroupId)
select Id, 2
from(
	select row_number() over (partition by tenantId order by Email) as Num, *
	from [dbo].[AspNetUsers]
	where ISADHOC=0
) t where t.Num<=2;

insert into [hss].[Friends](UserId, FriendId)
select gm1.UserId, gm2.UserId
from 
[hss].[GroupMembers] gm1, [hss].[GroupMembers] gm2
where gm1.GroupId=2 and gm2.GroupId=2 and gm1.UserId <> gm2.UserId

-- add default support for different tenants
INSERT INTO [dbo].[AspNetUserRoles]
select Id, 'asupportid'
from(
	select row_number() over (partition by tenantId order by Email) as Num, *
	from [dbo].[AspNetUsers]
	where ISADHOC=0
) t where t.Num<=2;