
truncate table [hss].[GroupMembers];
GO;
truncate table [hss].[Friends];
GO;
DELETE FROM [hss].[Groups];
DBCC CHECKIDENT ('ssh-db.hss.Groups', RESEED, 0)
GO;

insert into [hss].[Groups]([Name]) values('Hello World');
insert into [hss].[Groups]([Name]) values('Test Group');

insert into [hss].[GroupMembers](UserId, GroupId) values ('2c8c3408-849d-4f91-beab-0ca1af147f07', 1);
insert into [hss].[GroupMembers](UserId, GroupId) values ('370bb376-12ba-47c8-8250-f1b9744a1677', 1);
insert into [hss].[GroupMembers](UserId, GroupId) values ('a65eb99f-eaaf-4051-be30-53bee1084ba8', 1);

insert into [hss].[GroupMembers](UserId, GroupId) values ('2c8c3408-849d-4f91-beab-0ca1af147f07', 2);
insert into [hss].[GroupMembers](UserId, GroupId) values ('370bb376-12ba-47c8-8250-f1b9744a1677', 2);
insert into [hss].[GroupMembers](UserId, GroupId) values ('a65eb99f-eaaf-4051-be30-53bee1084ba8', 2);

insert into [hss].[Friends](UserId, FriendId) values('2c8c3408-849d-4f91-beab-0ca1af147f07', '370bb376-12ba-47c8-8250-f1b9744a1677'); --16 & lei
insert into [hss].[Friends](UserId, FriendId) values('2c8c3408-849d-4f91-beab-0ca1af147f07', 'a65eb99f-eaaf-4051-be30-53bee1084ba8'); --16 & 15

insert into [hss].[Friends](UserId, FriendId) values('a65eb99f-eaaf-4051-be30-53bee1084ba8', '370bb376-12ba-47c8-8250-f1b9744a1677'); --15 & lei
insert into [hss].[Friends](UserId, FriendId) values('a65eb99f-eaaf-4051-be30-53bee1084ba8', '2c8c3408-849d-4f91-beab-0ca1af147f07'); --15 & 16

insert into [hss].[Friends](UserId, FriendId) values('370bb376-12ba-47c8-8250-f1b9744a1677', 'a65eb99f-eaaf-4051-be30-53bee1084ba8'); --lei & 15
insert into [hss].[Friends](UserId, FriendId) values('370bb376-12ba-47c8-8250-f1b9744a1677', '2c8c3408-849d-4f91-beab-0ca1af147f07'); --lei & 16